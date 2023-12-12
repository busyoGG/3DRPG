using UnityEngine;

public class ICollide
{
    public virtual bool Interactive(AABBData other, out Vector3 point) { point = CollideUtils._minValue; return false; }
    public virtual bool Interactive(OBBData other, out Vector3 point) { point = CollideUtils._minValue; return false; }
    public virtual bool Interactive(CircleData other, out Vector3 point) { point = CollideUtils._minValue; return false; }
    public virtual bool Interactive(RayData other, out Vector3 point) { point = CollideUtils._minValue; return false; }
}
