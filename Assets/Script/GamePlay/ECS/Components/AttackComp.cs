using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CompRegister(typeof(AttackComp))]
public class AttackComp : Comp
{
    public int group;
    public int entityId;
    public override void Reset()
    {
        entityId = -1;
        group = -1;
    }
}
