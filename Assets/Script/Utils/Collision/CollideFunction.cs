using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollideFunction
{
    
    public static bool CheckCollide(AABBData data1, AABBData data2, out Vector3 point)
    {
        point = CollideUtils._minValue;
        return CollideUtils.CollisionAABB(data1, data2);
    }

    public static bool CheckCollide(OBBData data1, OBBData data2, out Vector3 point)
    {
        point = CollideUtils._minValue;
        return CollideUtils.CollisionOBB(data1, data2);
    }

    public static bool CheckCollide(CircleData data1, CircleData data2, out Vector3 point)
    {
        point = CollideUtils._minValue;
        return CollideUtils.CollisionCircle(data1, data2);
    }

    public static bool CheckCollide(CircleData data1, AABBData data2, out Vector3 point)
    {
        return CollideUtils.CollisionCircle2AABB(data1, data2, out point);
    }

    public static bool CheckCollide(CircleData data1, OBBData data2, out Vector3 point)
    {
        return CollideUtils.CollisionCircle2OBB(data1, data2, out point);
    }

    public static bool CheckCollide(RayData data1, AABBData data2, out Vector3 point)
    {
        return CollideUtils.CollisionRay2AABB(data1, data2, out point);
    }

    public static bool CheckCollide(RayData data1, OBBData data2, out Vector3 point)
    {
        return CollideUtils.CollisionRay2OBB(data1, data2, out point);
    }

    public static bool CheckCollide(RayData data1, CircleData data2, out Vector3 point)
    {
        return CollideUtils.CollisionRay2Circle(data1, data2, out point);
    }

    //----- ¾µÏñº¯Êý -----

    public static bool CheckCollide(AABBData data1, OBBData data2, out Vector3 point)
    {
        point = CollideUtils._minValue;
        return CollideUtils.CollisionOBB(data1.obb, data2);
    }

    public static bool CheckCollide(OBBData data1, AABBData data2, out Vector3 point)
    {
        point = CollideUtils._minValue;
        return CollideUtils.CollisionOBB(data1, data2.obb);
    }

    public static bool CheckCollide(AABBData data1, CircleData data2, out Vector3 point)
    {
        return CollideUtils.CollisionCircle2AABB(data2, data1,out point);
    }

    public static bool CheckCollide(OBBData data1, CircleData data2, out Vector3 point)
    {
        return CollideUtils.CollisionCircle2OBB(data2, data1, out point);
    }

    public static bool CheckCollide(AABBData data1, RayData data2, out Vector3 point)
    {
        return CollideUtils.CollisionRay2AABB(data2, data1, out point);
    }

    public static bool CheckCollide(OBBData data1, RayData data2, out Vector3 point)
    {
        return CollideUtils.CollisionRay2OBB(data2, data1, out point);
    }

    public static bool CheckCollide(CircleData data1, RayData data2, out Vector3 point)
    {
        return CollideUtils.CollisionRay2Circle(data2, data1, out point);
    }
}
