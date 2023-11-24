using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class QtreeManager:Singleton<QtreeManager>
{
    private List<QTree> _qtree = new List<QTree>();

    /// <summary>
    /// 创建一个四叉树
    /// </summary>
    /// <param name="bounds"></param>
    /// <param name="maxDepth"></param>
    /// <param name="maxCount"></param>
    public void CreateQtree(AABBData bounds, int maxDepth, int maxCount)
    {
        _qtree.Add(new QTree(bounds, maxDepth, maxCount));
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
    public QNode Insert(int id,QTreeObj obj)
    {
        QNode node = _qtree[id].Insert(obj);
        obj.qNode = node;
        return node;
    }

    /// <summary>
    /// 移除对象
    /// </summary>
    /// <param name="obj"></param>
    public void Remove(int id, QTreeObj obj)
    {
        _qtree[id].Remove(obj);
    }

    /// <summary>
    /// 查找对象
    /// </summary>
    /// <param name="bounds"></param>
    /// <returns></returns>
    public List<QTreeObj> Find(int id, AABBData bounds)
    {
        return _qtree[id].Find(bounds);
    }
}

