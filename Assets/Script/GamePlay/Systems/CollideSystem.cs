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
            //��ʼ����ײ����
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

            if (colliders.Count > 0 && move.isClimbTop != 2)
            {
                move.isTop = true;
                move.isSlope = false;
                move.isClimbTop = 1;
            }

            float minClose = float.MaxValue;
            //��˳������ײ
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
                        //�����µ�����
                        move.isSlope = true;
                        Vector3 inputForward = InputSingleton.Ins().GetForward(entity.id);
                        //��ֹ����
                        if (!inputForward.Equals(Vector3.zero))
                        {
                            move.forwardOffset = normal;
                        }

                        if (normal.y >= 1 || move.isClimbTop == 2)
                        {
                            collider.totalOffset += normal * len;
                        }
                    }
                    else
                    {
                        //����ƫ�Ʒ���
                        if ((collider.totalOffset.y > 0 || move.nextPostition.y <= 1) && normal.y < 0)
                        {
                            normal.y = 0;
                        }
                        collider.totalOffset += normal * len;

                        //������ƫ��
                        Vector3 closest = GetClosestPoint(collider.position, collideComp);
                        //������ߵ�
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
                        //�ж��Ƿ��������� û�еĻ��ͼ�����������
                        if (_tempVec.y - higherest.y < 0.1f)
                        {
                            move.isClimbTop = 0;
                            //������������
                            Vector3 dir = collider.position - closest;
                            if (dir.magnitude < minClose)
                            {
                                minClose = dir.magnitude;
                                move.forwardOffset = dir;
                            }
                        }
                    }
                    //ˢ�°�Χ��
                    RefreshCollider(collider, normal * len);
                    //�ж��Ƿ�վ�ڶ���
                    if (collider.minY < collideComp.maxY)
                    {
                        move.isTop = false;
                    }
                }
            }

            if (move.isClimb)
            {
                //��������״̬ƫ��
                if (collider.totalOffset.x != 0)
                {
                    collider.totalOffset.x -= move.forwardOffset.normalized.x * 0.05f;
                }
                if (collider.totalOffset.z != 0)
                {
                    collider.totalOffset.z -= move.forwardOffset.normalized.z * 0.05f;
                }
                //collider.totalOffset -= move.forwardOffset.normalized * 0.05f ;
            }
            else
            {
                //û������ ������������״̬
                move.isClimbTop = 0;
            }

            //������������״̬
            if (move.isClimbTop == 2)
            {
                move.isClimbTop = 0;
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
        //���°�Χ��
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
    /// ��ȡ�����
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
