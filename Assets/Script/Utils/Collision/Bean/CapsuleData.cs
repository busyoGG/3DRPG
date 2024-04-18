using UnityEngine;

public class CapsuleData : ICollide
{
    public CollisionType type = CollisionType.Capsule;

    public Vector3 position
    {
        get
        {
            return _position;
        }
        set
        {
            _position = value;
            _maxY = 0;
            _minY = 0;
        }
    }

    private Vector3 _position;

    public float height { get; set; }
    
    public Quaternion rot
    {
        get
        {
            return _rot;
        }
        set
        {
            _rot = value;
            _maxY = 0;
            _minY = 0;
        }
    }

    private Quaternion _rot;
    
    public float radius { get; set; }
    
    public float maxY
    {
        get
        {
            if (_maxY == 0)
            {
                _maxY = (position + rot * Vector3.up * height * 0.5f).y + radius;
            }
            return _maxY;
        }
    }

    private float _maxY;

    public float minY
    {
        get
        {
            if (_minY == 0)
            {
                _minY = (position - rot * Vector3.up * height * 0.5f).y - radius;
            }
            return _minY;
        }
    }

    private float _minY;

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

    public override bool Interactive(RayData ohter, out Vector3 point)
    {
        return CollideFunction.CheckCollide(this, ohter, out point);
    }
    
    public override Vector3 GetNormal(AABBData other, out float len)
    {
        string id = this.GetHashCode() + "-" + other.GetHashCode();
        return CollideUtils.GetCollideNormal(id,radius, out len);
    }

    public override Vector3 GetNormal(OBBData other, out float len)
    {
        string id = this.GetHashCode() + "-" + other.GetHashCode();
        return CollideUtils.GetCollideNormal(id,radius ,out len);
    }
    
    public override Vector3 GetNormal(CapsuleData other, out float len)
    {
        string id = this.GetHashCode() + "-" + other.GetHashCode();
        return CollideUtils.GetCollideNormal(id,radius + other.radius, out len);
    }
}
