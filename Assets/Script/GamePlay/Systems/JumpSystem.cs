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

            //float moveY = TransformSingleton.Ins().GetMoveY(entity.id);
            float moveY = TransformSingleton.Ins().GetMoveY(entity.id);
            Vector3 totalNormal = CollisionSingleton.Ins().GetTotalNormal(entity.id);
            float higherest = CollisionSingleton.Ins().GetHigherest(entity.id);
            float lower = TransformSingleton.Ins().lower;

            jump.curY = moveY + move.nextPostition.y;

            bool isCollide = false;
            List<CollideComp> colliders = CollisionSingleton.Ins().GetColliders(entity.id);

            CollideComp collider = null;

            if (jump.startJump)
            {
                jump.duration = 0;
                jump.baseY = move.nextPostition.y + moveY;
                jump.curY = jump.baseY;
                jump.startJump = false;
                jump.onLand = false;
            }
            else
            {
                bool isLower = jump.curY <= lower && moveY <= 0;
                isCollide = colliders != null && colliders.Count > 0;

                if (isCollide)
                {
                    collider = entity.Get<CollideComp>();
                }

                if (!jump.isJump && (isLower || isCollide && (totalNormal.y > 0 && totalNormal.y < 0.9 || totalNormal.y == 1)))
                {
                    jump.onLand = true;
                    jump.duration = 0;
                }
                else
                {
                    jump.onLand = false;
                }
            }

            if (!jump.onLand)
            {
                if (jump.duration == 0 && !jump.isJump)
                {
                    jump.baseY = move.nextPostition.y + moveY;
                    jump.curY = jump.baseY;
                    //ConsoleUtils.Log("下落初始化",move.nextPostition.y,moveY, jump.curY,isCollide);
                }
                float t = jump.speed / jump.gravity;
                ConsoleUtils.Log("自由落体/跳跃 ", isCollide, totalNormal.y);
                if (jump.duration >= t && isCollide && (totalNormal.y > 0 && totalNormal.y < 0.9 || totalNormal.y == 1))
                {
                    jump.onLand = true;
                    jump.duration = 0;
                    jump.isJump = false;
                    switch (collider.type)
                    {
                        case CollisionType.AABB:
                            jump.baseY = higherest + collider.aabb.halfSize.y;
                            break;
                        case CollisionType.OBB:
                            jump.baseY = higherest + collider.obb.halfSize.y;
                            break;
                    }
                    jump.curY = jump.baseY;
                    TransformSingleton.Ins().SetMoveY(entity.id, jump.baseY - move.nextPostition.y);
                    TransformSingleton.Ins().SetFreeFall(entity.id, false);
                    //ConsoleUtils.Log("落到障碍物",totalNormal);
                    //ConsoleUtils.Log("moveY-Collide", moveY, jump.baseY - move.nextPostition.y);
                }
                else
                {
                    jump.duration += _dt * jump.scale;
                    //float lastY = curY;
                    if (jump.isJump)
                    {
                        jump.curY = jump.baseY + jump.speed * jump.duration - 0.5f * jump.gravity * jump.duration * jump.duration;
                    }
                    else
                    {
                        jump.curY = jump.baseY - 0.5f * jump.gravity * jump.duration * jump.duration;
                        //ConsoleUtils.Log("掉落");
                    }

                    //ConsoleUtils.Log(jump.isJump, curY, jump.baseY,jump.duration);

                    if (jump.curY <= lower)
                    {
                        jump.onLand = true;
                        jump.isJump = false;
                        jump.baseY = lower;
                        jump.duration = 0;
                        jump.curY = jump.baseY;
                        TransformSingleton.Ins().SetMoveY(entity.id, jump.baseY - move.nextPostition.y);
                        //moveY = (lower - lastY) / move.speed;
                        //ConsoleUtils.Log(lower, lastY, move.forward.y);
                        //ConsoleUtils.Log("moveY-Fall", jump.baseY - move.nextPostition.y);
                    }
                    else
                    {
                        //moveY = (curY - lastY) / move.speed;
                        TransformSingleton.Ins().SetMoveY(entity.id, jump.curY - move.nextPostition.y);
                        //ConsoleUtils.Log("跳跃",jump.curY - move.nextPostition.y);
                        //ConsoleUtils.Log("moveY-Falling", jump.curY, jump.baseY, moveY, jump.curY - move.nextPostition.y);
                    }
                    //ConsoleUtils.Log("下落", moveY, curY, lastY);
                    TransformSingleton.Ins().SetFreeFall(entity.id, true);
                }
            }
            else
            {
                //moveY = 0;
                //jump.baseY = move.nextPostition.y + moveY;
                TransformSingleton.Ins().SetFreeFall(entity.id, false);
            }

            //TransformSingleton.Ins().SetMoveY(entity.id, moveY);
        }
    }
}
