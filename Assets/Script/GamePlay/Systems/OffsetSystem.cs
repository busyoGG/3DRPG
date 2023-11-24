using Game;
using System.Collections.Generic;
using UnityEngine;

public class OffsetSystem : ECSSystem
{
    public override ECSMatcher Filter()
    {
        return ECSManager.Ins().AllOf(typeof(MoveComp), typeof(CollideComp));
    }

    public override void OnUpdate(List<Entity> entities)
    {
        foreach (Entity entity in entities)
        {
            MoveComp move = entity.Get<MoveComp>();
            CollideComp collider = entity.Get<CollideComp>();

            if (!collider.totalOffset.Equals(Vector3.zero))
            {
                move.nextPostition += collider.totalOffset;
            }
            //ConsoleUtils.Log("Æ«ÒÆÁ¿", move.nextPostition,collider.totalOffset);
        }
    }
}
