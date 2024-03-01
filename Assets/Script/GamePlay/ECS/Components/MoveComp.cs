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
    /// 判定是否可以检测攀爬状态
    /// </summary>
    public bool isCanCheckClimb;
    /// <summary>
    /// 是否攀爬
    /// </summary>
    public bool isClimb;
    /// <summary>
    /// 是否在平台上
    /// </summary>
    public bool isOnPlane;
    /// <summary>
    /// 攀爬跳跃阶段
    /// </summary>
    public int climbJump;

    public Vector3 inputForward;

    public Vector3 forward;

    public Vector3 curForwad;

    public Vector3 fixedForward;
    /// <summary>
    /// 上方向
    /// </summary>
    public Quaternion up;

    //public Vector3 climbOffset;

    //public Quaternion climbOffsetQua;

    public override void Reset()
    {
        //isMove = false;
        //lastPosition = Vector3.zero;
        nextPostition = Vector3.zero;
        //climbOffset = Vector3.zero;
        //climbOffsetQua = Quaternion.identity;
        up = Quaternion.identity;
        inputForward = Vector3.zero;
        forward = Vector3.zero;
        curForwad = Vector3.zero;
        fixedForward = Vector3.zero;

        speed = 0f;
        fallTime = 0f;
        gravity = 9.8f;
        //isTop = false;
        //isSlope = false;
        isJump = false;
        isClimb = false;
        jumpScale = 0f;
        jumpSpeed = 0f;
        //isClimbTop = 0;
        isOnPlane = true;
        climbJump = 0;
        isCanCheckClimb = true;
    }
}
