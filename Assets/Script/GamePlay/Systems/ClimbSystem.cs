
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
                //��ʼ��
                move.climbOffset = Vector3.zero;
                move.climbOffsetQua = Quaternion.identity;

                CollideComp collider = entity.Get<CollideComp>();

                Vector3 inputForward = InputSingleton.Ins().GetForward(entity.id);

                float sameSide = Vector3.Dot(collider.totalOffset.normalized, inputForward);

                if (sameSide < -0.9f)
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
                if (climb.firstClimb)
                {
                    climb.firstClimb = false;

                    Vector3 originForward = InputSingleton.Ins().GetOriginForward(entity.id);

                    Quaternion climbRot = Quaternion.identity;
                    climbRot.SetLookRotation(-move.climbOffset);

                    originForward = climbRot * originForward;

                    move.nextPostition += move.climbOffsetQua * originForward * move.speed;
                }
                else
                {
                    if (move.isSlope || move.isTop)
                    {
                        move.isClimb = false;
                    }
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
