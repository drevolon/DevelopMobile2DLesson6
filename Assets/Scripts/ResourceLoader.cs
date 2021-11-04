using System;
using UnityEngine;
using Object = UnityEngine.Object;

public static class ResourceLoader
{
    public static GameObject LoadPrefab(ResourcePath path)
    {
        return Resources.Load<GameObject>(path.PathResource);
    }

    public static T LoadObject<T>(ResourcePath path) where T : Object
    {
        return Resources.Load<T>(path.PathResource);
    }
} 
