using Game;
using Microsoft.SqlServer.Server;
using System.Collections.Generic;
using System.Diagnostics;
using System.Transactions;
using Unity.VisualScripting;
using UnityEngine;

public class CollideSystem : ECSSystem
{
    private Vector3 _tempVec = Vector3.zero;
    public override ECSMatcher Filter()
    {
        return ECSManager.Ins().AllOf(typeof(CollideComp), typeof(QTreeComp), typeof(MoveComp));
    }

    public override void OnUpdate(List<Entity> entities)
    {
        foreach (var entity in entities)
        {
            CollideComp collider = entity.Get<CollideComp>();

            if (collider.isStatic) continue;

            MoveComp move = entity.Get<MoveComp>();
            //初始化碰撞数组
            List<CollideComp> colliders = CollisionSingleton.Ins().GetColliders(entity.id);
            List<CollideComp> collidedComps = new List<CollideComp>();

            List<QTreeObj> objs = QTreeSingleton.Ins().GetQtreeObjs(entity.id);

            Vector3 point;

            foreach (var obj in objs)
            {
                if (obj.entity.id == entity.id) continue;
                CollideComp collideComp = obj.entity.Get<CollideComp>();
                if (collideComp != null)
                {
                    bool isCollided = CheckCollide(collider, collideComp, out point);
                    if (isCollided)
                    {
                        if (!colliders.Contains(collideComp))
                        {
                            colliders.Insert(0, collideComp);
                        }
                        collidedComps.Add(collideComp);
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
                CollideComp collideComp = colliders[i];
                if (!collidedComps.Contains(collideComp))
                {
                    colliders.RemoveAt(i);
                }
                else
                {
                    float len;
                    Vector3 normal = GetNormal(collider, collideComp, out len);
                    if (collider.position.y >= 1 && normal.y > 0)
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
                        RefreshCollider(collider, normal * len);
                    }
                    else
                    {
                        if (move.isClimb && move.isClimbTop != 2)
                        {
                            //攀爬用偏移
                            Vector3 closest = GetClosestPoint(collider.position, collideComp);
                            //攀爬最高点
                            _tempVec = collider.position;
                            switch (collider.type)
                            {
                                case CollisionType.AABB:
                                    _tempVec.y = collider.aabb.max.y;
                                    break;
                                case CollisionType.OBB:
                                    _tempVec.y = collider.obb.maxY;
                                    break;
                            }
                            Vector3 higherest = GetClosestPoint(_tempVec, collideComp);
                            //判断是否爬到顶部 没有的话就计算攀爬法线
                            if (_tempVec.y - higherest.y < 0.1f)
                            {
                                move.isClimbTop = 0;
                                //计算攀爬法线
                                Vector3 dir = collider.position - closest;
                                if (dir.magnitude - move.speed <= minClose)
                                {
                                    minClose = dir.magnitude;

                                    Quaternion rotation = Quaternion.identity;
                                    rotation.SetFromToRotation(Vector3.up, dir);

                                    move.climbOffset = dir * 2;
                                    move.climbOffsetQua = rotation;

                                    switch (collider.type)
                                    {
                                        case CollisionType.AABB:
                                            collider.totalOffset = closest + dir.normalized * (collider.aabb.halfSize.z - move.speed) - move.nextPostition;
                                            break;
                                        case CollisionType.OBB:
                                            collider.totalOffset = closest + dir.normalized * (collider.obb.halfSize.z - move.speed) - move.nextPostition;
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
                            RefreshCollider(collider, normal * len);
                        }

                    }
                    //判断是否站在顶部
                    if (collider.minY < collideComp.maxY)
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

    private void RefreshCollider(CollideComp collider, Vector3 offset)
    {
        //更新包围盒
        collider.position += offset;
        switch (collider.type)
        {
            case CollisionType.AABB:
                if (collider.aabb.position != collider.position)
                {
                    collider.aabb.position = collider.position;
                }
                break;
            case CollisionType.OBB:
                if (collider.obb.position != collider.position)
                {
                    collider.obb.position = collider.position;
                }
                //if(collider.obb.axes)
                break;
        }
    }

    public bool CheckCollide(CollideComp collide1, CollideComp collide2, out Vector3 point)
    {
        switch (collide1.type)
        {
            case CollisionType.Circle:
                switch (collide2.type)
                {
                    case CollisionType.Circle:
                        return Collision.CheckCollide(collide1.circle, collide2.circle, out point);
                    case CollisionType.AABB:
                        return Collision.CheckCollide(collide1.circle, collide2.aabb, out point);
                    case CollisionType.OBB:
                        return Collision.CheckCollide(collide1.circle, collide2.obb, out point);
                    case CollisionType.Ray:
                        return Collision.CheckCollide(collide2.ray, collide1.circle, out point);
                }
                break;
            case CollisionType.AABB:
                switch (collide2.type)
                {
                    case CollisionType.Circle:
                        return Collision.CheckCollide(collide2.circle, collide1.aabb, out point);
                    case CollisionType.AABB:
                        return Collision.CheckCollide(collide1.aabb, collide2.aabb, out point);
                    case CollisionType.OBB:
                        return Collision.CheckCollide(collide1.aabb.obb, collide2.obb, out point);
                    case CollisionType.Ray:
                        return Collision.CheckCollide(collide2.ray, collide1.aabb, out point);
                }
                break;
            case CollisionType.OBB:
                switch (collide2.type)
                {
                    case CollisionType.Circle:
                        return Collision.CheckCollide(collide2.circle, collide1.obb, out point);
                    case CollisionType.AABB:
                        return Collision.CheckCollide(collide1.obb, collide2.aabb.obb, out point);
                    case CollisionType.OBB:
                        return Collision.CheckCollide(collide1.obb, collide2.obb, out point);
                    case CollisionType.Ray:
                        return Collision.CheckCollide(collide2.ray, collide1.obb, out point);
                }
                break;
            case CollisionType.Ray:
                switch (collide2.type)
                {
                    case CollisionType.Circle:
                        return Collision.CheckCollide(collide1.ray, collide2.circle, out point);
                    case CollisionType.AABB:
                        return Collision.CheckCollide(collide1.ray, collide2.aabb, out point);
                    case CollisionType.OBB:
                        return Collision.CheckCollide(collide1.ray, collide2.obb, out point);
                        //case CollisionType.Ray:
                        //    return Collision.CheckCollide(collide1.ray, collide1.ray, out point);
                }
                break;
        }
        point = CollideUtils._minValue;
        return false;
    }

    public Vector3 GetNormal(CollideComp collide1, CollideComp collide2, out float len)
    {
        switch (collide1.type)
        {
            case CollisionType.Circle:
                break;
            case CollisionType.AABB:
                switch (collide2.type)
                {
                    case CollisionType.Circle:
                    case CollisionType.AABB:
                        return CollideUtils.GetCollideNormal(collide1.aabb, collide2.aabb, out len);
                    case CollisionType.OBB:
                        return CollideUtils.GetCollideNormal(collide1.aabb.obb, collide2.obb, out len);
                    case CollisionType.Ray:
                        break;
                }
                break;
            case CollisionType.OBB:
                switch (collide2.type)
                {
                    case CollisionType.Circle:
                    case CollisionType.AABB:
                        return CollideUtils.GetCollideNormal(collide1.obb, collide2.aabb.obb, out len);
                    case CollisionType.OBB:
                        return CollideUtils.GetCollideNormal(collide1.obb, collide2.obb, out len);
                    case CollisionType.Ray:
                        break;
                }
                break;
            case CollisionType.Ray:
                switch (collide2.type)
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
    private Vector3 GetClosestPoint(Vector3 position, CollideComp data2)
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
