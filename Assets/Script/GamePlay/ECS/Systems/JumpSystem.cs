
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
                move.isJump = true;
                jump.startJump = false;
                move.fixedForward = move.forward;
            }
        }
    }
}
