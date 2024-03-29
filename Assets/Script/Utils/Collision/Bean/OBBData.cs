
using System.Drawing;
using UnityEngine;
using UnityEngine.UIElements;

public class OBBData : ICollide
{
    public Vector3 position
    {
        get
        {
            return _position;
        }
        set
        {
            _position = value;
            _vertexts = null;
            _aabb = null;
            _maxY = float.MinValue;
            _minY = float.MaxValue;
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

    public Vector3[] axes
    {
        get
        {
            return _axes;
        }
        set
        {
            _axes = value;
            _vertexts = null;
            _aabb = null;
            _rot = Quaternion.LookRotation(_axes[2]);
        }
    }

    public Quaternion rot
    {
        get
        {
            return _rot;
        }
        set
        {
            _rot = value;
            _axes = new Vector3[3]
            {
                _rot * Vector3.right,
                _rot * Vector3.up,
                _rot * Vector3.forward,
            };
        }
    }

    private Quaternion _rot;

    private Vector3[] _axes;

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
                            for (int k = 0; k < 3; k++)
                            {
                                float len = 0;
                                switch (k)
                                {
                                    case 0:
                                        len = indX * halfSize.x;
                                        break;
                                    case 1:
                                        len = indY * halfSize.y;
                                        break;
                                    case 2:
                                        len = indZ * halfSize.z;
                                        break;
                                }
                                center += len * axes[k];
                            }
                            _vertexts[i++] = center;
                        }
                    }
                }
            }
            return _vertexts;
        }
    }

    private Vector3[] _vertexts;

    public float maxY
    {
        get
        {
            if (_maxY == float.MinValue)
            {
                for (int i = 0; i < vertexts.Length; i++)
                {
                    _maxY = Mathf.Max(_maxY, vertexts[i].y);
                }
            }
            return _maxY;
        }
    }

    private float _maxY = float.MinValue;

    public float minY
    {
        get
        {
            if (_minY == float.MaxValue)
            {
                for (int i = 0; i < vertexts.Length; i++)
                {
                    _minY = Mathf.Min(_minY, vertexts[i].y);
                }
            }
            return _minY;
        }
    }

    private float _minY = float.MaxValue;

    public AABBData aabb
    {
        get
        {
            if (_aabb == null)
            {
                _aabb = new AABBData();
                _aabb.position = position;

                Vector3 min = vertexts[0];
                Vector3 max = vertexts[0];

                for (int i = 1, len = vertexts.Length; i < len; i++)
                {
                    Vector3 v = vertexts[i];
                    min = Vector3.Min(min, v);
                    max = Vector3.Max(max, v);
                }
                _aabb.size = max - min;
            }
            return _aabb;
        }
    }

    public AABBData _aabb;
    public override bool Interactive(OBBData ohter, out Vector3 point)
    {
        return CollideFunction.CheckCollide(this, ohter, out point);
    }

    public override bool Interactive(AABBData ohter, out Vector3 point)
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
        return CollideUtils.GetCollideNormal(aabb, other, out len);
    }

    public override Vector3 GetNormal(OBBData other, out float len)
    {
        return CollideUtils.GetCollideNormal(this, other, out len);
    }
}
