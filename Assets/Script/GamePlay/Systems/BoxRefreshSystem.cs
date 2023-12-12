using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxRefreshSystem : ECSSystem
{
    public override ECSMatcher Filter()
    {
        return ECSManager.Ins().AllOf(typeof(BoxComp), typeof(MoveComp), typeof(CollideComp));
    }

    public override void OnUpdate(List<Entity> entities)
    {
        foreach (var entity in entities)
        {
            BoxComp box = entity.Get<BoxComp>();
            MoveComp move = entity.Get<MoveComp>();
            CollideComp col = entity.Get<CollideComp>();

            //¸üÐÂ°üÎ§ºÐ
            box.position = move.nextPostition;
            col.totalOffset = Vector3.zero;
            move.forwardOffset = Vector3.zero;

            switch (box.type)
            {
                case CollisionType.AABB:
                    if (box.aabb.position != move.nextPostition)
                    {
                        box.aabb.position = move.nextPostition;
                    }
                    break;
                case CollisionType.OBB:
                    if (box.obb.position != move.nextPostition)
                    {
                        box.obb.position = move.nextPostition;
                    }
                    //if(box.obb.axes)
                    break;
            }
        }
    }
}
