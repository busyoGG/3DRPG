using Game;
using System.Drawing;
using UnityEngine;
using UnityEngine.UIElements;

public class OBBData
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

    public Vector3[] axes { 
        get
        {
            return _axes;
        }
        set
        {
            _axes = value;
            _vertexts = null;
            _aabb = null;
        }
    }

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
                for (int i = 0; i < _vertexts.Length; i++)
                {
                    _maxY = Mathf.Max(_maxY, _vertexts[i].y);
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
                for (int i = 0; i < _vertexts.Length; i++)
                {
                    _minY = Mathf.Min(_minY, _vertexts[i].y);
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

    //private void GetMaxMin()
    //{
    //    _max = vertexts[0];
    //    _min = vertexts[0];
    //    for (int i = 1, len = vertexts.Length; i < len; i++)
    //    {
    //        if (_max.x < vertexts[i].x && _max.y < vertexts[i].y && _max.z < vertexts[i].z)
    //        {
    //            _max = vertexts[i];
    //        }

    //        if (_min.x > vertexts[i].x && _min.y > vertexts[i].y && _min.z > vertexts[i].z)
    //        {
    //            _min = vertexts[i];
    //        }
    //    }
    //}
}
