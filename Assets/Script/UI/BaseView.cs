using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FairyGUI;

public class BaseView
{
    public GComponent main;
    public string name;
    /// <summary>
    /// ֻ�ڴ�����ʱ��ִ��
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
    /// ÿ��չʾ��ʱ��ִ��
    /// </summary>
    public virtual void OnShow() { }

    /// <summary>
    /// ÿ�����ص�ʱ��ִ��
    /// </summary>
    public virtual void OnHide() { }

    public virtual void InitAction() { }
}
