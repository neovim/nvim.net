using MsgPack;
using System;
using System.Collections;
using System.Collections.Generic;

namespace NvimClient;

/// <summary>
///     A class the provides extensions to the <see cref="object"/> class for dealing
///     with <see cref="MessagePackObject"/>.
/// </summary>
///
/// <remarks>
///     Neovim uses lua with provides dynamic objects. Thus there is a lot of boxing
///     involved.
/// </remarks>
public static class ObjectMessagePackExtensions {

    /// <summary>
    /// Converts an <see cref="object"/> to a <see cref="MessagePackObject"/>
    /// </summary>
    public static MessagePackObject ToMessagePackObject(this object? obj) {
        //If Array or Dictionary use specific extensions for conversion
        if (obj is Array array) {
            return array.ToMessagePackObject();
        }

        if (obj is IDictionary dictionary) {
            return dictionary.ToMessagePackObject();
        }

        return MessagePackObject.FromObject(obj);
    }

    /// <summary>
    /// Converts an <see cref="Array"/> of objects to an array of <see cref="MessagePackObject"/>
    /// </summary>
    private static MessagePackObject[] ToMessagePackArray(this Array inputArray) {
        MessagePackObject[] list = new MessagePackObject[inputArray.Length];

        for (int i = 0; i < inputArray.Length; i++) {
            list[i] = inputArray.GetValue(i).ToMessagePackObject();
        }

        return list;
    }

    /// <summary>
    /// Converts an <see cref="Array"/> of objects to a single <see cref="MessagePackObject"/>
    /// </summary>
    private static MessagePackObject ToMessagePackObject(this Array inputArray) {
        List<MessagePackObject> list = [];

        foreach (object obj in inputArray) {
            MessagePackObject a = obj.ToMessagePackObject();
            list.Add(a);
        }

        return MessagePackObject.FromObject(list);
    }


    /// <summary>
    /// Converts an <see cref="IDictionary"/> of objects to a single <see cref="MessagePackObject"/>
    /// </summary>
    private static MessagePackObject ToMessagePackObject(this IDictionary dictionary) {

        MessagePackObjectDictionary map = [];

        //foreach (KeyValuePair<string, string> kvp in dictionary) {
        foreach (DictionaryEntry kvp in dictionary) {
            map.Add(
                    kvp.Key.ToMessagePackObject(),
                    kvp.Value.ToMessagePackObject()
            // MessagePackObject.FromObject(kvp.Key),
            // MessagePackObject.FromObject(kvp.Value)
            );
        }

        MessagePackObject mpo = new(map);
        return mpo;
    }


    /// <summary>
    /// Converts an <see cref="IEnumerable"/> of objects to a <see cref="List{MessagePackObject}"/>
    /// </summary>
    private static List<MessagePackObject> ToMessagePackEnumerable(this IEnumerable<object> enumerable) {
        List<MessagePackObject> list = [];

        foreach (object obj in enumerable) {
            list.Add(obj.ToMessagePackObject());
        }

        return list;
    }
}