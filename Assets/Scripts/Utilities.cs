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
    
    public static int GetChildCountActive(this Transform t)
    {
        return t.Cast<Transform>().Count(c => c.gameObject.activeSelf);
    }
    
    public static int GetTransformCountCondition(this Transform t, bool condition)
    {
        return t.Cast<Transform>().Count(c => condition);
    }
    
    public static int GetTransformCountPredicate(this Transform t, Func<Transform,bool> predicate)
    {
        return t.Cast<Transform>().Count(predicate);
    }
    
    public static bool IsOfAnyType<T>(T obj, Type[] types)
    {
        bool isOfAnyType = false;

        for (int i = 0; i < types.Length; i++)
        {
            if (types[i].IsAssignableFrom (obj.GetType()))
            {
                isOfAnyType = true;
                break;
            }
        }
        return isOfAnyType;
    }

    public static bool IsOfType<T>(T obj, Type type)
    {
        return type.IsInstanceOfType(obj);
    }
}
 