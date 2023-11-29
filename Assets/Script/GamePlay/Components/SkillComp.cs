using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SkillType { 
    
}

[CompRegister(typeof(SkillComp))]
public class SkillComp : Comp
{
    public int id;

    public float duration;

    public float passTime;

    public override void Reset()
    {

    }
}
