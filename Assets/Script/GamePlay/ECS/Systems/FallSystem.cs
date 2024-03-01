using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

public class FallSystem : ECSSystem
{
    public override ECSMatcher Filter()
    {
        return ECSManager.Ins().AllOf(typeof(MoveComp), typeof(CollideComp), typeof(TransformComp));
    }

    public override void OnUpdate(List<Entity> entities)
    {
        foreach (Entity entity in entities)
        {
            MoveComp move = entity.Get<MoveComp>();
            //float timeOfHigherest = move.jumpSpeed / move.gravity;
            //float over = Mathf.Abs(Vector3.Dot(Vector3.down, move.fixedForward));

            if (move.isClimb || move.isOnPlane && (!move.isJump || move.isJump && move.fallTime > 0))
            {
                move.isJump = false;
                move.fallTime = 0f;
            }
            else
            {
                //ÏÂÂä
                move.fallTime += _dt * move.jumpScale;

                if (move.isJump)
                {
                    move.nextPostition.y += (move.jumpSpeed * move.jumpScale - move.gravity * move.jumpScale * move.fallTime) * _dt;
                }
                else
                {
                    move.nextPostition.y += -move.gravity * move.jumpScale * move.fallTime * _dt;
                }
                //transform.position = move.nextPostition;
            }

            if (move.nextPostition.y < Global.MinGround)
            {
                move.isJump = false;
                move.fallTime = 0f;
                move.nextPostition.y = Global.MinGround;
            }
        }
    }
}
