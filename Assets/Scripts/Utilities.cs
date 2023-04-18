using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        return t.Cast<Transform>().Count(c => c.gameObject.activeSelf);
    }
}
 