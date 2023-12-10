using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CompRegister(typeof(AttackComp))]
public class AttackComp : Comp
{
    public int entityId;
    public override void Reset()
    {
        entityId = -1;
    }
}
