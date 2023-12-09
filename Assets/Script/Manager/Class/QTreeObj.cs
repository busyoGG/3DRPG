using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class QTreeObj
{
    public int id { get; set; }

    //public GameObject node { get; set; }
    public Entity entity { get; set; }

    public AABBData bounds { get; set; }

    public QNode qNode { get; set; }

    public QTreeObj() { }

    public QTreeObj(int id, Entity entity, AABBData bounds)
    {
        this.id = id;
        this.entity = entity;
        this.bounds = bounds;
    }
    /// <summary>
    /// 刷新包围盒，并更新在四叉树中的节点位置
    /// </summary>
    /// <param name="pos"></param>
    public void RefreshBounds(Vector3 pos)
    {
        if (bounds.position != pos)
        {
            bounds.position = pos;
            qNode = qNode.Refresh(this);
        }
    }
}
