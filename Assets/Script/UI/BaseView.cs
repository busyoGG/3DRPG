using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FairyGUI;

public class BaseView
{
    public GComponent main;
    public string name;
    /// <summary>
    /// 只在创建的时候执行
    /// </summary>
    public virtual void OnAwake() { 
        InitAction();
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
    public virtual void OnShow() { }

    /// <summary>
    /// 每次隐藏的时候执行
    /// </summary>
    public virtual void OnHide() { }

    public virtual void InitAction() { }
}
