using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class ClimbUpSystem : ECSSystem
{
    public override ECSMatcher Filter()
    {
        return ECSManager.Ins().AllOf(typeof(ClimbUpComp), typeof(MoveComp));
    }

    public override void OnEnter(List<Entity> entities)
    {
        foreach (var entity in entities)
        {
            ClimbUpComp climbUp = entity.Get<ClimbUpComp>();
            MoveComp move = entity.Get<MoveComp>();
            //�˴���ʱӲ����Ϊ����1�߶�
            climbUp.targetY = move.nextPostition.y + 1.2f;
            climbUp.climbOffset = move.climbOffset;
            //����Ϊ�Զ�����״̬
            move.isClimbTop = 2;
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
                    move.nextPostition += -climbUp.climbOffset * 0.2f;
                }
                else
                {
                    //�Ƴ����
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
            move.isClimbTop = 0;
        }
    }
}
