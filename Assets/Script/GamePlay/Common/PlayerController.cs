using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
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
    /// 攀爬跳跃方向
    /// </summary>
    private Vector3 _climbJumpForward = Vector3.zero;
    /// <summary>
    /// 控制类型
    /// </summary>
    private ControlType _controlType = ControlType.KeyboardOnly;

    public Entity player { get; set; }

    MoveComp _move;

    JumpComp _jump;

    SkillComp _skill;

    ClimbComp _climb;

    void Start()
    {
        _move = player.Get<MoveComp>();
        _jump = player.Get<JumpComp>();
        _skill = player.Get<SkillComp>();
        _climb = player.Get<ClimbComp>();
        InitInput();
    }

    private void InitInput()
    {
        //Dictionary<InputKey, InputStatus> curKey = InputManager.Ins().GetKey();

        InputManager.Ins().AddEventListener(() =>
        {
            bool isFocus = UIManager.Ins().IsFocus();
            if (isFocus) return;

            Dictionary<InputKey, InputStatus> curKey = InputManager.Ins().GetKey();
            bool isAlt = curKey.ContainsKey(InputKey.LeftAlt) || curKey.ContainsKey(InputKey.RightAlt);
            foreach (var data in curKey)
            {
                InputKey key = data.Key;
                InputStatus status = data.Value;

                //基本操作监听
                if (status == InputStatus.Hold || status == InputStatus.Down)
                {
                    SetForward(key);
                    if (status == InputStatus.Hold)
                    {
                        SetClimbJumpForward(key);
                    }
                }
                else
                {
                    ResetForward(key);
                    ResetClimbJumpForward(key);
                }

                //技能监听
                if (key == InputKey.A || key == InputKey.D || key == InputKey.W || key == InputKey.S || key == InputKey.Space ||
                isAlt || key == InputKey.X) { }
                else
                {
                    if (key == InputKey.MouseLeft || key == InputKey.MouseRight)
                    {
                        _skill.key = key;
                        _skill.status = status;
                    }
                }
            }
        });
    }

    private void SetForward(InputKey key)
    {
        switch (key)
        {
            case InputKey.A:
                _forward.x = -1f;
                break;
            case InputKey.D:
                _forward.x = 1f;
                break;
            case InputKey.W:
                _forward.z = 1f;
                break;
            case InputKey.S:
                _forward.z = -1f;
                break;
            case InputKey.J:
                UIManager.Ins().Show<MissionView>("MissionView");
                EventManager.TriggerEvent("show_mission_list", new ArrayList() { MissionManager.Ins().GetNotBranchMission() });
                break;
            case InputKey.Space:
                if (_canJump)
                {
                    _jump.startJump = true;
                }
                _move.climbJump = 3;
                return;
            case InputKey.X:
                _move.isClimb = false;
                _move.fixedForward = _move.forward;
                return;
            default:
                return;
        }

        if (_canMove)
        {
            Vector3 res = _forward.normalized;
            //InputSingleton.Ins().SetForward(player.id, res.x, res.z, cam.GetRotation(false));
            _move.forward = Global.Cam.GetRotation(false) * res;
            _move.forward.y = 0;
            _move.inputForward = res;
        }
        else
        {
            ResetForward(key);
        }
    }

    private void ResetForward(InputKey key)
    {
        switch (key)
        {
            case InputKey.A:
            case InputKey.D:
                _forward.x = 0f;
                break;
            case InputKey.W:
            case InputKey.S:
                _forward.z = 0f;
                break;
            default:
                return;
        }
        Vector3 res = _forward.normalized;
        //InputSingleton.Ins().SetForward(player.id, res.x, res.z, cam.GetRotation(false));
        _move.forward = Global.Cam.GetRotation(false) * res;
        _move.forward.y = 0;
        _move.inputForward = res;
    }

    private void SetClimbJumpForward(InputKey key)
    {
        switch (key)
        {
            case InputKey.A:
                _climbJumpForward.x = -1f;
                break;
            case InputKey.D:
                _climbJumpForward.x = 1f;
                break;
            case InputKey.W:
                _climbJumpForward.z = 1f;
                break;
            case InputKey.S:
                _climbJumpForward.z = -1f;
                break;
            default:
                return;
        }
        Vector3 res = _climbJumpForward.normalized;
        _climb.jumpForward = res;
    }

    private void ResetClimbJumpForward(InputKey key)
    {
        switch (key)
        {
            case InputKey.A:
            case InputKey.D:
                _climbJumpForward.x = 0f;
                break;
            case InputKey.W:
            case InputKey.S:
                _climbJumpForward.z = 0f;
                break;
            default:
                return;
        }
        Vector3 res = _climbJumpForward.normalized;
        _climb.jumpForward = res;
    }

    public void SetControlType(ControlType type)
    {
        _controlType = type;
    }
}
