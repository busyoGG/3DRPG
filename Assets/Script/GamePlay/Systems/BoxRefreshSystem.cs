using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxRefreshSystem : ECSSystem
{
    public override ECSMatcher Filter()
    {
        return ECSManager.Ins().AllOf(typeof(BoxComp), typeof(TransformComp));
    }

    public override void OnUpdate(List<Entity> entities)
    {
        foreach (var entity in entities)
        {
            BoxComp box = entity.Get<BoxComp>();
            MoveComp move = entity.Get<MoveComp>();
            TransformComp transform = entity.Get<TransformComp>();

            //¸üÐÂ°üÎ§ºÐ
            if (move != null)
            {
                box.position = move.nextPostition;

                switch (box.type)
                {
                    case CollisionType.AABB:
                        if (box.aabb.position != move.nextPostition)
                        {
                            box.aabb.position = move.nextPostition;
                        }

                        if (box.aabb.size != transform.scale)
                        {
                            box.aabb.size = transform.scale;
                        }
                        break;
                    case CollisionType.OBB:
                        if (box.obb.position != move.nextPostition)
                        {
                            box.obb.position = move.nextPostition;
                        }

                        if (box.obb.rot != transform.rotation)
                        {
                            box.obb.rot = transform.rotation;
                        }

                        if (box.obb.size != transform.scale)
                        {
                            box.obb.size = transform.scale;
                        }

                        //if(box.obb.rot != move.)
                        //if(box.obb.axes)
                        break;
                }
            }
            else
            {
                box.position = transform.position;

                switch (box.type)
                {
                    case CollisionType.AABB:
                        if (box.aabb.position != transform.position)
                        {
                            box.aabb.position = transform.position;
                        }

                        if(box.aabb.size != transform.scale)
                        {
                            box.aabb.size = transform.scale;
                        }
                        break;
                    case CollisionType.OBB:
                        if (box.obb.position != transform.position)
                        {
                            box.obb.position = transform.position;
                        }

                        if (box.obb.rot != transform.rotation)
                        {
                            box.obb.rot = transform.rotation;
                        }

                        if(box.obb.size != transform.scale)
                        {
                            box.obb.size = transform.scale;
                        }
                        break;
                }
            }

        }
    }
}
