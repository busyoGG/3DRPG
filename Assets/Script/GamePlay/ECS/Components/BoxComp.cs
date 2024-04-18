using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CompRegister(typeof(BoxComp))]
public class BoxComp : Comp
{
    public CollisionType type;

    public Vector3 position;

    public CircleData circle;

    public AABBData aabb;

    public OBBData obb;

    public RayData ray;

    public CapsuleData capsule;

    public bool isPositive;

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
                case CollisionType.Capsule:
                    return capsule.maxY;
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
                case CollisionType.Capsule:
                    return capsule.minY;
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
        isPositive = false;
    }
}
