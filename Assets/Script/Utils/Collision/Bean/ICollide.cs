using UnityEngine;

public class ICollide
{
    public virtual bool Interactive(AABBData other, out Vector3 point) { point = CollideUtils._minValue; return false; }
    public virtual bool Interactive(OBBData other, out Vector3 point) { point = CollideUtils._minValue; return false; }
    public virtual bool Interactive(CircleData other, out Vector3 point) { point = CollideUtils._minValue; return false; }
    public virtual bool Interactive(RayData other, out Vector3 point) { point = CollideUtils._minValue; return false; }
    public virtual Vector3 GetNormal(AABBData data2, out float len) { len = 0; return Vector3.zero; }
    public virtual Vector3 GetNormal(OBBData data2, out float len) { len = 0; return Vector3.zero; }
}
