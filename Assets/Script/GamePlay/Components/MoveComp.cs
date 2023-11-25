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

    public float gravity;

    public float fallTime;

    public float speed;

    public bool isMove;

    public bool isTop;

    public bool isSlope;

    public Vector3 forwardOffset;

    public override void Reset()
    {
        isMove = false;
        lastPosition = Vector3.zero;
        nextPostition = Vector3.zero;
        forwardOffset = Vector3.zero;
        //forward = Vector3.zero;
        speed = 0f;
        fallTime = 0f;
        gravity = 9.8f;
        isTop = false;
        isSlope = false;
    }
}
