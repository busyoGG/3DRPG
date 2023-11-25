using Game;
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
            List<CollideComp> colliders = CollisionSingleton.Ins().GetColliders(entity.id);
            Vector3 inputForward = InputSingleton.Ins().GetForward(entity.id);

            //��ײƫ��
            if (!collider.totalOffset.Equals(Vector3.zero))
            {
                move.nextPostition += collider.totalOffset;
            //ConsoleUtils.Log("ƫ����", move.nextPostition,collider.totalOffset);
            }

            if (move.isSlope)
            {
                ////��̨
                //Vector3 left = Vector3.Cross(inputForward, collider.totalOffset).normalized;
                //Vector3 slopeForward = Vector3.Cross( collider.totalOffset, left);
                //move.nextPostition += slopeForward;
                //ConsoleUtils.Log("�·���",left, collider.totalOffset,slopeForward);
            }
            else
            {
                ////��ײƫ��
                //if (!inputForward.Equals(Vector3.zero))
                //{
                //    move.nextPostition += collider.totalOffset;
                //}
                //����
                move.fallTime += _dt * 0.5f;
                move.nextPostition.y += -0.5f * move.gravity * move.fallTime * move.fallTime;

                if (move.nextPostition.y < 1)
                {
                    move.nextPostition.y = 1;
                    move.fallTime = 0f;
                }
            }
        }
    }
}
