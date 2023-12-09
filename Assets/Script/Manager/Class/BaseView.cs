using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FairyGUI;

public class BaseView
{
    public string id;
    public GComponent main;
    public string name;

    /// <summary>
    /// ֻ�ڴ�����ʱ��ִ��
    /// </summary>
    public void OnAwake() { 
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

    /// <summary>
    /// ÿ��չʾ��ʱ��ִ��
    /// </summary>
    protected virtual void OnShow() { }

    /// <summary>
    /// ÿ�����ص�ʱ��ִ��
    /// </summary>
    protected virtual void OnHide() { }

    protected virtual void InitAction() { }

    protected virtual void BindItem() { }

    protected virtual void InitListener() { }
}