using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// °ó¶¨UI×é¼þ
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
public class UICompBind :Attribute
{
    public string _path;
    public UICompBind(string path)
    {
        _path = path;
    }
}
