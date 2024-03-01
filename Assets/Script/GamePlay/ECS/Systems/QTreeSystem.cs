
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QTreeSystem : ECSSystem
{
    public override ECSMatcher Filter()
    {
        return ECSManager.Ins().AllOf(typeof(QTreeComp), typeof(TransformComp));
    }

    public override void OnUpdate(List<Entity> entities)
    {
        foreach (Entity entity in entities)
        {
            QTreeComp qTree = entity.Get<QTreeComp>();
            TransformComp transform = entity.Get<TransformComp>();

            qTree.qObj.RefreshBounds(transform.position);
        }
    }

    public override void OnDrawGizmos(List<Entity> entities)
    {
        foreach (Entity entity in entities)
        {
            QTreeComp qTree = entity.Get<QTreeComp>();
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(qTree.qObj.bounds.position, qTree.qObj.bounds.size);
        }
    }
}
