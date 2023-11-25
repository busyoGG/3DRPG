using Game;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

public class JumpSystem : ECSSystem
{
    public override ECSMatcher Filter()
    {
        return ECSManager.Ins().AllOf(typeof(JumpComp), typeof(MoveComp));
    }

    public override void OnUpdate(List<Entity> entities)
    {
        foreach (Entity entity in entities)
        {
            JumpComp jump = entity.Get<JumpComp>();
            MoveComp move = entity.Get<MoveComp>();

            if (jump.startJump)
            {
                jump.isJump = true;
                jump.startJump = false;
            }

            if (jump.isJump)
            {
                if (move.nextPostition.y <= 1 && jump.duration > 0 || move.isSlope)
                {
                    jump.isJump = false;
                    jump.duration = 0;
                }
                else
                {
                    jump.duration += _dt * 0.5f;
                    move.nextPostition.y += jump.speed * jump.duration;
                }
            }
        }
    }
}
