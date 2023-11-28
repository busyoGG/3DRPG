using Game;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;

public enum ControlType
{
    MouseAndKeyboard,
    KeyboardOnly,
    ControllerOnly
}

public class PlayerController : MonoBehaviour
{
    /// <summary>
    /// 移动速度
    /// </summary>
    public float _moveSpeed = 0.1f;
    /// <summary>
    /// 旋转速度
    /// </summary>
    public float _rotateLerpSpeed = 0.3f;
    /// <summary>
    /// 跳跃初速度
    /// </summary>
    public float _jumpSpeed = 1f;
    /// <summary>
    /// 重力
    /// </summary>
    public float _gravity = 9.8f;
    /// <summary>
    /// 跳跃过程倍率
    /// </summary>
    public float _jumpScale = 1f;
    /// <summary>
    /// 是否可以移动
    /// </summary>
    private bool _canMove = true;
    /// <summary>
    /// 是否可以跳跃
    /// </summary>
    private bool _canJump = true;
    /// <summary>
    /// 方向向量
    /// </summary>
    private Vector3 _forward = Vector3.zero;
    /// <summary>
    /// 控制类型
    /// </summary>
    private ControlType _controlType = ControlType.KeyboardOnly;

    public Entity player { get; set; }

    public CameraScript camera { get; set; }

    void Start()
    {

        player.Get<MoveComp>().speed = _moveSpeed;
        player.Get<MoveComp>().lastPosition = transform.position;
        player.Get<MoveComp>().nextPostition = transform.position;

        player.Get<RenderComp>().node = transform;

        player.Get<MoveComp>().jumpSpeed = _jumpSpeed;
        //player.Get<JumpComp>().curY = transform.position.y;
        //TransformSingleton.Ins().SetMoveY(player.id,transform.position.y);
        player.Get<MoveComp>().gravity = _gravity;
        player.Get<MoveComp>().jumpScale = _jumpScale;
        ConsoleUtils.Log("玩家初始化");
        InitInput();
    }

    void Update()
    {
        //Rotate();
        //Jump();
        if (_controlType == ControlType.MouseAndKeyboard)
        {
            //MoveTo();
        }
        //DoMove();

    }

    private void InitInput()
    {
        InputManager.Ins().AddKeyboardInputCallback(KeyCode.W, InputStatus.Hold, () =>
        {
            if (_canMove)
            {
                //_movePos.z += _moveSpeed;
                SetForward(KeyCode.W);
            }
        });

        InputManager.Ins().AddKeyboardInputCallback(KeyCode.S, InputStatus.Hold, () =>
        {
            if (_canMove)
            {
                //_movePos.z += -_moveSpeed;
                SetForward(KeyCode.S);
            }
        });

        InputManager.Ins().AddKeyboardInputCallback(KeyCode.A, InputStatus.Hold, () =>
        {
            if (_canMove)
            {
                //_movePos.x += -_moveSpeed;
                SetForward(KeyCode.A);
            }
        });

        InputManager.Ins().AddKeyboardInputCallback(KeyCode.D, InputStatus.Hold, () =>
        {
            if (_canMove)
            {
                //_movePos.x += _moveSpeed;
                SetForward(KeyCode.D);
            }
        });

        InputManager.Ins().AddKeyboardInputCallback(KeyCode.W, InputStatus.Up, () =>
        {
            if (_canMove)
            {
                //_movePos.z += _moveSpeed;
                ResetForward(KeyCode.W);
            }
        });

        InputManager.Ins().AddKeyboardInputCallback(KeyCode.S, InputStatus.Up, () =>
        {
            if (_canMove)
            {
                //_movePos.z += -_moveSpeed;
                ResetForward(KeyCode.S);
            }
        });

        InputManager.Ins().AddKeyboardInputCallback(KeyCode.A, InputStatus.Up, () =>
        {
            if (_canMove)
            {
                //_movePos.x += -_moveSpeed;
                ResetForward(KeyCode.A);
            }
        });

        InputManager.Ins().AddKeyboardInputCallback(KeyCode.D, InputStatus.Up, () =>
        {
            if (_canMove)
            {
                //_movePos.x += _moveSpeed;
                ResetForward(KeyCode.D);
            }
        });

        InputManager.Ins().AddKeyboardInputCallback(KeyCode.Space, InputStatus.Down, () =>
        {
            if (_canJump)
            {
                player.Get<JumpComp>().startJump = true;
            }
        });

        InputManager.Ins().AddKeyboardInputCallback(KeyCode.X, InputStatus.Down, () =>
        {
            player.Get<MoveComp>().isClimb = false;
        });
    }

    private void SetForward(KeyCode key)
    {
        switch (key)
        {
            case KeyCode.A:
                _forward.x = -1f;
                break;
            case KeyCode.D:
                _forward.x = 1f;
                break;
            case KeyCode.W:
                _forward.z = 1f;
                break;
            case KeyCode.S:
                _forward.z = -1f;
                break;
        }
        Vector3 res = _forward.normalized;
        InputSingleton.Ins().SetForward(player.id, res.x, res.z, camera.GetRotation(false));
    }

    private void ResetForward(KeyCode key)
    {
        switch (key)
        {
            case KeyCode.A:
            case KeyCode.D:
                _forward.x = 0f;
                break;
            case KeyCode.W:
            case KeyCode.S:
                _forward.z = 0f;
                break;
        }
        Vector3 res =  _forward.normalized;
        InputSingleton.Ins().SetForward(player.id, res.x, res.z, camera.GetRotation(false));
    }

    public void SetControlType(ControlType type)
    {
        _controlType = type;
    }
}
