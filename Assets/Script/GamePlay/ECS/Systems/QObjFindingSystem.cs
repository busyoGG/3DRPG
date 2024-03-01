using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class QObjFindingSystem : ECSSystem
{
    public override ECSMatcher Filter()
    {
        return ECSManager.Ins().AllOf(typeof(QTreeComp)).AnyOf(typeof(CollideComp), typeof(TriggerComp));
    }

    public override void OnUpdate(List<Entity> entities)
    {
        foreach (Entity entity in entities)
        {
            TriggerComp trigger = entity.Get<TriggerComp>();
            CollideComp collider = entity.Get<CollideComp>();

            bool isFind = collider != null || trigger != null && trigger.isPositive;

            //List<QTreeObj> qTreeObjs = QTreeSingleton.Ins().GetQtreeObjs(entity.id);
            //qTreeObjs.Clear();
            if (isFind)
            {
                QTreeComp qTree = entity.Get<QTreeComp>();
                AABBData bounds = qTree.qObj.bounds;
                List<QTreeObj> objs = QtreeManager.Ins().Find(bounds);
                //qTreeObjs.AddRange(objs);
                qTree.foundObjs.Clear();
                qTree.foundObjs.AddRange(objs);
            }
        }
    }
}
