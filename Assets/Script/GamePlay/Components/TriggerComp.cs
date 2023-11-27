using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TriggerStatus
{
    Idle,
    Enter,
    Keeping,
    Exit
}

[CompRegister(typeof(TriggerComp))]
public class TriggerComp : Comp
{
    /// <summary>
    /// 是否主动查找
    /// </summary>
    public bool isPositive;

    public Action<Entity,Entity> OnTriggerEnter { get; set; }

    public Action<Entity, Entity> OnTriggerExit { get; set; }

    public Action<Entity, Entity> OnTriggerKeeping { get; set; }

    public Dictionary<int, TriggerStatus> status = new Dictionary<int, TriggerStatus>();
    public override void Reset()
    {
        OnTriggerEnter = null;
        OnTriggerExit = null;
        OnTriggerKeeping = null;
        isPositive = false;
        status.Clear();
    }
}
