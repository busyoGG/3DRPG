using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBarSystem : ECSSystem
{
    public override ECSMatcher Filter()
    {
        return ECSManager.Ins().AllOf(typeof(HealthBarSystem));
    }

    public override void OnUpdate(List<Entity> entities)
    {
        foreach (Entity entity in entities)
        {

        }
    }
}
