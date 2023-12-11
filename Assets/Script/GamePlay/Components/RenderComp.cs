using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CompRegister(typeof(RenderComp))]
public class RenderComp : Comp
{
    public Transform node;

    public float totalStep;

    public float curStep;
    public override void Reset()
    {
        node = null;
        totalStep = 0.05f;
        curStep = 0;
    }
}
