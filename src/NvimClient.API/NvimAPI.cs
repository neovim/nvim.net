using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using MsgPack;
using MsgPack.Serialization;
using NvimClient.NvimMsgpack.Models;

namespace NvimClient.API;

/// <summary>
/// The C# exposed nvim api this is spread into two files one of which is
/// generated.
/// </summary>
public sealed partial class NvimAPI : IDisposable {
    /// <summary>
    /// An event that is fired when a nvim message is not handled by an NvimAPI instance
    /// </summary>
    public event EventHandler<NvimUnhandledRequestEventArgs>? OnUnhandledRequest;

    /// <summary>
    /// An event that is fired when a notification is not handled by an NvimAPI instance
    /// </summary>
    public event EventHandler<NvimUnhandledNotificationEventArgs>? OnUnhandledNotification;

    /// <summary>
    /// The input stream through which Nvim Communicates. This is the stdin
    /// of nvim or a tcp stream. This produces <see cref="NvimResponse"/>
    /// an nvim process is reading this stream.
    /// </summary>
    private readonly Stream _nvim_input_stream;

    /// <summary>
    /// The output stream through which Nvim Communicates. This is the stdout
    /// of nvim or a tcp stream. This produces <see cref="NvimResponse"/>
    /// an nvim process is writing to this stream.
    /// </summary>
    private readonly Stream _nvim_output_stream;

    /// <summary>
    /// A serializer that serilizes <see cref="NvimRequest"/> towards neovim.
    /// </summary>
    private readonly MessagePackSerializer<NvimRequest> _request_serializer;

    /// <summary>
    /// A serializer that serializes <see cref="NvimResponse"/> towards neovim.
    /// </summary>
    private readonly MessagePackSerializer<NvimResponse> _response_serializer;

    /// <summary>
    /// A serializer that deserializes <see cref="NvimResponse"/> from neovim
    /// but nvim response has two different types.
    /// </summary>
    private readonly MessagePackSerializer<MessagePackObject> _response_mpo_serializer;

    private readonly BlockingCollection<NvimRequest> _requestQueue;
    private readonly BlockingCollection<NvimResponse> _responseQueue;

    private readonly ConcurrentDictionary<long, PendingRequest> _pendingOutgoingRequests;

    /// <summary>
    /// Defines a delegate for hanlding nvim requests
    /// </summary>
    private delegate void NvimHandler(uint? requestId, object[] arguments);

    /// <summary>
    /// Functions that handle nvim requests
    /// </summary>
    private readonly ConcurrentDictionary<string, NvimHandler> _handlers;
    private uint _messageIdCounter;
    private readonly ManualResetEvent _waitEvent = new(false);

    private readonly CancellationTokenSource cts;
    private readonly Task transmitTask;
    private readonly Task receiveTask;

    /// <summary>
    ///     Communicates with Nvim through the provided input and output streams.
    /// </summary>
    ///
    /// <param name="inputStream">
    ///     The input stream to use.
    /// </param>
    ///
    /// <param name="outputStream">
    ///     The output stream to use.
    /// </param>
    public NvimAPI(Stream inputStream, Stream outputStream) {
        SerializationContext context = new() {
            SerializationMethod = SerializationMethod.Array
        };
        _request_serializer = MessagePackSerializer.Get<NvimRequest>(context);
        _response_serializer = MessagePackSerializer.Get<NvimResponse>(context);
        _response_mpo_serializer = MessagePackSerializer.Get<MessagePackObject>(context);
        _nvim_input_stream = inputStream;
        _nvim_output_stream = outputStream;
        _requestQueue = [];
        _responseQueue = [];
        _pendingOutgoingRequests = new ConcurrentDictionary<long, PendingRequest>();
        _handlers = new ConcurrentDictionary<string, NvimHandler>();

        cts = new CancellationTokenSource();
        transmitTask = SendLoop(cts.Token);
        receiveTask = ReceiveLoop(cts.Token);
    }

    /// <summary>
    ///     Communicates with Nvim through the provided stream. This implies that
    ///     nvim reads and writes to the same stream. This is the case for TCP
    ///     connections. This contsuctor just passes the same stream to the input
    ///     and output streams.
    /// </summary>
    ///
    /// <param name="inputOutputStream">
    ///     The stream to use.
    /// </param>
    public NvimAPI(Stream inputOutputStream) : this(inputOutputStream, inputOutputStream) {
    }

