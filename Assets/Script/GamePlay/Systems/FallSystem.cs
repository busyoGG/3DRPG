using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallSystem : ECSSystem
{
    public override ECSMatcher Filter()
    {
        return ECSManager.Ins().AllOf(typeof(MoveComp), typeof(CollideComp));
    }

    public override void OnUpdate(List<Entity> entities)
    {
        foreach (Entity entity in entities)
        {
            MoveComp move = entity.Get<MoveComp>();

            float timeOfHigherest = move.jumpSpeed / move.gravity;

            if (move.isSlope && (!move.isJump || move.isJump && move.fallTime > timeOfHigherest))
            {
                move.isJump = false;
                move.isFall = false;
                move.fallTime = 0f;
            }
            else
            {
                //ÏÂÂä
                move.isFall = true;
                move.fallTime += _dt * move.jumpScale;

                //if (move.isJump)
                //{
                //    move.nextPostition.y += move.jumpSpeed * move.fallTime - 0.5f * move.gravity * move.fallTime * move.fallTime;
                //}
                //else
                //{
                //    move.nextPostition.y += -0.5f * move.gravity * move.fallTime * move.fallTime;
                //}

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
                    move.isFall = false;
                    move.fallTime = 0f;
                    move.nextPostition.y = 1;
                }
            }
        }
    }
}
