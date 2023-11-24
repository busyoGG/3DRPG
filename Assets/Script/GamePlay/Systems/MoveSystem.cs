using Game;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Xml.Schema;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class MoveSystem : ECSSystem
{
    public override ECSMatcher Filter()
    {
        return ECSManager.Ins().AnyOf(typeof(MoveComp));
    }

    public override void OnUpdate(List<Entity> entities)
    {
        foreach (Entity entity in entities)
        {
            MoveComp move = entity.Get<MoveComp>();
            Vector3 inputForward = InputSingleton.Ins().GetForward(entity.id);
            float moveY = TransformSingleton.Ins().GetMoveY(entity.id);

            if (!inputForward.Equals(Vector3.zero))
            {
                List<CollideComp> colliders = CollisionSingleton.Ins().GetColliders(entity.id);
                List<Vector3> closestPoint = CollisionSingleton.Ins().GetClosestPointList(entity.id);
                CollideComp higherestCollide = CollisionSingleton.Ins().GetHigherestCollide(entity.id);
                Vector3 totalNormal = CollisionSingleton.Ins().GetTotalNormal(entity.id);
                List<Vector3> normals = CollisionSingleton.Ins().GetNormalList(entity.id);
                List<Vector3> closestSelf = CollisionSingleton.Ins().GetClosestPointSelfList(entity.id);
                List<Vector3> insidePoints = CollisionSingleton.Ins().GetInsidePointList(entity.id);

                float lower = TransformSingleton.Ins().lower;

                bool isCollide = colliders != null && colliders.Count > 0;

                move.lastPosition = move.nextPostition;

                float maxY = float.MinValue;
                if (higherestCollide != null)
                {
                    switch (higherestCollide.type)
                    {
                        case CollisionType.AABB:
                            maxY = higherestCollide.aabb.max.y;
                            break;
                        case CollisionType.OBB:
                            maxY = higherestCollide.obb.maxY;
                            break;
                    }
                }

                Vector3 offset = Vector3.zero;
                if (isCollide)
                {
                    CollideComp collider = entity.Get<CollideComp>();
                    Vector3 maxVec = Vector3.zero;
                    Vector3 minVec = Vector3.zero;

                    for (int i = 0; i < normals.Count; i++)
                    {
                        Vector3 normal = normals[i];
                        Vector3 point = closestPoint[i];
                        switch (collider.type)
                        {
                            case CollisionType.AABB:
                                maxVec = collider.aabb.max;
                                minVec = collider.aabb.min;
                                if (normal.x > 0)
                                {
                                    float offsetX = point.x - minVec.x;
                                    offset.x = Mathf.Max(offset.x, offsetX);
                                }
                                else if (normal.x < 0)
                                {
                                    float offsetX = point.x - maxVec.x;
                                    offset.x = Mathf.Min(offset.x, offsetX);
                                }

                                if (normal.z > 0)
                                {
                                    float offsetZ = point.z - minVec.z;
                                    offset.z = Mathf.Max(offset.z, offsetZ);
                                }
                                else if (normal.z < 0)
                                {
                                    float offsetZ = point.z - maxVec.z;
                                    offset.z = Mathf.Min(offset.z, offsetZ);
                                }
                                break;
                            case CollisionType.OBB:

                                if (insidePoints.Count > 0)
                                {
                                    Vector3 dir = insidePoints[0] - move.nextPostition;
                                    float dotX = Vector3.Dot(collider.obb.axes[0], dir);
                                    float dotZ = Vector4.Dot(collider.obb.axes[2], dir);

                                    Vector3 forward = Vector3.zero;
                                    if (Mathf.Abs(dotX) > Mathf.Abs(dotZ))
                                    {
                                        if (dotX > 0)
                                        {
                                            forward = (collider.obb.halfSize.x - dotX) * collider.obb.axes[0];
                                        }
                                        else
                                        {
                                            forward = (-collider.obb.halfSize.x - dotX) * collider.obb.axes[0];
                                        }
                                    }
                                    else
                                    {
                                        if (dotZ > 0)
                                        {
                                            forward = (collider.obb.halfSize.z - dotZ) * collider.obb.axes[2];
                                        }
                                        else
                                        {
                                            forward = (-collider.obb.halfSize.z - dotZ) * collider.obb.axes[2];
                                        }
                                    }
                                    //offset += Vector3.Scale(forward, normal);?
                                    if (normal.x < 0)
                                    {
                                        offset.x += forward.x * normal.x;
                                    }
                                    else
                                    {
                                        offset.x += -forward.x * normal.x;
                                    }
                                    if(normal.z < 0)
                                    {
                                        offset.z += forward.z * normal.z;
                                    }
                                    else
                                    {
                                        offset.z += -forward.z * normal.z;
                                    }
                                    //ConsoleUtils.Log("offset - ", offset, collider.obb.halfSize.z,dotZ, forward.z,normal);
                                }

                                //offset += closestSelf[i] - point;
                                break;
                        }
                    }
                }

                bool isTop = higherestCollide != null && move.lastPosition.y - 0.5f + moveY >= maxY - 0.05;

                if (!isCollide || totalNormal.y == 1 && totalNormal.x == 0 && totalNormal.z == 0 || isTop)
                {
                    move.nextPostition = move.lastPosition + inputForward * move.speed;
                    //float higherest = CollisionSingleton.Ins().GetHigherest(entity.id);
                    //if (higherest + 0.5f > move.nextPostition.y)
                    //{
                    //    move.nextPostition.y = higherest + 0.5f;
                    //}
                    if (isTop)
                    {
                        //move.nextPostition.y = maxY;
                        TransformSingleton.Ins().SetMoveY(entity.id, maxY + 0.5f - move.nextPostition.y);
                        //ConsoleUtils.Log("moveY-Top", moveY);
                    }
                    //ConsoleUtils.Log("moveY-None-Collide", move.lastPosition, move.nextPostition);
                }
                else
                {
                    Vector3 forward = totalNormal + inputForward;
                    if (totalNormal.y > 0.9 && totalNormal.y < 1 || totalNormal.y <= 0)
                    {
                        //去除抖动
                        if (inputForward.x > 0 && totalNormal.x < 0 || inputForward.x < 0 && totalNormal.x > 0)
                        {
                            forward.x = 0;
                        }

                        if (inputForward.z > 0 && totalNormal.z < 0 || inputForward.z < 0 && totalNormal.z > 0)
                        {
                            forward.z = 0;
                        }
                        forward.y = 0;
                        move.nextPostition = move.lastPosition + forward * move.speed + offset;
                    }
                    else
                    {
                        //爬坡
                        float totalNormalY = totalNormal.y;

                        //if (Mathf.Abs(totalNormal.x) == 1)
                        //{
                        //    totalNormal.x = 0;
                        //}
                        //else if (Mathf.Abs(totalNormal.z) == 1)
                        //{
                        //    totalNormal.z = 0;
                        //}
                        float absX = Mathf.Abs(totalNormal.x);
                        float absZ = Mathf.Abs(totalNormal.z);
                        if (absX > 0 && absX < 1)
                        {
                            if (totalNormal.x > 0 && inputForward.x < 0 || totalNormal.x < 0 && inputForward.x > 0)
                            {
                                //forward.y = Mathf.Abs(totalNormal.x) * Mathf.Abs(inputForward.x);
                                moveY += Mathf.Abs(totalNormal.x) * Mathf.Abs(inputForward.x) * move.speed;
                                TransformSingleton.Ins().SetMoveY(entity.id, moveY);
                            }
                            else
                            {
                                //forward.y = -Mathf.Abs(totalNormal.x) * Mathf.Abs(inputForward.x);
                                moveY += -Mathf.Abs(totalNormal.x) * Mathf.Abs(inputForward.x) * move.speed;
                                TransformSingleton.Ins().SetMoveY(entity.id, moveY);
                            }

                            forward.x = totalNormalY * inputForward.x;
                        }

                        if (absZ > 0 && absZ < 1)
                        {
                            if (totalNormal.z > 0 && inputForward.z < 0 || totalNormal.z < 0 && inputForward.z > 0)
                            {
                                //forward.y = Mathf.Abs(totalNormal.z) * Mathf.Abs(inputForward.z);
                                moveY += Mathf.Abs(totalNormal.z) * Mathf.Abs(inputForward.z) * move.speed;
                                TransformSingleton.Ins().SetMoveY(entity.id, moveY);
                            }
                            else
                            {
                                //forward.y = -Mathf.Abs(totalNormal.z) * Mathf.Abs(inputForward.z);
                                moveY += -Mathf.Abs(totalNormal.z) * Mathf.Abs(inputForward.z) * move.speed;
                                TransformSingleton.Ins().SetMoveY(entity.id, moveY);
                            }
                            forward.z = totalNormalY * inputForward.z;
                        }
                        forward.y = 0;
                        move.nextPostition = move.lastPosition + forward * move.speed;
                    }
                    //ConsoleUtils.Log("移动", forward, totalNormal, inputForward, "坐标", move.lastPosition, move.nextPostition, moveY);
                    //ConsoleUtils.Log("moveY", move.lastPosition, move.nextPostition, offset);
                }

                if (move.nextPostition.y + moveY < lower)
                {
                    //move.nextPostition.y = lower;
                    TransformSingleton.Ins().SetMoveY(entity.id, 0);
                }

                //if (curY < move.nextPostition.y)
                //{
                //    TransformSingleton.Ins().SetCurY(entity.id, move.nextPostition.y);
                //}
                move.isMove = true;

            }
            else
            {
                move.isMove = false;
            }

            //保存计算后坐标
            Vector3 calculatedPosition = move.nextPostition;
            calculatedPosition.y += moveY;
            TransformSingleton.Ins().SetCalculatedPosition(entity.id, calculatedPosition);
            //if(float.NaN.Equals(calculatedPosition.x))
            //{
            //    ConsoleUtils.Log("计算错误");
            //}
            
        }
    }

    public override void OnDrawGizmos(List<Entity> entities)
    {
        foreach (Entity entity in entities)
        {
            MoveComp move = entity.Get<MoveComp>();
            Vector3 inputForward = InputSingleton.Ins().GetForward(entity.id);
            //Gizmos.DrawLine(move.lastPosition, (move.lastPosition + inputForward * 5));

            List<CollideComp> colliders = CollisionSingleton.Ins().GetColliders(entity.id);
            List<Vector3> normals = CollisionSingleton.Ins().GetNormalList(entity.id);
            Vector3 totalNormal = CollisionSingleton.Ins().GetTotalNormal(entity.id);

            for (int i = 0; i < colliders.Count; i++)
            {
                Vector3 forward = totalNormal + inputForward;
                if (totalNormal.y > 0.9 || totalNormal.y == 0)
                {
                    //去除抖动
                    if (inputForward.x > 0 && totalNormal.x < 0 || inputForward.x < 0 && totalNormal.x > 0)
                    {
                        forward.x = 0;
                    }

                    if (inputForward.z > 0 && totalNormal.z < 0 || inputForward.z < 0 && totalNormal.z > 0)
                    {
                        forward.z = 0;
                    }
                }
                else
                {
                    //爬坡
                    float totalNormalY = totalNormal.y;
                    if (totalNormal.x != 0)
                    {
                        if (totalNormal.x > 0 && inputForward.x < 0 || totalNormal.x < 0 && inputForward.x > 0)
                        {
                            forward.y = Mathf.Abs(totalNormal.x);
                        }
                        else
                        {
                            forward.y = -Mathf.Abs(totalNormal.x);
                        }

                        forward.x = totalNormalY * inputForward.x;
                    }
                    else if (totalNormal.z != 0)
                    {
                        if (totalNormal.z > 0 && inputForward.z < 0 || totalNormal.z < 0 && inputForward.z > 0)
                        {
                            forward.y = Mathf.Abs(totalNormal.z);
                        }
                        else
                        {
                            forward.y = -Mathf.Abs(totalNormal.z);
                        }
                        forward.z = totalNormalY * inputForward.z;
                    }
                }

                CollideComp coll = colliders[i];
                if (coll.type == CollisionType.OBB)
                {
                    Gizmos.DrawLine(coll.obb.position, (coll.obb.position + normals[i] * 5));
                }
                else
                {
                    Gizmos.DrawLine(coll.aabb.position, (coll.aabb.position + normals[i] * 5));

                }
                Gizmos.DrawLine(move.lastPosition, (move.lastPosition + forward * 5));
            }
        }
    }
}
