using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CompRegister(typeof(MoveComp))]
public class MoveComp : Comp
{
    public Vector3 lastPosition;

    public Vector3 nextPostition;

    public float fallTime;

    public float speed;

    public bool isJump;

    public float jumpSpeed;

    public float jumpScale;

    public float gravity;
    /// <summary>
    /// �Ƿ��ƶ�
    /// </summary>
    public bool isMove;
    /// <summary>
    /// �Ƿ��ڶ���
    /// </summary>
    public bool isTop;
    /// <summary>
    /// �Ƿ���
    /// </summary>
    public bool isSlope;
    /// <summary>
    /// �Ƿ�����
    /// </summary>
    public bool isClimb;
    /// <summary>
    /// �Ƿ������� 0δ���� 1���� 2�Զ�����
    /// </summary>
    public int isClimbTop;

    public Vector3 forwardOffset;

    public Quaternion forwardOffsetQua;

    public Vector3 climbOffset;

    public Quaternion climbOffsetQua;

    public override void Reset()
    {
        isMove = false;
        lastPosition = Vector3.zero;
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
