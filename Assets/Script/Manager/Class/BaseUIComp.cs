using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FairyGUI;

public class BaseUIComp
{
    public string id;

    public GComponent main;

    public UIPanel panel;

    public Transform target;

    public string name { get; set; }

    /// <summary>
    /// 只在创建的时候执行
    /// </summary>
    public void OnAwake()
    {
        BindItem();
        InitListener();
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

    public virtual void Reset()
    {

    }

    /// <summary>
    /// 每次展示的时候执行
    /// </summary>
    protected virtual void OnShow() { }

    /// <summary>
    /// 每次隐藏的时候执行
    /// </summary>
    protected virtual void OnHide() { }

    protected virtual void InitAction() { }

    protected virtual void BindItem() { }

    protected virtual void InitListener() { }
}
