using System.Collections.Generic;
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

            LinkedList<(int, Entity)> intersectObjs = IntersectSingleton.Ins().GetIntersectObjs(entity.id);

            bool isCollide = false;
            move.up = Quaternion.identity;
            collider.closestTop = CollideUtils._maxValue;
            float minClose = float.MaxValue;
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

                    _tempVec = box.position;
                    _tempVec.y = box.maxY;
                    Vector3 higherest = GetClosestPoint(_tempVec, boxComp);
                    //获取最高点
                    collider.closestTop = higherest;

                    move.isCanCheckClimb = collideComp.isCanClimb;

                    if (move.isClimb && collideComp.isCanClimb)
                    {
                        //�ж��Ƿ��������� û�еĻ��ͼ�����������
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
                            //旋转前进方向
                            move.up.SetFromToRotation(Vector3.up, normal);
                            move.fixedForward = move.up * move.curForwad;
                        }
                    }

                    Vector3 offset = Vector3.zero;
                    if (box.minY < boxComp.maxY && box.minY + collider.stepHeight >= boxComp.maxY)
                    {
                        //̨�׵����
                        offset.y = boxComp.maxY - box.minY;
                    }
                    else
                    {
                        offset = normal * len;

                        //�Ƶ����
                        if (!collider.isStatic && !collideComp.isStatic)
                        {
                            if (collider.powerToMove > collideComp.powerToBeMove)
                            {
                                if (collider.powerToMove != 0)
                                {
                                    float powerRatio = collideComp.powerToBeMove / collider.powerToMove;
                                    Vector3 reverseOffset = offset * (1 - powerRatio);
                                    offset *= powerRatio;
                                    collideComp.totalOffset += -reverseOffset;
                                }
                            }
                        }
                    }
                    collider.totalOffset += offset;
                    collider.lastTotalOffset = collider.totalOffset;
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

    /// <summary>
    /// 刷新包围盒
    /// </summary>
    /// <param name="box"></param>
    /// <param name="offset"></param>
    private void RefreshCollider(BoxComp box, Vector3 offset)
    {
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
            case CollisionType.Capsule:
                if (box.capsule.position != box.position)
                {
                    box.capsule.position = box.position;
                }
                break;
        }
    }

    /// <summary>
    /// 获取碰撞法向量
    /// </summary>
    /// <param name="box1"></param>
    /// <param name="box2"></param>
    /// <param name="len"></param>
    /// <returns></returns>
    public Vector3 GetNormal(BoxComp box1, BoxComp box2, out float len)
    {
        switch (box1.type)
        {
            case CollisionType.AABB:
                return GetNormal(box1.aabb, box2, out len);
            case CollisionType.OBB:
                return GetNormal(box1.obb, box2, out len);
            case CollisionType.Capsule:
                return GetNormal(box1.capsule, box2, out len);
        }
        len = 0;
        return Vector3.zero;
    }

    /// <summary>
    /// 获取碰撞法向量的具体实现
    /// </summary>
    /// <param name="data1"></param>
    /// <param name="box2"></param>
    /// <param name="len"></param>
    /// <returns></returns>
    private Vector3 GetNormal(ICollide data1, BoxComp box2, out float len)
    {
        switch (box2.type)
        {
            case CollisionType.AABB:
                return data1.GetNormal(box2.aabb, out len);
            case CollisionType.OBB:
                return data1.GetNormal(box2.obb, out len);
            case CollisionType.Capsule:
                return data1.GetNormal(box2.capsule,out len);
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
            // case 
        }
        return Vector3.zero;
    }
}
