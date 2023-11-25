using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CollisionType
{
    Circle,
    AABB,
    OBB,
    Ray
}

[CompRegister(typeof(CollideComp))]
public class CollideComp : Comp
{
    public CollisionType type;

    public Vector3 position;

    public CircleData circle;

    public AABBData aabb;

    public OBBData obb;

    public RayData ray;

    /// <summary>
    /// ÍÆÁ¦
    /// </summary>
    public float thrust;
    /// <summary>
    /// Ä¦²ÁÁ¦
    /// </summary>
    public float friction;

    public bool isStatic;

    public Vector3 totalOffset;

    public float maxY
    {
        get
        {
            switch (type)
            {
                case CollisionType.Circle:
                    return circle.position.y + circle.radius;
                case CollisionType.AABB:
                    return aabb.max.y;
                case CollisionType.OBB:
                    return obb.maxY;
                case CollisionType.Ray:
                    break;
            }
            return 0;
        }
    }

    public float minY
    {
        get
        {
            switch (type)
            {
                case CollisionType.Circle:
                    return circle.position.y - circle.radius;
                case CollisionType.AABB:
                    return aabb.min.y;
                case CollisionType.OBB:
                    return obb.minY;
                case CollisionType.Ray:
                    break;
            }
            return 0;
        }
    }

    public override void Reset()
    {
        type = CollisionType.Circle;
        position = Vector3.zero;
        circle = null;
        aabb = null;
        obb = null;
        ray = null;
        thrust = 0.0f;
        friction = 0.0f;
        isStatic = true;
        totalOffset = Vector3.zero;
    }
}
