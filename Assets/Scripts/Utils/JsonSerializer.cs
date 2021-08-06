using System.IO;
using FullSerializer;
using UnityEngine;

public static class JsonSerializer
{
    public static T Load<T>(string path) where T : class
    {
        if (!File.Exists(path))
        {
            return null;
        }

        var file = new StreamReader(path);

        var fileContents = file.ReadToEnd();
        var data = fsJsonParser.Parse(fileContents);
        object deserialized = null;
        var serializer = new fsSerializer();
        serializer.TryDeserialize(data, typeof(T), ref deserialized).AssertSuccessWithoutWarnings();

        file.Close();

        return deserialized as T;
    }

    public static void Save<T>(string path, T data) where T : class
    {
        fsData serializedData;
        var serializer = new fsSerializer();
        serializer.TrySerialize(data, out serializedData).AssertSuccessWithoutWarnings();

        var file = new StreamWriter(path);

        var json = fsJsonPrinter.PrettyJson(serializedData);
        file.WriteLine(json);

        file.Close();
    }

    //public static void Save<T>(string path, T obj)
    //{
    //    string jsonData = UnityEditor.EditorJsonUtility.ToJson(obj, true);

    //    File.WriteAllText(path, jsonData);
    //}

    //public static T Load<T>(string path)
    //{
    //    if (!File.Exists(path))
    //    {
    //        return default;
    //    }

    //    string jsonData = File.ReadAllText(path);
        
    //    return JsonUtility.FromJson<T>(jsonData);
    //}
}
