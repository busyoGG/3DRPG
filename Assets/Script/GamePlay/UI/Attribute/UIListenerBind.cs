using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// °ó¶¨UI¼àÌýÊÂ¼þ
/// </summary>
[AttributeUsage(AttributeTargets.Method)]
public class UIListenerBind : Attribute
{
    public string _name;

    public UIListenerBind(string name)
    {
        _name = name;
    }
}
