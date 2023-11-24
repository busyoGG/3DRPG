using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestWorld : ECSWorld
{
    public override void Init()
    {
        Add(new JumpSystem());
        Add(new MoveSystem());
        Add(new QTreeSystem());
        Add(new CollideSystem());
        Add(new RenderSystem());
    }
}
