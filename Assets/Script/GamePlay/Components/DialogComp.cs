using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CompRegister(typeof(DialogComp))]
public class DialogComp : Comp
{
    public List<int> randomIds;

    public List<int> missionIds;

    public string name;
    public override void Reset()
    {
        name = string.Empty;
        randomIds = new List<int>();
        missionIds = new List<int>();
    }
}
