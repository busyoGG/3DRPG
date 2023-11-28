using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestWorld : ECSWorld
{
    public override void SystemAdd()
    {
        Add(new ClimbSystem());
        Add(new MoveSystem());
        Add(new JumpSystem());
        Add(new ClimbUpSystem());
        Add(new FallSystem());
        Add(new QObjFindingSystem());
        Add(new CollideRefreshSystem());
        Add(new CollideSystem());
        Add(new TriggerSystem());
        Add(new OffsetSystem());
        Add(new QTreeSystem());
        Add(new RenderSystem());
        Add(new CollideGizmosSystem());
    }
}
