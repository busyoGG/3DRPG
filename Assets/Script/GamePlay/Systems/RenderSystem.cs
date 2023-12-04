

using System.Collections.Generic;
using UnityEngine;

public class RenderSystem : ECSSystem
{
    public override ECSMatcher Filter()
    {
        return ECSManager.Ins().AllOf(typeof(RenderComp), typeof(TransformComp));
    }

    public override void OnUpdate(List<Entity> entities)
    {
        foreach (var entity in entities)
        {
            RenderComp render = entity.Get<RenderComp>();
            TransformComp transform = entity.Get<TransformComp>();

            Transform transformNode = render.node.transform;

            transformNode.rotation = transform.rotation;
            transformNode.position = transform.position;

        }
    }
}

