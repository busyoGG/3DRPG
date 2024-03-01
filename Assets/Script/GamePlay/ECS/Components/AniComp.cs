using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CompRegister(typeof(AniComp))]
public class AniComp : Comp
{
    //public string curAni;

    public string lastAni;

    public float speed;

    public bool isChange;

    public Animator animator;

    public override void Reset()
    {
        //curAni = string.Empty; 
        lastAni = string.Empty; 
        speed = 0f;
        isChange = true;
        animator = null;
    }
}
