

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

            if (transform.changed)
            {
                transform.changed = false;
                transform.lastPosition = transformNode.position;
                transform.lastRotation = transformNode.rotation;
                render.curStep = 0;
            }

            //transformNode.rotation = transform.rotation;

            render.curStep += _dt;

            float ratio = render.curStep / render.totalStep;
            if (ratio > 1)
            {
                ratio = 1;
            }

            if (ratio <= 1)
            {
                transformNode.position = Vector3.Lerp(transform.lastPosition, transform.position, ratio);
                transformNode.rotation = Quaternion.Lerp(transform.lastRotation, transform.rotation, ratio);

                if (ratio == 1)
                {
                    render.curStep = 0;
                    transform.lastPosition = transform.position;
                    transform.lastRotation = transform.rotation;
                    transform.lastScale = transform.scale;
                }
            }
        }
    }
}

