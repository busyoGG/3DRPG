
using System.Collections.Generic;
using UnityEngine;

public class ClimbSystem : ECSSystem
{
    public override ECSMatcher Filter()
    {
        return ECSManager.Ins().AllOf(typeof(ClimbComp), typeof(CollideComp), typeof(MoveComp));
    }

    public override void OnUpdate(List<Entity> entities)
    {
        foreach (var entity in entities)
        {
            MoveComp move = entity.Get<MoveComp>();
            ClimbComp climb = entity.Get<ClimbComp>();
            if (!move.isClimb)
            {
                CollideComp collider = entity.Get<CollideComp>();

                Vector3 inputForward = InputSingleton.Ins().GetForward(entity.id);

                float sameSide = Vector3.Dot(collider.totalOffset, inputForward);

                if (sameSide < 0)
                {
                    climb.enterTime += _dt;
                    if (climb.enterTime >= climb.targetTime)
                    {
                        move.isClimb = true;
                        climb.firstClimb = true;
                    }
                }
                else
                {
                    climb.enterTime = 0;
                }
            }
            else
            {
                //if (climb.firstClimb)
                //{
                //    climb.firstClimb = false;
                //    move.nextPostition -= move.forwardOffset * move.speed;
                //}
                //move.nextPostition += move.climbOffset * move.speed;
                //move.climbOffset = Vector3.zero;
            }

            if (move.nextPostition.y < 1)
            {
                move.isClimb = false;
            }

            //攀爬到顶并且没有ClimbUpComp的情况下添加组件
            if (move.isClimbTop == 1 && !entity.Has<ClimbUpComp>())
            {
                entity.Add<ClimbUpComp>();
            }
        }
    }
}
