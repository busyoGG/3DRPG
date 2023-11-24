using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CollisionType { 
    Circle,
    AABB,
    OBB,
    Ray
}

[CompRegister(typeof(CollideComp))]
public class CollideComp : Comp
{
    public CollisionType type;

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

    public override void Reset()
    {
        type = CollisionType.Circle;
        circle = null;
        aabb = null;
        obb = null;
        ray = null;
        thrust = 0.0f;
        friction = 0.0f;
        isStatic = true;
    }
}
