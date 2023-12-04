using System;
using System.Collections.Generic;

[CompRegister(typeof(TriggerComp))]
public class TriggerComp : Comp
{
    /// <summary>
    /// 是否主动查找
    /// </summary>
    public bool isPositive;

    public Action<Entity, Entity> OnTriggerEnter;

    public Action<Entity, Entity> OnTriggerExit;

    public Action<Entity, Entity> OnTriggerKeeping;

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
