using Game;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QTreeSystem : ECSSystem
{
    public override ECSMatcher Filter()
    {
        return ECSManager.Ins().AllOf(typeof(QTreeComp), typeof(MoveComp));
    }

    public override void OnUpdate(List<Entity> entities)
    {
        foreach (Entity entity in entities)
        {
            QTreeComp qTree = entity.Get<QTreeComp>();
            MoveComp move = entity.Get<MoveComp>();

            if (move.lastPosition != move.nextPostition || move.isFall)
            {
                qTree.qObj.RefreshBounds(move.nextPostition);
            }

        }
    }
}
