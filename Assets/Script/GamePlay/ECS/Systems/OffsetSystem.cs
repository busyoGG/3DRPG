
using System.Collections.Generic;
using UnityEngine;

public class OffsetSystem : ECSSystem
{
    public override ECSMatcher Filter()
    {
        return ECSManager.Ins().AllOf(typeof(MoveComp), typeof(CollideComp),typeof(BoxComp));
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
                move.nextPostition += collider.totalOffset;
                //�����ų�
                collider.totalOffset = Vector3.zero;
            }
        }
    }

    public override void OnDrawGizmos(List<Entity> entities)
    {
        foreach (Entity entity in entities)
        {
            CollideComp colider = entity.Get<CollideComp>();
            BoxComp box = entity.Get<BoxComp>();
            Gizmos.color = Color.red;
            Gizmos.DrawLine(box.position, box.position + colider.totalOffset * 100);
        }
    }
}
