using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformSystem : ECSSystem
{
    public override ECSMatcher Filter()
    {
        return ECSManager.Ins().AllOf(typeof(TransformComp),typeof(MoveComp));
    }

    public override void OnUpdate(List<Entity> entities)
    {
        foreach (var entity in entities)
        {
            TransformComp transform = entity.Get<TransformComp>();
            MoveComp move = entity.Get<MoveComp>();

            transform.position = move.nextPostition;
        }
    }
}
