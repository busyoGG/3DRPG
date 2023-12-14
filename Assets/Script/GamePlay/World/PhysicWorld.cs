using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicWorld : ECSWorld
{
    public override void SystemAdd()
    {
        Add(new FallSystem());
        Add(new BoxRefreshSystem());
        Add(new IntersectSystem());
        Add(new TriggerSystem());
        Add(new CollideSystem());
        Add(new BoxGizmosSystem());
        Add(new OffsetSystem());
    }
}
