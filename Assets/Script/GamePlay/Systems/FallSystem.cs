using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallSystem : ECSSystem
{
    public override ECSMatcher Filter()
    {
        return ECSManager.Ins().AllOf(typeof(MoveComp), typeof(CollideComp),typeof(TransformComp));
    }

    public override void OnUpdate(List<Entity> entities)
    {
        foreach (Entity entity in entities)
        {
            MoveComp move = entity.Get<MoveComp>();
            TransformComp transform = entity.Get<TransformComp>();

            float timeOfHigherest = move.jumpSpeed / move.gravity;

            if (move.isClimb || move.isSlope && (!move.isJump || move.isJump && move.fallTime > timeOfHigherest))
            {
                move.isJump = false;
                move.fallTime = 0f;
            }
            else
            {
                //ÏÂÂä
                move.fallTime += _dt * move.jumpScale;

                if(move.isJump )
                {
                    move.nextPostition.y += (move.jumpSpeed * move.jumpScale - move.gravity * move.jumpScale * move.fallTime) * _dt;
                }
                else
                {
                    move.nextPostition.y += -move.gravity * move.jumpScale * move.fallTime * _dt;
                }

                if (move.nextPostition.y < 1)
                {
                    move.isJump = false;
                    move.fallTime = 0f;
                    move.nextPostition.y = 1;
                }
                transform.position = move.nextPostition;
            }
        }
    }
}
