using Game;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class ClassFactory
{
    /// <summary>
    /// 反射创建类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="classType"></param>
    /// <returns></returns>
    public static T CreateClass<T>(Type classType) where T : class
    {
        T instance = System.Activator.CreateInstance(classType) as T;
        return instance;
    }

    /// <summary>
    /// 反射获取类属性值
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="Value"></typeparam>
    /// <param name="classType"></param>
    /// <param name="key"></param>
    /// <param name="instance"></param>
    /// <returns></returns>
    public static Value GetPropValue<T,Value>(Type classType, T instance, string key) where T : class
    {
        return (Value)classType.GetProperty(key).GetValue(instance);
    }

    /// <summary>
    /// 反射设置类属性值
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="Value"></typeparam>
    /// <param name="classType"></param>
    /// <param name="instance"></param>
    /// <param name="key"></param>
    /// <param name="value"></param>
    public static void SetPropValue<T,Value>(Type classType, T instance, string key,Value value)
    {
        classType.GetProperty(key).SetValue(instance, value);
    }

    /// <summary>
    /// 反射设置类属性值
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="Value"></typeparam>
    /// <param name="classType"></param>
    /// <param name="instance"></param>
    /// <param name="key"></param>
    /// <param name="value"></param>
    public static void SetStaticPropValue<Value>(Type classType, string key, Value value)
    {
        classType.GetProperty(key, BindingFlags.Static | BindingFlags.Public).SetValue(null, value);
    }
}
