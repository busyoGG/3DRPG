using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestWorld : ECSWorld
{
    public override void SystemAdd()
    {
        Add(new TransformSystem());
        Add(new ClimbSystem());
        Add(new MoveSystem());
        Add(new JumpSystem());
        Add(new ClimbUpSystem());
        Add(new TriggerSystem());
        Add(new OffsetSystem());
        Add(new QTreeSystem());
        Add(new CollideGizmosSystem());
        Add(new SkillSystem());
        Add(new AniSystem());
        Add(new RenderSystem());
    }
}