    /// <summary>
    /// Communicates with an already-running Nvim process through its stdin and stdout streams.
    /// </summary>
    /// <param name="process">The already running nvim process</param>
    public NvimAPI(Process process) : this(process.StandardInput.BaseStream, process.StandardOutput.BaseStream) {
    }

    /// <summary>
    /// Communicates with Nvim through the specified server address.
    /// </summary>
    /// <param name="serverAddress"></param>
    public NvimAPI(string serverAddress) : this(GetStreamFromServerAddress(serverAddress)) {
    }

    private static Stream GetStreamFromServerAddress(string serverAddress) {
        int lastColonIndex = serverAddress.LastIndexOf(':');
        bool port_parsed = int.TryParse(serverAddress[(lastColonIndex + 1)..], out int port);

        if (lastColonIndex != -1 && lastColonIndex != 0 && port_parsed) {
            // TCP socket
            TcpClient tcpClient = new();
            string hostname = serverAddress[..lastColonIndex];
            tcpClient.Connect(hostname, port);
            return tcpClient.GetStream();
        }

        // Interprocess communication socket
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) {
            // Named Pipe on Windows
            Match match = MyRegex().Match(serverAddress);
            string serverName = match.Groups["serverName"].Value;
            string pipeName = match.Groups["pipeName"].Value;
            NamedPipeClientStream pipeStream = new(serverName, pipeName, PipeDirection.InOut, PipeOptions.Asynchronous);
            pipeStream.Connect();
            return pipeStream;
        }

