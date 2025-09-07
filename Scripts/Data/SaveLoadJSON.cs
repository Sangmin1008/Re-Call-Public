using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System;
using Unity.VisualScripting;
public static class SaveLoadJSON
{
    static readonly string DataPath = "Assets/Data/";
    private static string FINDSAVEPath(string className)
    {
        return Path.Combine(Application.persistentDataPath,className + "Data.json");
    }
    public static List<T> LoadDataList<T>()
    {
        string className = typeof(T).Name;
        string path = DataPath+$"{className}Data.json";
        if (!File.Exists(path))
        {
            Debug.Log($"{className}파일이 없습니다.");
            return null;
        }
        string json = File.ReadAllText(path);
        Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(json);
        if (wrapper == null)
        {
            Debug.Log($"{className} 리스트 변환 실패.");
            return null;
        }
        Debug.Log("파일 로드됨: " + path+wrapper.list.Count);
        return wrapper.list;
    }
    public static List<IDataAble> LoadData<T>(string className)
    {
        string path = DataPath + $"{className}.json";
        if (!File.Exists(path))
        {
            Debug.Log($"{className}파일이 없습니다.");
            return null;
        }
        string json = File.ReadAllText(path);
        Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(json);
        if (wrapper == null)
        {
            Debug.Log($"{className} 리스트 변환 실패.");
            return null;
        }
        Debug.Log("파일 로드됨: " + path);
        string objcetClassName = className.Replace("Data", "");

        Type typeName = Type.GetType(objcetClassName);
        if (typeName == null)
        {
            Debug.LogWarning($"{typeName} 생성실패.");
            return null;
        }
        List<IDataAble> objList=new List<IDataAble>();
        foreach (T a in wrapper.list)
        {
            object obj = Activator.CreateInstance(typeName);
            //if(obj is IDataAble dataAble)
            //{
            //    dataAble.SetData(a);
            //}
        }
        return objList;
    }
    public static void SaveJSON<T>(string className,List<T> needToSave)
    {
        string path = FINDSAVEPath(className);
        var wrapper = new Wrapper<T>(needToSave);
        string json = JsonUtility.ToJson(wrapper);
       
        File.WriteAllText(path, json);
        Debug.Log("파일 저장됨: " + path);

    }
    public static List<T> LoadJSON<T>(string className)
    {
        string path = FINDSAVEPath(className);
        if (!File.Exists(path)) {
            Debug.Log($"{className}파일이 없습니다.");
            return null;
        }
        string json = File.ReadAllText(path);
        Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(json);
        if (wrapper==null)
        {
            Debug.Log($"{className} 리스트 변환 실패.");
            return null;
        }
        Debug.Log("파일 로드됨: " + path);
        return wrapper.list;
    }
    [System.Serializable]
    private class Wrapper<T>
    {
        public List<T> list;

        public Wrapper(List<T> list)
        {
            this.list = list;
        }
    }
}
