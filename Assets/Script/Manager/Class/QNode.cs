using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QNode
{
    private QNode _parent;

    private QTree _belong;

    private AABBData _bounds;

    private AABBData _looseBounds;

    private List<QNode> _children;

    private List<QTreeObj> _objs;

    private int _depth;

    private bool _isLeaf;

    private int _objCount = 0;

    public QNode(AABBData bounds, QNode parent, QTree belong, int depth)
    {
        _bounds = bounds;
        _parent = parent;
        _belong = belong;
        _depth = depth;
        _children = new List<QNode>();
        _objs = new List<QTreeObj>();
        if (_depth == belong._maxDepth)
        {
            _isLeaf = true;
        }
        else
        {
            _isLeaf = false;
        }

        _looseBounds = new AABBData();
        _looseBounds.position = bounds.position;
        _looseBounds.size = bounds.size * 2;

    }

    public QNode Insert(QTreeObj obj)
    {
        if (_children.Count == 0 && (_depth == 0 || !_isLeaf && this._objCount > _belong._maxCount))
        {

            CreateChildren();
            if (_objs.Count > 0)
            {
                for (int j = _objs.Count; j >= 0; j--)
                {
                    if (_children.Count > 0)
                    {
                        for (int i = 0; i < 4; i++)
                        {
                            QNode temp = _children[i];
                            if (temp != null)
                            {
                                Debug.Log("_objs.Count:" + _objs.Count + " i=" + i + " j=" + j + " _children.cout:" + _children.Count);
                                if (CheckInBounds(_objs[j].bounds, temp._bounds))
                                {
                                    temp.Insert(_objs[j]);
                                    _objs.RemoveAt(j);
                                    break;
                                }
                            }
                        }
                    }
                }
            }

        }

        if (_children.Count > 0)
        {
            for (int i = 0; i < 4; i++)
            {
                if (CheckInBounds(obj.bounds, _children[i]._bounds))
                {
                    ++this._objCount;
                    return _children[i].Insert(obj);
                }
            }
        }

        _objs.Add(obj);
        ++this._objCount;

        return this;
    }

    public void Remove(QTreeObj obj)
    {
        if (_parent != null && _parent._objCount <= 0)
        {
            _parent._children.Clear();
        }

        if (_objs.Count > 0)
        {

        }
        for (int i = _objs.Count - 1; i >= 0; i--)
        {
            if (_objs[i].id == obj.id)
            {
                _objs.RemoveAt(i);
                --_objCount;
                return;
            }
        }

        if (_children.Count > 0)
        {
            for (int i = 0; i < 4; i++)
            {
                if (CheckInBounds(obj.bounds, _children[i]._bounds))
                {
                    _children[i].Remove(obj);
                    --_objCount;
                    return;
                }
            }
        }

    }

    public List<QTreeObj> Find(AABBData bounds)
    {
        List<QTreeObj> res = new List<QTreeObj>();

        int len = _objs.Count;
        for (int i = 0; i < len; i++)
        {
            if (CheckIntersects(bounds, _objs[i].bounds))
            {
                res.Add(_objs[i]);
            }
        }

        if (_children.Count > 0)
        {
            for (int i = 0; i < 4; i++)
            {
                //Debug.Log("输出错误的下标:" + i + "  限定长度：" + _children.Count);
                if (CheckIntersects(bounds, _children[i]._looseBounds))
                {
                    res.AddRange(_children[i].Find(bounds));
                }
            }
        }

        return res;
    }

    public QNode Refresh(QTreeObj obj)
    {
        if (CheckInBounds(obj.bounds, _bounds))
        {
            return this;
        }
        else
        {
            Remove(obj);
            return _belong.Insert(obj);
        }
    }

    /// <summary>
    /// 创建四叉树子节点
    /// </summary>
    private void CreateChildren()
    {
        for (int i = 0; i < 4; i++)
        {
            AABBData bounds = new AABBData();
            float newX = _bounds.position.x + _bounds.halfSize.x * Mathf.Pow(-1, i + 1);
            float newZ = _bounds.position.z + _bounds.halfSize.z * (i < 2 ? 1 : -1);
            bounds.position = new Vector3(newX, _bounds.position.y, newZ);
            bounds.size = _bounds.size * 0.5f;
            QNode child = new QNode(bounds, this, _belong, _depth + 1);
            _children.Add(child);
        }
    }

    /// <summary>
    /// 检测bounds1中心点是否在bounds2内
    /// </summary>
    /// <param name="bounds1"></param>
    /// <param name="bounds2"></param>
    /// <returns></returns>
    private bool CheckInBounds(AABBData bounds1, AABBData bounds2)
    {
        if (bounds1.position.x <= bounds2.max.x && bounds1.position.x >= bounds2.min.x &&
            bounds1.position.z <= bounds2.max.z && bounds1.position.x >= bounds2.min.z)
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// 检测bounds1和bounds2是否相交
    /// </summary>
    /// <param name="bounds1"></param>
    /// <param name="bounds2"></param>
    /// <returns></returns>
    private bool CheckIntersects(AABBData bounds1, AABBData bounds2)
    {
        return CollideUtils.CollisionAABB(bounds1, bounds2);
    }
}