        // Unix Domain Socket on other OSes
        Socket unixDomainSocket = new(AddressFamily.Unix, SocketType.Stream, ProtocolType.Unspecified);
        unixDomainSocket.Connect(new UnixDomainSocketEndPoint(serverAddress));
        return new NetworkStream(unixDomainSocket, true);
    }



    /// <summary>
    /// Registers a function that may return things to execute for the given name
    /// </summary>
    public void RegisterHandler(string name, Func<object[], object?> handler) {
        RegisterHandler(name, (Delegate)handler);
    }

    /// <summary>
    /// Registers a function that returns void to execute for the given name
    /// </summary>
    public void RegisterHandler(string name, Action<object[]> handler) {
        RegisterHandler(name, (Delegate)handler);
    }

    public void RegisterHandler(string name, Func<object[], Task<object>> handler) {
        RegisterHandler(name, (requestId, args) => {
            _ = Task.Run(() => {
                if (requestId.HasValue) {
                    CallHandlerAndSendResponse(requestId.Value, handler, args);
                } else {
                    _ = handler(args);
                }
            });
        });
    }

    private void RegisterHandler(string name, Delegate handler) {
        RegisterHandler(name, (requestId, args) => {
            if (requestId.HasValue) {
                CallHandlerAndSendResponse(requestId.Value, handler, args);
            } else {
                _ = handler.DynamicInvoke(args);
            }
        });
    }

    private void RegisterHandler(string name, NvimHandler handler) {
        if (!_handlers.TryAdd(name, handler)) {
            throw new InvalidOperationException($"Handler for \"{name}\" is already registered");
        }
    }

    private void CallHandlerAndSendResponse(uint requestId, Delegate handler, object[] args) {
        object? result = null;
        object? error = null;

        try {
            object? handlerResult = handler.DynamicInvoke([args]);
            if (handlerResult is Task task) {
                task.GetAwaiter().GetResult();
                if (handlerResult is Task<object> taskObject) {
                    result = taskObject.GetAwaiter().GetResult();
                }
            } else {
                result = handlerResult;
            }
        } catch (Exception exception) {
            error = exception.ToString();
        }

        SendResponse(requestId, result, error);
    }


    internal void SendResponse(NvimUnhandledRequestEventArgs args, object? result, object? error) {
        SendResponse(args.RequestId, result, error);
    }

    internal void SendResponse(uint msgid, object? result, object? error) {
        NvimResponse response = new() {
            Type = MsgPackDefinitions.ResponseTypeId,
            MsgId = msgid,
            Result = result.ToMessagePackObject(),
            Error = error.ToMessagePackObject()
        };
        _response_serializer.Pack(_nvim_input_stream, response);
        _nvim_input_stream.Flush();
    }



    /// <summary>
    /// Send one RPC request to Neovim and return a Task that completes when the
    /// matching NvimResponse arrives. This is performed in three ways:
    /// 1) Assign a message id
    /// 2) Create a <see cref="PendingRequest"/> object store it by id
    /// 3) Enqueue the request for the send loop
    /// 4) Return the task representing the future response
    /// </summary>
    ///
    /// <returns>
    ///     A task that when completed provides a response.
    /// </returns>
    private Task<NvimResponse?> ScheduleRequestSend(NvimRequest request) {
        request.MsgId = _messageIdCounter++;
        //Create a pending request to map the response type to the caller.
        PendingRequest pendingRequest = new(request);
        _pendingOutgoingRequests[request.MsgId] = pendingRequest;

        // Add the request to the queue in order for it to be trnasmissted.
        _requestQueue.Add(request);
        return pendingRequest.GetResponseReceptionTask();
    }

    /// <summary>
    /// This function does the same as <see cref="ScheduleRequestSend(NvimRequest)"/> but
    /// also converts the result to the given type.
    /// </summary>
    private async Task<TResult> SendAndReceive<TResult>(NvimRequest request) {
        NvimResponse? response = await ScheduleRequestSend(request);

        if (response is null) {
            throw new InvalidOperationException("Neovim request returned no response.");
        }

        object result = ConvertFromMessagePackObject(response.Result);

        if (typeof(TResult).IsArray) {
            object[] objectArray = (object[])result;
            Array resultArray = Array.CreateInstance(typeof(TResult).GetElementType()!, objectArray.Length);
            Array.Copy(objectArray, resultArray, resultArray.Length);
            return (TResult)(object)resultArray;
        }

        return (TResult)result;


    }

    /// <summary>
    /// A loop that continously sends data to the nvim instance.
    /// </summary>
    private async Task SendLoop(CancellationToken token) {
        Console.WriteLine("Starting Send Loop");

        //GetConsumingEnumerable(token) is a synchronous blocking consumer.
        //If the queue is empty, the foreach blocks the calling thread immediately.
        //Instead we yield allowing the others to execute.
        await Task.Yield(); //<-- Important

        try {
            // This blocks until an item is available, until CompleteAdding() is called,
            // or until the token is cancelled. It's better than a while loop with a
            // take in the following manner:
            //     It avoids InvalidOperationException
            //     It encodes shutdown semantics declaratively
            //     It is the idiomatic BlockingCollection consumer
            foreach (NvimRequest request in _requestQueue.GetConsumingEnumerable(token)) {
                await _request_serializer.PackAsync(_nvim_input_stream, request, token).ConfigureAwait(false);
                //Console.WriteLine("Sent Request: {0}", request);
            }
        } catch (OperationCanceledException) when (token.IsCancellationRequested) {
            // Normal shutdown path: cancellation requested.
        } finally {
            _ = _waitEvent.Set();
        }
    }

    private async Task ReceiveLoop(CancellationToken token) {

        Console.WriteLine("Starting Receive Loop");

        while (!token.IsCancellationRequested) {

            MessagePackObject obj;
            try {
                obj = await _response_mpo_serializer.UnpackAsync(_nvim_output_stream, token);
            } catch (OperationCanceledException) when (token.IsCancellationRequested) {
                Console.WriteLine("Receive Operation Canceled");
                break; // normal shutdown
            } catch (EndOfStreamException) {
                Console.WriteLine("Receive end of stream detected");
                break;
            }

            if (!obj.IsArray) {
                Console.WriteLine("Non array Response Received!");
                continue;
            }
            int t = (int)obj.AsList()[0];

            if (t is MsgPackDefinitions.ResponseTypeId) {
                NvimResponse? r = NvimResponse.FromMessagePackObject(obj);
                //Console.WriteLine("Received: {0}", r);
                HandleResponse(r);
            } else if (t is MsgPackDefinitions.NotificationTypeId) {
                NvimNotification? n = NvimNotification.FromMessagePackObject(obj);
                //Console.WriteLine("Received: {0}", n);
                HandleNotification(n);
            } else {
                NvimRequest? req = NvimRequest.FromMessagePackObject(obj);
                //Console.WriteLine("Received: {0}", req);
                HandleIncomingRequest(req);
            }
        }
    }

    private void HandleResponse(NvimResponse? response) {
        if (response is null) {
            return;
        }

        bool ok = _pendingOutgoingRequests.TryRemove(response.MsgId, out PendingRequest? pendingRequest);

        if (!ok) {
            //TODO: Log failure here
            throw new InvalidOperationException($"Received response with unknown message ID \"{response.MsgId}\"");
        }

        pendingRequest?.Complete(response);
    }

    private void HandleNotification(NvimNotification? notification) {
        if (notification is null) {
            return;
        }

        if (notification.Method == "redraw") {
            var uiEvents = notification.Params.SelectMany(
              uiEvent => {
                  System.Collections.Generic.IList<MessagePackObject> data = uiEvent.AsList();
                  string name = data.First().AsString();
                  return data.Select(args => new { Name = name, Args = args })
                                 .Skip(1);
              });
            foreach (var uiEvent in uiEvents) {
                CallUIEventHandler(uiEvent.Name, (object[])ConvertFromMessagePackObject(uiEvent.Args));
            }
        }

        bool has_handler = _handlers.TryGetValue(notification.Method, out NvimHandler? handler);
        object[] arguments = (object[])ConvertFromMessagePackObject(notification.Params);

        if (has_handler && handler is not null) {
            handler.Invoke(requestId: null, arguments);
        } else {
            NvimUnhandledNotificationEventArgs args = new(notification.Method, arguments);
            OnUnhandledNotification?.Invoke(this, args);
        }
    }

    private void HandleIncomingRequest(NvimRequest? request) {

        if (request is null) {
            return;
        }

        object? result = null;
        object? error = null;

        try {
            bool has_handler = _handlers.TryGetValue(request.Method, out NvimHandler? handler);
            if (has_handler) {
                object[] args = [.. request.Params.Select(ConvertFromMessagePackObject)];

                // handler should send response
                handler?.Invoke(request.MsgId, args);
                return;
            }

            if (OnUnhandledRequest is not null) {
                object[] args = [.. request.Params.Select(ConvertFromMessagePackObject)];
                OnUnhandledRequest.Invoke(this, new NvimUnhandledRequestEventArgs(this, request.MsgId, request.Method, args));
                return;
            }

            error = $"Unhandled request: {request.Method}";
        } catch (Exception ex) {
            error = ex.ToString();
        }

        // fallback response if no handler/exception
        SendResponse(request.MsgId, result, error);

        //TODO: Handle the request. Maybe I need PendingRequest with generics?

        //bool ok = _pendingOutgoingRequests.TryRemove(request.MsgId, out PendingRequest? pendingRequest);

        //if (!ok) {
        //    //TODO: Log failure here
        //    throw new InvalidOperationException($"Received response with unknown message ID \"{request.MsgId}\"");
        //}

        //pendingRequest?.Complete(request);
    }


    /// <summary>
    /// Waits for disconnect
    /// </summary>
    public void WaitForDisconnect() {
        _ = _waitEvent.WaitOne();
    }


    private object ConvertFromMessagePackObject(MessagePackObject msgPackObject) {
        if (msgPackObject.IsTypeOf<long>() ?? false) {
            return msgPackObject.AsInt64();
        }

        if (msgPackObject.IsTypeOf<double>() ?? false) {
            return msgPackObject.AsDouble();
        }

        if (msgPackObject.IsArray) {
            return msgPackObject.AsEnumerable().Select(ConvertFromMessagePackObject)
              .ToArray();
        }

        if (msgPackObject.IsDictionary) {
            MessagePackObjectDictionary msgPackDictionary = msgPackObject.AsDictionary();
            return msgPackDictionary.ToDictionary(
              keyValuePair => ConvertFromMessagePackObject(keyValuePair.Key),
              keyValuePair => ConvertFromMessagePackObject(keyValuePair.Value));
        }

        object obj = msgPackObject.ToObject();
        if (obj is MessagePackExtendedTypeObject msgpackExtObj) {
            return GetExtensionType(msgpackExtObj);
        }

        return obj;
    }


    /// <summary>
    /// Converts an array of boxed objects to and array of <see cref="MessagePackObject"/> objects
    /// </summary>
    private static MessagePackObject[] GetRequestArguments(params object[] parameters) {
        MessagePackObject[] r = new MessagePackObject[parameters.Length];
        for (int i = 0; i < parameters.Length; i++) {
            r[i] = parameters[i].ToMessagePackObject();
            // //Msgpack does not auto convert dictionaries.
            // //We will do the conversion ourselves.
            // if (parameters[i] is IDictionary dictionary) {
            //     r[i] = dictionary.ToMessagePackObject();
            // } else {
            //     r[i] = MessagePackObject.FromObject(parameters[i]);
            // }
        }
        return r;
    }


    /// <summary>
    /// Source generated regex for performance
    /// </summary>
    [GeneratedRegex(@"\\\\(?'serverName'[^\\]+)\\pipe\\(?'pipeName'[^\\]+)")]
    private static partial Regex MyRegex();

    public void Dispose() {
        cts.Cancel();
        transmitTask.Wait();
        receiveTask.Wait();
        cts.Dispose();
        _waitEvent.Dispose();
    }
}