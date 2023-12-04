
using System;
using UnityEngine;

[Serializable]
public class TriggerBase
{
    public Action<Entity, Entity> OnTriggerEnter;

    public Action<Entity, Entity> OnTriggerExit;

    public Action<Entity, Entity> OnTriggerKeeping;
}
