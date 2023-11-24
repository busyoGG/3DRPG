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
    /// �ƶ��ٶ�
    /// </summary>
    public float _moveSpeed = 0.1f;
    /// <summary>
    /// ��ת�ٶ�
    /// </summary>
    public float _rotateLerpSpeed = 0.3f;
    /// <summary>
    /// ��Ծ���ٶ�
    /// </summary>
    public float _jumpSpeed = 1f;
    /// <summary>
    /// ����
    /// </summary>
    public float _gravity = 9.8f;
    /// <summary>
    /// ��Ծ���̱���
    /// </summary>
    public float _jumpScale = 1f;
    /// <summary>
    /// �ƶ�����
    /// </summary>
    private Vector3 _movePos = Vector3.zero;
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

    public CameraScript camera { get; set; }

    void Start()
    {
        _movePos = transform.position;

        player.Get<MoveComp>().speed = _moveSpeed;
        player.Get<MoveComp>().lastPosition = transform.position;
        player.Get<MoveComp>().nextPostition = transform.position;

        player.Get<RenderComp>().node = transform;

        player.Get<JumpComp>().speed = _jumpSpeed;
        player.Get<JumpComp>().baseY = transform.position.y;
        //player.Get<JumpComp>().curY = transform.position.y;
        //TransformSingleton.Ins().SetMoveY(player.id,transform.position.y);
        player.Get<JumpComp>().gravity = _gravity;
        player.Get<JumpComp>().scale = _jumpScale;
        ConsoleUtils.Log("��ҳ�ʼ��");
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
                player.Get<JumpComp>().isJump = true;
                player.Get<JumpComp>().startJump = true;
            }
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
        //_forward = _forward.normalized;
        Vector3 res = (camera.GetRotation(false) * _forward).normalized;
        //res.y = 0f;
        //player.Get<MoveComp>().forward.x = res.x;
        //player.Get<MoveComp>().forward.z = res.z;
        InputSingleton.Ins().SetForward(player.id, res.x, res.z);
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
        Vector3 res = (camera.GetRotation(false) * _forward).normalized;
        //res.y = 0f;
        //player.Get<MoveComp>().forward.x = res.x;
        //player.Get<MoveComp>().forward.z = res.z;
        InputSingleton.Ins().SetForward(player.id, res.x, res.z);
    }

    public void SetControlType(ControlType type)
    {
        _controlType = type;
    }
}
