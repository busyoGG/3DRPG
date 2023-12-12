using UnityEngine;

public class AABBData : ICollide
{
    public CollisionType type = CollisionType.AABB;

    public Vector3 position
    {
        get
        {
            return _position;
        }
        set
        {
            _position = value;
            _max = Vector3.zero;
            _min = Vector3.zero;
            _obb = null;
            _vertexts = null;
        }
    }

    private Vector3 _position;

    public Vector3 size;

    public Vector3 halfSize
    {
        get
        {
            if (_halfSize == Vector3.zero)
            {
                _halfSize = size * 0.5f;
            }
            return _halfSize;
        }
    }
    private Vector3 _halfSize;

    public Vector3 max
    {
        get
        {
            if (_max == Vector3.zero)
            {
                _max = position + halfSize;
            }
            return _max;
        }
    }
    private Vector3 _max;

    public Vector3 min
    {
        get
        {
            if (_min == Vector3.zero)
            {
                _min = position - halfSize;
            }
            return _min;
        }
    }

    private Vector3 _min;

    //public float maxY
    //{
    //    get
    //    {
    //        return max.y;
    //    }
    //}

    //public float minY
    //{
    //    get
    //    {
    //        return min.y;
    //    }
    //}

    public OBBData obb
    {
        get
        {
            if (_obb == null)
            {
                _obb = new OBBData();
                _obb.position = position;
                _obb.size = size;
                _obb.axes = new Vector3[3] { new Vector3(1, 0, 0), new Vector3(0, 1, 0), new Vector3(0, 0, 1) };
            }
            return _obb;
        }
    }
    private OBBData _obb;

    public Vector3[] vertexts
    {
        get
        {
            if (_vertexts == null)
            {
                _vertexts = new Vector3[8];
                int i = 0;
                for (int indX = -1; indX < 2; indX += 2)
                {
                    for (int indY = -1; indY < 2; indY += 2)
                    {
                        for (int indZ = -1; indZ < 2; indZ += 2)
                        {
                            Vector3 center = position;
                            center.x += indX * halfSize.x;
                            center.y += indY * halfSize.y;
                            center.z += indZ * halfSize.z;
                            _vertexts[i++] = center;
                        }
                    }
                }
            }
            return _vertexts;
        }
        set
        {
            _vertexts = value;
        }
    }

    private Vector3[] _vertexts;

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
}
