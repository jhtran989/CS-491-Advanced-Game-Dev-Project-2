using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utilities
{
    public static readonly string EmptyString = "";
    
    public static IReadOnlyList<T> GetValues<T>()
    {
        return (T[]) Enum.GetValues(typeof(T));
    }
    
    public static int GetChildCountActive( this Transform t )
    {
        int k = 0;
        foreach(Transform c in t)
        {
            if(c.gameObject.activeSelf)
                k++;
        }
        return k;
    }
}
 