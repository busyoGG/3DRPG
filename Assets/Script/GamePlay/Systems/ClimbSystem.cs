
using System.Collections.Generic;
using UnityEngine;

public class ClimbSystem : ECSSystem
{
    public override ECSMatcher Filter()
    {
        return ECSManager.Ins().AllOf(typeof(ClimbComp), typeof(CollideComp), typeof(MoveComp), typeof(BoxComp));
    }

    public override void OnUpdate(List<Entity> entities)
    {
        foreach (var entity in entities)
        {
            MoveComp move = entity.Get<MoveComp>();
            ClimbComp climb = entity.Get<ClimbComp>();
            CollideComp collider = entity.Get<CollideComp>();
            BoxComp box = entity.Get<BoxComp>();

            if (!move.isClimb)
            {
                //初始化
                //move.climbOffset = Vector3.zero;
                //move.climbOffsetQua = Quaternion.identity;

                //Vector3 inputForward = InputSingleton.Ins().GetForward(entity.id);

                float sameSide = Vector3.Dot(collider.totalOffset.normalized, move.forward);

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
                //if (climb.firstClimb)
                //{
                //    climb.firstClimb = false;

                //    Vector3 originForward = move.forward;

                //    Quaternion climbRot = Quaternion.identity;
                //    climbRot.SetLookRotation(-move.climbOffset);

                //    originForward = climbRot * originForward;

                //    move.nextPostition += move.climbOffsetQua * originForward * move.speed;
                //}
                //else
                //{
                //    //if (move.isSlope || move.isTop)
                //    //{
                //    //    move.isClimb = false;
                //    //}

                //}


                Vector3 dir = box.position - collider.closestCenter;

                Quaternion qua = Quaternion.identity;
                qua.SetLookRotation(-dir);

                move.fixedForward = qua * move.curForwad;

                move.up.SetFromToRotation(Vector3.up, dir);

                move.fixedForward = move.up * move.fixedForward;

                //攀爬到顶并且没有ClimbUpComp的情况下添加组件
                if (box.maxY - collider.closestTop.y > 0.1f && !entity.Has<ClimbUpComp>())
                {
                    entity.Add<ClimbUpComp>();
                }

            }

            if (move.nextPostition.y < 1)
            {
                move.isClimb = false;
            }

            //if (move.isClimbTop == 1 && !entity.Has<ClimbUpComp>())
            //{
            //    entity.Add<ClimbUpComp>();
            //}
        }
    }
}
