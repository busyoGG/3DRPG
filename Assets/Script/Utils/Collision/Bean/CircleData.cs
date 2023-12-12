using UnityEngine;

public class CircleData: ICollide
{
    public Vector3 position;

    public float radius;
    public override bool Interactive(AABBData ohter, out Vector3 point)
    {
        return CollideFunction.CheckCollide(this, ohter, out point);
    }

    public override bool Interactive(OBBData ohter, out Vector3 point)
    {
        return CollideFunction.CheckCollide(this, ohter, out point);
    }

    public override bool Interactive(RayData ohter, out Vector3 point)
    {
        return CollideFunction.CheckCollide(this, ohter, out point);
    }

    public override bool Interactive(CircleData ohter, out Vector3 point)
    {
        return CollideFunction.CheckCollide(this, ohter, out point);
    }
}
