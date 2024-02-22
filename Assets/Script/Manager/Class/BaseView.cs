using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FairyGUI;
using System;
using System.Reflection;
using Unity.VisualScripting;
using System.Linq;
using System.Runtime.InteropServices;

public class BaseView : UIBase
{
    public string id;
    public GComponent main;
    public string name;

    /// <summary>
    /// 只在创建的时候执行
    /// </summary>
    public void OnAwake()
    {
        Bind();
    }

    public void Show()
    {
        main.visible = true;
        OnShow();
    }

    public void Hide()
    {
        main.visible = false;
        OnHide();
    }

    public bool GetVisible()
    {
        return main.visible;
    }

    /// <summary>
    /// 每次展示的时候执行
    /// </summary>
    protected virtual void OnShow() { }

    /// <summary>
    /// 每次隐藏的时候执行
    /// </summary>
    protected virtual void OnHide() { }
}
