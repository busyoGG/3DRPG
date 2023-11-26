using Game;
using Microsoft.SqlServer.Server;
using System.Collections.Generic;
using System.Diagnostics;
using System.Transactions;
using Unity.VisualScripting;
using UnityEngine;

public class CollideSystem : ECSSystem
{
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
            //¼ì²âÅö×²
            QTreeComp qTree = entity.Get<QTreeComp>();
            //³õÊ¼»¯Åö×²Êý×é
            List<CollideComp> colliders = CollisionSingleton.Ins().GetColliders(entity.id);
            List<CollideComp> collidedComps = new List<CollideComp>();

            AABBData bounds = qTree.qObj.bounds;

            List<QTreeObj> objs = QtreeManager.Ins().Find(MapManager.Ins().GetIndex(bounds.position), bounds);

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

            move.isTop = true;
            move.isSlope = false;
            //°´Ë³Ðò¼ì²âÅö×²
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
                        move.isSlope = true;
                        Vector3 inputForward = InputSingleton.Ins().GetForward(entity.id);
                        if (!inputForward.Equals(Vector3.zero))
                        {
                            move.forwardOffset = normal;
                        }
                    }
                    else
                    {
                        if ((collider.totalOffset.y > 0 || move.nextPostition.y <= 1) && normal.y < 0)
                        {
                            normal.y = 0;
                        }
                        collider.totalOffset += normal * len;
                    }

                    RefreshCollider(collider, normal * len);

                    if (collider.minY < collideComp.maxY)
                    {
                        move.isTop = false;
                    }
                }
            }
        }
    }

    public override void OnDrawGizmos(List<Entity> entities)
    {
        foreach (Entity entity in entities)
        {
            CollideComp collider = entity.Get<CollideComp>();
            if (collider.type == CollisionType.AABB)
            {
                Gizmos.DrawWireCube(collider.aabb.position, collider.aabb.size);
            }
            else
            {
                Gizmos.DrawLineList(collider.obb.vertexts);
            }
        }
    }

    private void RefreshCollider(CollideComp collider, Vector3 offset)
    {
        //¸üÐÂ°üÎ§ºÐ
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
}
