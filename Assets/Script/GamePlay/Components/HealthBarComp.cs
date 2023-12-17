using FairyGUI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CompRegister(typeof(HealthBarComp))]
public class HealthBarComp : Comp
{
    public GComponent ui;
    public override void Reset()
    {
        ui = null;
    }
}
