using System.Collections.Generic;
using UnityEngine;

public class ClimbUpSystem : ECSSystem
{
    public override ECSMatcher Filter()
    {
        return ECSManager.Ins().AllOf(typeof(ClimbUpComp), typeof(MoveComp),typeof(CollideComp),typeof(BoxComp));
    }

    public override void OnEnter(List<Entity> entities)
    {
        foreach (var entity in entities)
        {
            ConsoleUtils.Log("开始爬到顶");
            ClimbUpComp climbUp = entity.Get<ClimbUpComp>();
            MoveComp move = entity.Get<MoveComp>();
            BoxComp box = entity.Get<BoxComp>();
            //此处暂时硬编码为上升1.2高度
            climbUp.targetY = move.nextPostition.y + 1.2f;

            CollideComp collider = entity.Get<CollideComp>();
            Vector3 dir = collider.closestCenter - box.position;
            dir.y = 0;
            climbUp.climbOffset = dir.normalized;
        }
    }

    public override void OnUpdate(List<Entity> entities)
    {
        for (int i = 0; i < entities.Count; i++)
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
                if (climbUp.progress < 1f)
                {
                    move.nextPostition += climbUp.climbOffset * 0.2f;
                }
                else
                {
                    //移除组件
                    entity.Remove<ClimbUpComp>();
                }
                climbUp.progress += 0.2f;
            }
        }
    }

    public override void OnRemove(List<Entity> entities)
    {
        foreach (Entity entity in entities)
        {
            MoveComp move = entity.Get<MoveComp>();
            move.isClimb = false;
            //move.isClimbTop = 0;
        }
    }
}
