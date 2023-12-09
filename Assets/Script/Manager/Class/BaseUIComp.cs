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

    public virtual void OnAwake()
    {

    }

    public virtual void Reset()
    {

    }
}
