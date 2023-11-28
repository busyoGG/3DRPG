using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class ClimbUpSystem : ECSSystem
{
    public override ECSMatcher Filter()
    {
        return ECSManager.Ins().AllOf(typeof(ClimbUpComp),typeof(MoveComp));
    }

    public override void OnEnter(List<Entity> entities)
    {
        foreach (var entity in entities)
        {
            ClimbUpComp climbUp = entity.Get<ClimbUpComp>();
            MoveComp move = entity.Get<MoveComp>();
            //此处暂时硬编码为上升1高度
            climbUp.targetY = move.nextPostition.y + 1;
        }
    }

    public override void OnUpdate(List<Entity> entities)
    {
        for(int i = 0; i < entities.Count; i++)
        {
            Entity entity = entities[i];
            ClimbUpComp climbUp = entity.Get<ClimbUpComp>();
            MoveComp move = entity.Get<MoveComp>();

            if (move.nextPostition.y < climbUp.targetY)
            {
                move.nextPostition.y += move.speed;
            }
            else
            {
                move.nextPostition += -move.forwardOffset;
                //移除组件
                entity.Remove<ClimbUpComp>();
            }
        }
    }

    public override void OnRemove(List<Entity> entities)
    {
        foreach(Entity entity in entities)
        {
            MoveComp move = entity.Get<MoveComp>();
            move.isClimbTop = 2;
            move.isClimb = false;
        }
    }
}
