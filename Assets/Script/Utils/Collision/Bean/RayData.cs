
using UnityEngine;
public class RayData: ICollide
{
    public Vector3 origin;

    public Vector3 forward;

    public float length;

    public Vector3 target
    {
        get
        {
            if (_target == null)
            {
                _target = origin + forward * length;
            }
            return _target;
        }
    }

    private Vector3 _target;

    public override bool Interactive(AABBData ohter, out Vector3 point)
    {
        return CollideFunction.CheckCollide(this, ohter, out point);
    }

    public override bool Interactive(OBBData ohter, out Vector3 point)
    {
        return CollideFunction.CheckCollide(this, ohter, out point);
    }

    public override bool Interactive(CircleData ohter, out Vector3 point)
    {
        return CollideFunction.CheckCollide(this, ohter, out point);
    }
    //public override bool Interactive(RayData ohter, out Vector3 point)
    //{
    //    return CollideFunction.CheckCollide(this, ohter, out point);
    //}
}
