using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CompRegister(typeof(DialogComp))]
public class DialogComp : Comp
{
    public List<int> randomIds;

    public float maxDelta;

    public float minDelta;

    public string name;
    public override void Reset()
    {
        name = string.Empty;
        randomIds = new List<int>();
        maxDelta = 0;
        minDelta = 0;
    }
}
