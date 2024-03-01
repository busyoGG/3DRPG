using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class QtreeManager : Singleton<QtreeManager>
{
    QTree _qtree = null;

    /// <summary>
    /// 创建一个四叉树
    /// </summary>
    /// <param name="bounds"></param>
    /// <param name="maxDepth"></param>
    /// <param name="maxCount"></param>
    public void CreateQtree(AABBData bounds, int maxDepth, int maxCount)
    {
        _qtree = new QTree(bounds, maxDepth, maxCount);
    }

    /// <summary>
    /// 创建一个包围盒
    /// </summary>
    /// <param name="center"></param>
    /// <param name="size"></param>
    /// <returns></returns>
    public AABBData CreateBounds(Vector3 position, Vector3 size)
    {
        AABBData bounds = new AABBData();
        bounds.position = position;
        bounds.size = size;
        return bounds;
    }

    /// <summary>
    /// 创建四叉树插入对象
    /// </summary>
    /// <param name="bounds"></param>
    /// <param name="gObj"></param>
    /// <returns></returns>
    public QTreeObj CreateQtreeObj(AABBData bounds, Entity entity)
    {
        QTreeObj obj = new QTreeObj();
        obj.bounds = bounds;
        obj.id = entity.id;
        obj.entity = entity;
        return obj;
    }

    /// <summary>
    /// 插入四叉树
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public QNode Insert(QTreeObj obj)
    {
        QNode node = _qtree.Insert(obj);
        obj.qNode = node;
        return node;
    }

    /// <summary>
    /// 移除对象
    /// </summary>
    /// <param name="obj"></param>
    public void Remove(QTreeObj obj)
    {
        _qtree.Remove(obj);
    }

    /// <summary>
    /// 查找对象
    /// </summary>
    /// <param name="bounds"></param>
    /// <returns></returns>
    public List<QTreeObj> Find(AABBData bounds)
    {
        return _qtree.Find(bounds);
    }

    public void DrawQtree()
    {
        DrawLine(_qtree._root._bounds.position, _qtree._root._bounds.halfSize, Color.green);
        DrawRect(_qtree._root);
    }

    private void DrawRect(QNode parent)
    {
        foreach (QNode child in parent._children)
        {
            DrawLine(child._bounds.position, child._bounds.halfSize,Color.red);
            DrawRect(child);
        }
    }

    private void DrawLine(Vector3 center, Vector3 halfSize,Color color)
    {
        Gizmos.color = color;
        Vector3[] points = {
            new Vector3(center.x - halfSize.x,0,center.z - halfSize.z),
            new Vector3(center.x + halfSize.x,0,center.z - halfSize.z),
            new Vector3(center.x + halfSize.x,0,center.z + halfSize.z),
            new Vector3(center.x - halfSize.x,0,center.z + halfSize.z)
        };
        Gizmos.DrawLine(points[0], points[1]);
        Gizmos.DrawLine(points[1], points[2]);
        Gizmos.DrawLine(points[2], points[3]);
        Gizmos.DrawLine(points[3], points[0]);
    }
}

