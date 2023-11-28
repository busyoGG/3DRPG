
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
            if (!move.isClimb)
            {
                ClimbComp climb = entity.Get<ClimbComp>();
                CollideComp collider = entity.Get<CollideComp>();

                Vector3 inputForward = InputSingleton.Ins().GetForward(entity.id);

                float sameSide = Vector3.Dot(collider.totalOffset, inputForward);

                if (sameSide < 0)
                {
                    climb.enterTime += _dt;
                    if (climb.enterTime >= climb.targetTime)
                    {
                        move.isClimb = true;
                    }
                }
                else
                {
                    climb.enterTime = 0;
                }
            }

            if (move.nextPostition.y < 1)
            {
                move.isClimb = false;
            }

            //������������û��ClimbUpComp�������������
            if (move.isClimbTop == 1 && !entity.Has<ClimbUpComp>())
            {
                entity.Add<ClimbUpComp>();
            }
        }
    }
}
