using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AniSystem : ECSSystem
{
    public override ECSMatcher Filter()
    {
        return ECSManager.Ins().AllOf(typeof(AniComp), typeof(RenderComp));
    }

    public override void OnUpdate(List<Entity> entities)
    {
        foreach (Entity entity in entities)
        {
            AniComp ani = entity.Get<AniComp>();
            string curAni = AniSingleton.Ins().GetCurAni(entity.id);

            if (curAni != ani.lastAni)
            {
                ani.isChange = true;
            }

            if (ani.isChange)
            {
                ani.isChange = false;
                ani.lastAni = curAni;
                ani.animator.Play(curAni, 0, 0);
            }
        }
    }
}
