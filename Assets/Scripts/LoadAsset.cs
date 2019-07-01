using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LoadAsset : Singleton<LoadAsset> {

    string assetPath = "AssetData/";
    string field = "assetName";

    public T LoadAssets<T>() where T : ScriptableObject
    {
        T instance = Activator.CreateInstance<T>();
        string name = (string)instance.GetType().GetField(field).GetValue(instance);
        string path = assetPath + name;
        return Resources.Load<T>(path);
    }
}
