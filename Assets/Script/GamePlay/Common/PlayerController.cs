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
    /// 控制类型
    /// </summary>
    private ControlType _controlType = ControlType.KeyboardOnly;

    public Entity player { get; set; }

    MoveComp _move;

    JumpComp _jump;

    SkillComp _skill;

    public CameraScript cam { get; set; }

    void Start()
    {
        _move = player.Get<MoveComp>();
        _jump = player.Get<JumpComp>();
        _skill = player.Get<SkillComp>();
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
                }
                else
                {
                    ResetForward(key);
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
                EventManager.TriggerEvent("show_mission", new ArrayList() { MissionManager.Ins().GetNotBranchMission() });
                break;
            case InputKey.Space:
                if (_canJump)
                {
                    _jump.startJump = true;
                }
                return;
            case InputKey.X:
                _move.isClimb = false;
                return;
            default:
                return;
        }

        if (_canMove)
        {
            Vector3 res = _forward.normalized;
            InputSingleton.Ins().SetForward(player.id, res.x, res.z, cam.GetRotation(false));
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
        InputSingleton.Ins().SetForward(player.id, res.x, res.z, cam.GetRotation(false));
    }

    public void SetControlType(ControlType type)
    {
        _controlType = type;
    }
}
