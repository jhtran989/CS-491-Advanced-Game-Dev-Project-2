using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utilities : MonoBehaviour
{
    public static readonly string EmptyString = "";
    
    public static IReadOnlyList<T> GetValues<T>()
    {
        return (T[]) Enum.GetValues(typeof(T));
    }
}
 