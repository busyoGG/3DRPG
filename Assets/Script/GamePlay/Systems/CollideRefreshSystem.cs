using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollideRefreshSystem : ECSSystem
{
    public override ECSMatcher Filter()
    {
        return ECSManager.Ins().AllOf(typeof(CollideComp), typeof(MoveComp));
    }

    public override void OnUpdate(List<Entity> entities)
    {
        foreach (var entity in entities)
        {
            CollideComp collider = entity.Get<CollideComp>();
            MoveComp move = entity.Get<MoveComp>();

            //¸üÐÂ°üÎ§ºÐ
            collider.position = move.nextPostition;
            collider.totalOffset = Vector3.zero;
            move.forwardOffset = Vector3.zero;

            switch (collider.type)
            {
                case CollisionType.AABB:
                    if (collider.aabb.position != move.nextPostition)
                    {
                        collider.aabb.position = move.nextPostition;
                    }
                    break;
                case CollisionType.OBB:
                    if (collider.obb.position != move.nextPostition)
                    {
                        collider.obb.position = move.nextPostition;
                    }
                    //if(collider.obb.axes)
                    break;
            }
        }
    }
}
