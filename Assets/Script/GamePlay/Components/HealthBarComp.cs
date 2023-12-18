using FairyGUI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CompRegister(typeof(HealthBarComp))]
public class HealthBarComp : Comp
{
    public HealthBar ui;
    public override void Reset()
    {
        ui = null;
    }
}
