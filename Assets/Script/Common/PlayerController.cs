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
    /// �Ƿ�����ƶ�
    /// </summary>
    private bool _canMove = true;
    /// <summary>
    /// �Ƿ������Ծ
    /// </summary>
    private bool _canJump = true;
    /// <summary>
    /// ��������
    /// </summary>
    private Vector3 _forward = Vector3.zero;
    /// <summary>
    /// ��������
    /// </summary>
    private ControlType _controlType = ControlType.KeyboardOnly;

    public Entity player { get; set; }

    MoveComp _move;

    JumpComp _jump;

    public CameraScript cam { get; set; }

    void Start()
    {
        _move = player.Get<MoveComp>();
        _jump = player.Get<JumpComp>();
        InitInput();
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
                _jump.startJump = true;
            }
        });

        InputManager.Ins().AddKeyboardInputCallback(KeyCode.X, InputStatus.Down, () =>
        {
            _move.isClimb = false;
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
        InputSingleton.Ins().SetForward(player.id, res.x, res.z, cam.GetRotation(false));
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
        InputSingleton.Ins().SetForward(player.id, res.x, res.z, cam.GetRotation(false));
    }

    public void SetControlType(ControlType type)
    {
        _controlType = type;
    }
}
