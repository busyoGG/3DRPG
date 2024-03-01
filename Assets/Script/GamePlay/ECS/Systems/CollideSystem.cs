
using Microsoft.SqlServer.Server;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Transactions;
using Unity.VisualScripting;
using UnityEngine;

public class CollideSystem : ECSSystem
{
    private Vector3 _tempVec = Vector3.zero;
    public override ECSMatcher Filter()
    {
        return ECSManager.Ins().AllOf(typeof(CollideComp), typeof(QTreeComp), typeof(MoveComp), typeof(BoxComp));
    }

    public override void OnUpdate(List<Entity> entities)
    {
        foreach (var entity in entities)
        {
            BoxComp box = entity.Get<BoxComp>();
            if (!box.isPositive) continue;

            CollideComp collider = entity.Get<CollideComp>();

            MoveComp move = entity.Get<MoveComp>();
            //初始化碰撞数组

            LinkedList<(int, Entity)> intersectObjs = IntersectSingleton.Ins().GetIntersectObjs(entity.id);

            bool isCollide = false;
            //重置斜面旋转
            move.up = Quaternion.identity;
            //重置最高点
            collider.closestTop = CollideUtils._maxValue;
            //初始化最近点距离
            float minClose = float.MaxValue;
            //重置攀爬检测状态
            move.isCanCheckClimb = false;

            foreach (var intersectObj in intersectObjs)
            {
                Entity other = intersectObj.Item2;

                CollideComp collideComp = other.Get<CollideComp>();
                if (collideComp != null)
                {
                    isCollide = true;
                    BoxComp boxComp = other.Get<BoxComp>();

                    float len;
                    Vector3 normal = GetNormal(box, boxComp, out len);

                    //自己最高点
                    _tempVec = box.position;
                    _tempVec.y = box.maxY;
                    Vector3 higherest = GetClosestPoint(_tempVec, boxComp);
                    //障碍物最高点
                    collider.closestTop = higherest;

                    move.isCanCheckClimb = collideComp.isCanClimb;

                    if (move.isClimb && collideComp.isCanClimb)
                    {
                        //判断是否爬到顶部 没有的话就计算攀爬法线
                        if (_tempVec.y - higherest.y < 0.1f)
                        {
                            Vector3 closest = GetClosestPoint(box.position, boxComp);
                            Vector3 dir = box.position - closest;
                            if (dir.magnitude - move.speed <= minClose)
                            {
                                minClose = dir.magnitude;

                                collider.closestCenter = closest;
                            }
                        }
                    }
                    else
                    {
                        if (normal.y > 0)
                        {
                            //设置斜面旋转
                            move.up.SetFromToRotation(Vector3.up, normal);
                            move.fixedForward = move.up * move.curForwad;
                        }
                    }

                    Vector3 offset = Vector3.zero;
                    if (box.minY < boxComp.maxY && box.minY + collider.stepHeight >= boxComp.maxY)
                    {
                        //台阶的情况
                        offset.y = boxComp.maxY - box.minY;
                    }
                    else
                    {
                        offset = normal * len;

                        //推的情况
                        if (!collider.isStatic && !collideComp.isStatic)
                        {
                            if (collider.powerToMove > collideComp.powerToBeMove)
                            {
                                if (collider.powerToMove != 0)
                                {
                                    float powerRatio = collideComp.powerToBeMove / collider.powerToMove;
                                    Vector3 reverseOffset = offset * (1 - powerRatio);
                                    offset *= powerRatio;
                                    //被推的物体增加排斥值
                                    collideComp.totalOffset += -reverseOffset;
                                }
                            }
                        }
                    }
                    collider.totalOffset += offset;
                    RefreshCollider(box, offset);
                }
            }

            float over = Mathf.Abs(Vector3.Dot(Vector3.down, move.fixedForward));
            if (isCollide && (over > 0 && over < collider.slopeAngle || box.minY >= collider.closestTop.y))
            {
                move.isOnPlane = true;
            }
            else
            {
                move.isOnPlane = false;
            }
        }
    }

    private void RefreshCollider(BoxComp box, Vector3 offset)
    {
        //更新包围盒
        box.position += offset;
        switch (box.type)
        {
            case CollisionType.AABB:
                if (box.aabb.position != box.position)
                {
                    box.aabb.position = box.position;
                }
                break;
            case CollisionType.OBB:
                if (box.obb.position != box.position)
                {
                    box.obb.position = box.position;
                }
                //if(collider.obb.axes)
                break;
        }
    }

    public Vector3 GetNormal(BoxComp box1, BoxComp box2, out float len)
    {
        switch (box1.type)
        {
            case CollisionType.AABB:
                return GetNormal(box1.aabb, box2, out len);
            case CollisionType.OBB:
                return GetNormal(box1.obb, box2, out len);
        }
        len = 0;
        return Vector3.zero;
    }

    private Vector3 GetNormal(ICollide data1, BoxComp box2, out float len)
    {
        switch (box2.type)
        {
            case CollisionType.AABB:
                return data1.GetNormal(box2.aabb, out len);
            case CollisionType.OBB:
                return data1.GetNormal(box2.obb, out len);
        }
        len = 0;
        return Vector3.zero;
    }

    /// <summary>
    /// 获取最近点
    /// </summary>
    /// <param name="position"></param>
    /// <param name="data2"></param>
    /// <returns></returns>
    private Vector3 GetClosestPoint(Vector3 position, BoxComp data2)
    {
        switch (data2.type)
        {
            case CollisionType.AABB:
                return CollideUtils.GetClosestPoint(position, data2.aabb);
            case CollisionType.OBB:
                return CollideUtils.GetClosestPoint(position, data2.obb);
        }
        return Vector3.zero;
    }
}
