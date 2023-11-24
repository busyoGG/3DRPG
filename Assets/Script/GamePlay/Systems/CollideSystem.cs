using Game;
using System.Collections.Generic;
using System.Transactions;
using Unity.VisualScripting;
using UnityEngine;

public class CollideSystem : ECSSystem
{
    public override ECSMatcher Filter()
    {
        return ECSManager.Ins().AllOf(typeof(CollideComp), typeof(QTreeComp));
    }

    public override void OnUpdate(List<Entity> entities)
    {
        foreach (var entity in entities)
        {
            CollideComp collider = entity.Get<CollideComp>();

            if (collider.isStatic) continue;

            MoveComp move = entity.Get<MoveComp>();
            Vector3 calculatedPosition = TransformSingleton.Ins().GetCalculatedPosition(entity.id);

            //更新包围盒
            switch (collider.type)
            {
                case CollisionType.AABB:
                    if (collider.aabb.position != calculatedPosition)
                    {
                        collider.aabb.position = calculatedPosition;
                    }
                    break;
                case CollisionType.OBB:
                    if (collider.obb.position != calculatedPosition)
                    {
                        collider.obb.position = calculatedPosition;
                    }
                    //if(collider.obb.axes)
                    break;
            }

            //检测碰撞
            QTreeComp qTree = entity.Get<QTreeComp>();
            //初始化碰撞数组
            List<CollideComp> colliders = CollisionSingleton.Ins().GetColliders(entity.id);
            colliders.Clear();
            List<Vector3> normals = CollisionSingleton.Ins().GetNormalList(entity.id);
            normals.Clear();
            List<float> higherests = CollisionSingleton.Ins().GetHigherestList(entity.id);
            higherests.Clear();
            List<Vector3> closestPoints = CollisionSingleton.Ins().GetClosestPointList(entity.id);
            closestPoints.Clear();
            List<Vector3> closestPointSelfs = CollisionSingleton.Ins().GetClosestPointSelfList(entity.id);
            closestPointSelfs.Clear();
            List<Vector3> insidePoints = CollisionSingleton.Ins().GetInsidePointList(entity.id);
            insidePoints.Clear();

            AABBData bounds = qTree.qObj.bounds;

            List<QTreeObj> objs = QtreeManager.Ins().Find(MapManager.Ins().GetIndex(bounds.position), bounds);

            Vector3 point;
            Vector3 totalNormal = Vector3.zero;
            float totalHigherest = float.MinValue;

            Vector3 position = Vector3.zero;
            float maxY = 0;
            CollideComp higherestCollide = null;

            switch (collider.type)
            {
                case CollisionType.AABB:
                    position = collider.aabb.position;
                    break;
                case CollisionType.OBB:
                    position = collider.obb.position;
                    break;
            }

            foreach (var obj in objs)
            {
                if (obj.entity.id == entity.id) continue;
                CollideComp collideComp = obj.entity.Get<CollideComp>();
                if (collideComp != null)
                {
                    bool isCollided = CheckCollide(collider, collideComp, out point);
                    if (isCollided)
                    {
                        colliders.Add(collideComp);
                        Vector3 closest = Vector3.zero;
                        Vector3 closestSelf = Vector3.zero;

                        if (collideComp.type == CollisionType.AABB)
                        {
                            closest = CollideUtils.GetClosestPointAABB(position, collideComp.aabb);
                            switch (collider.type)
                            {
                                case CollisionType.AABB:
                                    closestSelf = CollideUtils.GetClosestPointAABB(closest, collider.aabb);
                                    break;
                                case CollisionType.OBB:
                                    closestSelf = CollideUtils.GetClosestPointOBB(collideComp.aabb.position, collider.obb);
                                    foreach (Vector3 vertext in collideComp.aabb.vertexts)
                                    {
                                        if (CollideUtils.CheckInsideObb(vertext - collider.obb.position, collider.obb))
                                        {
                                            insidePoints.Add(vertext);
                                            break;
                                        }
                                    }
                                    break;
                            }
                            //AABB情况
                            Vector3 normal = CollideUtils.GetCollideNormal(position, collideComp.aabb);
                            normals.Add(normal);
                            totalNormal += normal;
                            maxY = normal.y > 0 ? closestSelf.y : float.MinValue;
                        }
                        else if (collideComp.type == CollisionType.OBB)
                        {
                            //maxY = collideComp.obb.max.y;
                            closest = CollideUtils.GetClosestPointOBB(position, collideComp.obb);
                            switch (collider.type)
                            {
                                case CollisionType.AABB:
                                    closestSelf = CollideUtils.GetClosestPointAABB(closest, collider.aabb);
                                    break;
                                case CollisionType.OBB:
                                    closestSelf = CollideUtils.GetClosestPointOBB(collideComp.obb.position, collider.obb);
                                    foreach (Vector3 vertext in collideComp.obb.vertexts)
                                    {
                                        if (CollideUtils.CheckInsideObb(vertext, collider.obb))
                                        {
                                            insidePoints.Add(vertext);
                                            break;
                                        }
                                    }
                                    break;
                            }
                            //OBB情况
                            Vector3 normal = CollideUtils.GetCollideNormal(closestSelf, collideComp.obb);
                            
                            normals.Add(normal);
                            totalNormal += normal;
                            maxY = normal.y > 0 ? closestSelf.y : float.MinValue;
                        }

                        closestPoints.Add(closest);
                        closestPointSelfs.Add(closestSelf);
                        higherests.Add(maxY);
                        //totalHigherest = Mathf.Max(totalHigherest, maxY);
                        if (totalHigherest < maxY)
                        {
                            totalHigherest = maxY;
                            higherestCollide = collideComp;
                        }
                    }
                }
            }
            //ConsoleUtils.Log("最高", totalHigherest);

            totalNormal.x = Mathf.Clamp(totalNormal.x, -1, 1);
            totalNormal.y = Mathf.Clamp(totalNormal.y, -1, 1);
            totalNormal.z = Mathf.Clamp(totalNormal.z, -1, 1);
            CollisionSingleton.Ins().SetTotalNormal(entity.id, totalNormal);

            CollisionSingleton.Ins().SetHigherest(entity.id, totalHigherest);
            CollisionSingleton.Ins().SetHigherestCollide(entity.id, higherestCollide);
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
}
