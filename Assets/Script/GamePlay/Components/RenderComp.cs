using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CompRegister(typeof(RenderComp))]
public class RenderComp : Comp
{
    public Transform node;
    public override void Reset()
    {
        node = null;
    }
}
