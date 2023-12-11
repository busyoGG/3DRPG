using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicWorld : ECSWorld
{
    public override void SystemAdd()
    {
        Add(new FallSystem());
        Add(new CollideRefreshSystem());
        Add(new CollideSystem());
        Add(new CollideGizmosSystem());
        Add(new OffsetSystem());
    }
}
