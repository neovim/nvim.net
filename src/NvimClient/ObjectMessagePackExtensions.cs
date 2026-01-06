using MsgPack;
using System;
using System.Collections;
using System.Collections.Generic;

namespace NvimClient;

public static class ObjectMessagePackExtensions {
    public static MessagePackObject ToMessagePackObject(this object? obj) {


        if (obj is Array array) {
            return array.ToMessagePackObject();
        }

        if (obj is IDictionary dictionary) {
            return dictionary.ToMessagePackObject();
        }

        return MessagePackObject.FromObject(obj);
    }


    private static MessagePackObject[] ToMessagePackArray(this Array inputArray) {
        MessagePackObject[] list = new MessagePackObject[inputArray.Length];

        for (int i = 0; i < inputArray.Length; i++) {
            list[i] = inputArray.GetValue(i).ToMessagePackObject();
        }

        return list;
    }

    private static MessagePackObject ToMessagePackObject(this Array inputArray) {
        List<MessagePackObject> list = [];

        foreach (object obj in inputArray) {
            MessagePackObject a = obj.ToMessagePackObject();
            list.Add(a);
        }

        return MessagePackObject.FromObject(list);
    }

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


    private static List<MessagePackObject> ToMessagePackEnumerable(this IEnumerable<object> enumerable) {
        List<MessagePackObject> list = [];

        foreach (object obj in enumerable) {
            list.Add(obj.ToMessagePackObject());
        }

        return list;
    }
}