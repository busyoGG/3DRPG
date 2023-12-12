
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
            CollideComp collider = entity.Get<CollideComp>();
            BoxComp box = entity.Get<BoxComp>();

            if (collider.isStatic) continue;

            //collider.totalOffset = Vector3.zero;

            MoveComp move = entity.Get<MoveComp>();
            //初始化碰撞数组
            List<BoxComp> colliders = CollisionSingleton.Ins().GetColliders(entity.id);
            List<BoxComp> collidedComps = new List<BoxComp>();

            List<QTreeObj> objs = QTreeSingleton.Ins().GetQtreeObjs(entity.id);

            Vector3 point;

            foreach (var obj in objs)
            {
                if (obj.entity.id == entity.id) continue;
                BoxComp boxComp = obj.entity.Get<BoxComp>();
                if (boxComp != null)
                {
                    bool isCollided = CheckCollide(box, boxComp, out point);
                    if (isCollided)
                    {
                        if (!colliders.Contains(boxComp))
                        {
                            colliders.Insert(0, boxComp);
                        }
                        collidedComps.Add(boxComp);
                    }
                }
            }

            //证true的属性需要满足条件才初始化为true
            if (colliders.Count > 0 && move.isClimbTop != 2)
            {
                move.isClimbTop = 1;
                move.isTop = true;
            }
            move.isSlope = false;

            float minClose = float.MaxValue;
            //按顺序检测碰撞
            for (int i = colliders.Count - 1; i >= 0; i--)
            {
                BoxComp boxComp = colliders[i];
                if (!collidedComps.Contains(boxComp))
                {
                    colliders.RemoveAt(i);
                }
                else
                {
                    float len;
                    Vector3 normal = GetNormal(box, boxComp, out len);
                    if (boxComp.position.y >= 1 && normal.y > 0)
                    {
                        //计算坡道法线
                        move.isSlope = true;
                        //防止滑坡

                        Quaternion rotation = Quaternion.identity;
                        rotation.SetFromToRotation(Vector3.up, normal);

                        move.forwardOffset = normal;
                        move.forwardOffsetQua = rotation;

                        if (move.isClimbTop == 2)
                        {
                            collider.totalOffset += normal * len;
                        }
                        //刷新包围盒
                        RefreshCollider(box, normal * len);
                    }
                    else
                    {
                        if (move.isClimb && move.isClimbTop != 2)
                        {
                            //攀爬用偏移
                            Vector3 closest = GetClosestPoint(boxComp.position, boxComp);
                            //攀爬最高点
                            _tempVec = boxComp.position;
                            switch (boxComp.type)
                            {
                                case CollisionType.AABB:
                                    _tempVec.y = boxComp.aabb.max.y;
                                    break;
                                case CollisionType.OBB:
                                    _tempVec.y = boxComp.obb.maxY;
                                    break;
                            }
                            Vector3 higherest = GetClosestPoint(_tempVec, boxComp);
                            //判断是否爬到顶部 没有的话就计算攀爬法线
                            if (_tempVec.y - higherest.y < 0.1f)
                            {
                                move.isClimbTop = 0;
                                //计算攀爬法线
                                Vector3 dir = boxComp.position - closest;
                                if (dir.magnitude - move.speed <= minClose)
                                {
                                    minClose = dir.magnitude;

                                    Quaternion rotation = Quaternion.identity;
                                    rotation.SetFromToRotation(Vector3.up, dir);

                                    move.climbOffset = dir * 2;
                                    move.climbOffsetQua = rotation;

                                    switch (boxComp.type)
                                    {
                                        case CollisionType.AABB:
                                            collider.totalOffset = closest + dir.normalized * (boxComp.aabb.halfSize.z - move.speed) - move.nextPostition;
                                            break;
                                        case CollisionType.OBB:
                                            collider.totalOffset = closest + dir.normalized * (boxComp.obb.halfSize.z - move.speed) - move.nextPostition;
                                            break;
                                    }
                                }
                            }
                            else
                            {
                                ConsoleUtils.Log("到顶");
                            }
                        }
                        else
                        {

                            //计算偏移法线
                            if ((collider.totalOffset.y > 0 || move.nextPostition.y <= 1) && normal.y < 0)
                            {
                                normal.y = 0;
                            }
                            collider.totalOffset += normal * len;
                            //刷新包围盒
                            RefreshCollider(box, normal * len);
                        }

                    }
                    //判断是否站在顶部
                    if (boxComp.minY < boxComp.maxY)
                    {
                        move.isTop = false;
                    }
                }
            }

            move.forwardOffset = move.forwardOffset.normalized;

            if (!move.isClimb)
            {
                //没有攀爬 重置攀爬到顶状态
                move.isClimbTop = 0;
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

    public bool CheckCollide(BoxComp box1, BoxComp box2, out Vector3 point)
    {
        switch (box1.type)
        {
            case CollisionType.AABB:
                return SubCheckCollide(box1.aabb, box2, out point);
            case CollisionType.OBB:
                return SubCheckCollide(box1.obb, box2, out point);
            case CollisionType.Circle:
                return SubCheckCollide(box1.circle, box2, out point);
            case CollisionType.Ray:
                return SubCheckCollide(box1.ray, box2, out point);
        }
        point = CollideUtils._minValue;
        return false;
    }

    public bool SubCheckCollide(ICollide data1, BoxComp box2, out Vector3 point)
    {
        switch (box2.type)
        {
            case CollisionType.AABB:
                //return data1.Interactive(box2.aabb, out point);
                return data1.Interactive(box2.aabb, out point);
            case CollisionType.OBB:
                return data1.Interactive(box2.obb, out point);
            case CollisionType.Circle:
                return data1.Interactive(box2.circle, out point);
            case CollisionType.Ray:
                return data1.Interactive(box2.ray, out point);
        }
        point = CollideUtils._minValue;
        return false;
    }

    public Vector3 GetNormal(BoxComp box1, BoxComp box2, out float len)
    {
        switch (box1.type)
        {
            case CollisionType.Circle:
                break;
            case CollisionType.AABB:
                switch (box2.type)
                {
                    case CollisionType.Circle:
                    case CollisionType.AABB:
                        return CollideUtils.GetCollideNormal(box1.aabb, box2.aabb, out len);
                    case CollisionType.OBB:
                        return CollideUtils.GetCollideNormal(box1.aabb.obb, box2.obb, out len);
                    case CollisionType.Ray:
                        break;
                }
                break;
            case CollisionType.OBB:
                switch (box2.type)
                {
                    case CollisionType.Circle:
                    case CollisionType.AABB:
                        return CollideUtils.GetCollideNormal(box1.obb, box2.aabb.obb, out len);
                    case CollisionType.OBB:
                        return CollideUtils.GetCollideNormal(box1.obb, box2.obb, out len);
                    case CollisionType.Ray:
                        break;
                }
                break;
            case CollisionType.Ray:
                switch (box2.type)
                {
                    case CollisionType.Circle:
                    case CollisionType.AABB:
                    case CollisionType.OBB:
                    case CollisionType.Ray:
                        break;
                }
                break;
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
