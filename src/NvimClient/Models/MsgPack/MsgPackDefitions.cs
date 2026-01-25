namespace NvimClient.Models.MsgPack;

/// <summary>
/// MsgPack constants definitions based on
/// https://github.com/msgpack-rpc/msgpack-rpc/blob/master/spec.md
/// </summary>
public static class MsgPackDefinitions {
    /// <summary>
    /// MsgPack rpc requests have type 0
    /// </summary>
    public const int RequestTypeId = 0;

    /// <summary>
    /// MsgPack rpc responses have type 1
    /// </summary>
    public const int ResponseTypeId = 1;

    /// <summary>
    /// MsgPack rpc notifications have type 2
    /// </summary>
    public const int NotificationTypeId = 2;
}

public enum NvimMessageType {
    Request = 0,
    Response = 1,
    Notification = 2
}