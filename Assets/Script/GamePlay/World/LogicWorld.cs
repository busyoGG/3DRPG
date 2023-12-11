using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogicWorld : ECSWorld
{
    public override void SystemAdd()
    {
        Add(new QTreeSystem());
        Add(new QObjFindingSystem());
        Add(new TransformSystem());
        Add(new ClimbSystem());
        Add(new MoveSystem());
        Add(new JumpSystem());
        Add(new ClimbUpSystem());
        Add(new WeaponSystem());
        Add(new TriggerSystem());
        Add(new LogicAniSystem());
        Add(new AniSystem());
    }
}
