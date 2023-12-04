using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CompRegister(typeof(MoveComp))]
public class MoveComp : Comp
{
    //public Vector3 lastPosition;

    public Vector3 nextPostition;

    public float fallTime;

    public float speed;

    public bool isJump;

    public float jumpSpeed;

    public float jumpScale;

    public float gravity;
    /// <summary>
    /// 是否移动
    /// </summary>
    public bool isMove;
    /// <summary>
    /// 是否在顶部
    /// </summary>
    public bool isTop;
    /// <summary>
    /// 是否坡
    /// </summary>
    public bool isSlope;
    /// <summary>
    /// 是否攀爬
    /// </summary>
    public bool isClimb;
    /// <summary>
    /// 是否爬到顶 0未到顶 1到顶 2自动攀爬
    /// </summary>
    public int isClimbTop;

    public Vector3 forwardOffset;

    public Quaternion forwardOffsetQua;

    public Vector3 climbOffset;

    public Quaternion climbOffsetQua;

    public override void Reset()
    {
        isMove = false;
        //lastPosition = Vector3.zero;
        nextPostition = Vector3.zero;
        forwardOffset = Vector3.zero;
        forwardOffsetQua = Quaternion.identity;
        climbOffset = Vector3.zero;
        climbOffsetQua = Quaternion.identity;
        //forward = Vector3.zero;
        speed = 0f;
        fallTime = 0f;
        gravity = 9.8f;
        isTop = false;
        isSlope = false;
        isJump = false;
        isClimb = false;
        jumpScale = 0f;
        jumpSpeed = 0f;
        isClimbTop = 0;
    }
}
