using System;
using System.Collections.Generic;

[CompRegister(typeof(TriggerComp))]
public class TriggerComp : Comp
{
    /// <summary>
    /// 是否主动查找
    /// </summary>
    public bool isPositive;

    public bool isEnable;

    public Dictionary<TriggerFunction, Action<Entity, Entity>> OnTriggerEnter = new Dictionary<TriggerFunction, Action<Entity, Entity>>();

    public Dictionary<TriggerFunction, Action<Entity, Entity>> OnTriggerExit = new Dictionary<TriggerFunction, Action<Entity, Entity>>();

    public Dictionary<TriggerFunction, Action<Entity, Entity>> OnTriggerKeeping = new Dictionary<TriggerFunction, Action<Entity, Entity>>();

    public List<TriggerFunction> triggerFunc = new List<TriggerFunction>();

    public Dictionary<int, Dictionary<TriggerFunction, TriggerStatus>> status = new Dictionary<int, Dictionary<TriggerFunction, TriggerStatus>>();
    public override void Reset()
    {
        OnTriggerEnter.Clear();
        OnTriggerExit.Clear();
        OnTriggerKeeping.Clear();
        isPositive = false;
        status.Clear();
        triggerFunc.Clear();
        isEnable = true;
    }
}
