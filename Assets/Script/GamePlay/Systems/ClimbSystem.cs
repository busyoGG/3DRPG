
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

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
                if (!move.isCanCheckClimb) continue;

                Vector3 offsetNormal = collider.totalOffset.normalized;
                float sameSide = Vector3.Dot(offsetNormal, move.forward);

                if (offsetNormal.y < 0.5f && sameSide < -0.5f)
                {
                    climb.enterTime += _dt;
                    if (climb.enterTime >= climb.targetTime)
                    {
                        move.isClimb = true;
                        //climb.firstClimb = true;
                    }
                }
                else
                {
                    climb.enterTime = 0;
                }
            }
            else
            {
                Vector3 dir = (box.position - collider.closestCenter).normalized;

                if (dir.y < -0.7f)
                {
                    //头顶有障碍物
                    move.isClimb = false;
                }
                else
                {
                    Quaternion qua = Quaternion.identity;
                    qua.SetLookRotation(-dir);

                    move.fixedForward = qua * move.curForwad;
                    move.fixedForward.y = 0;

                    move.up.SetFromToRotation(Vector3.up, dir);

                    move.fixedForward = move.up * move.fixedForward;

                    bool isClimbTop = entity.Has<ClimbUpComp>();

                    if (box.maxY - collider.closestTop.y > 0.1f && !isClimbTop)
                    {
                        //攀爬到顶并且没有ClimbUpComp的情况下添加组件
                        entity.Add<ClimbUpComp>();
                        move.climbJump = 0;
                    }
                    else if (!isClimbTop)
                    {
                        //吸附
                        Vector3 offset = Vector3.zero;
                        switch (box.type)
                        {
                            case CollisionType.AABB:
                                offset = collider.closestCenter + dir * box.aabb.halfSize.z - move.nextPostition;
                                break;
                            case CollisionType.OBB:
                                offset = collider.closestCenter + dir * box.obb.halfSize.z - move.nextPostition;
                                break;
                        }
                        //跳跃
                        if (move.climbJump > 0)
                        {
                            if (move.climbJump == 3)
                            {
                                climb.fixedForward = climb.jumpForward;
                                if (climb.fixedForward == Vector3.zero)
                                {
                                    climb.fixedForward.z = 1;
                                }
                                climb.fixedForward = qua * climb.fixedForward;
                                climb.fixedForward = move.up * climb.fixedForward;
                            }
                            move.climbJump--;
                            move.nextPostition += climb.fixedForward * move.speed * 2;
                        }

                        move.nextPostition += offset;
                    }
                }


                if (move.nextPostition.y <= 1 && move.fixedForward.y < 0)
                {
                    move.isClimb = false;
                    move.climbJump = 0;
                }
            }


            //if (move.isClimbTop == 1 && !entity.Has<ClimbUpComp>())
            //{
            //    entity.Add<ClimbUpComp>();
            //}
        }
    }

    public override void OnDrawGizmos(List<Entity> entities)
    {
        foreach (Entity entity in entities)
        {
            BoxComp box = entity.Get<BoxComp>();
            CollideComp collider = entity.Get<CollideComp>();
            MoveComp move = entity.Get<MoveComp>();

            Vector3 dir = box.position - collider.closestCenter;

            Gizmos.color = Color.black;
            Gizmos.DrawLine(box.position, box.position + dir * 10);
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(box.position, box.position + move.fixedForward * 10);
            Gizmos.color = Color.white;
        }
    }

}
