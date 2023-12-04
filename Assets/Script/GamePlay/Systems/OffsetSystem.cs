
using System.Collections.Generic;
using UnityEngine;

public class OffsetSystem : ECSSystem
{
    public override ECSMatcher Filter()
    {
        return ECSManager.Ins().AllOf(typeof(MoveComp), typeof(CollideComp));
    }

    public override void OnUpdate(List<Entity> entities)
    {
        foreach (Entity entity in entities)
        {
            MoveComp move = entity.Get<MoveComp>();
            CollideComp collider = entity.Get<CollideComp>();

            //��ײƫ��
            if (!collider.totalOffset.Equals(Vector3.zero))
            {
                if (move.isSlope)
                {
                    move.nextPostition += move.forwardOffsetQua * collider.totalOffset;
                }
                else
                {
                    move.nextPostition += collider.totalOffset;
                }
                //ConsoleUtils.Log("ƫ����", move.nextPostition,collider.totalOffset);
            }
        }
    }

    public override void OnDrawGizmos(List<Entity> entities)
    {
        foreach (Entity entity in entities)
        {
            CollideComp colider = entity.Get<CollideComp>();
            Gizmos.color = Color.red;
            Gizmos.DrawLine(colider.position, colider.position + colider.totalOffset * 100);
        }
    }
}
