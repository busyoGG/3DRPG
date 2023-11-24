using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CompRegister(typeof(MoveComp))]
public class MoveComp : Comp
{

    //[RuntimeInitializeOnLoadMethod]
    //static void Init()
    //{
    //    id = ECSManager.Ins().GetCompId();
    //    ECSManager.Ins().CompRegister(typeof(TransformComp), id);
    //}

    public Vector3 lastPosition;

    public Vector3 nextPostition;

    //public Vector3 forward;

    public float speed;

    public bool isMove;

    public override void Reset()
    {
        isMove = false;
        lastPosition = Vector3.zero;
        nextPostition = Vector3.zero;
        //forward = Vector3.zero;
        speed = 0f;
    }
}
