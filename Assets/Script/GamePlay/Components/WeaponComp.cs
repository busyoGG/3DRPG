using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CompRegister(typeof(WeaponComp))]
public class WeaponComp : Comp
{
    public int entityId;
    public override void Reset()
    {
        entityId = 0;
    }
}
