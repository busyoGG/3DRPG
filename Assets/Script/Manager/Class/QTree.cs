using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class QTree
{
    /// <summary>
    /// 最大深度
    /// </summary>
    public int _maxDepth;
    /// <summary>
    /// 节点内最大对象数
    /// </summary>
    public int _maxCount;

    public QNode _root;

    public QTree(AABBData bounds, int maxDepth, int maxCount)
    {
        _root = new QNode(bounds, null, this, 0);
        _maxDepth = maxDepth;
        _maxCount = maxCount;
    }

    public QNode Insert(QTreeObj obj)
    {
        return _root.Insert(obj);
    }

    public void Remove(QTreeObj obj)
    {
        _root.Remove(obj);
    }

    public List<QTreeObj> Find(AABBData bounds)
    {
        return _root.Find(bounds);
    }

    public QNode Refresh(QTreeObj obj)
    {
        return _root.Refresh(obj);
    }
}