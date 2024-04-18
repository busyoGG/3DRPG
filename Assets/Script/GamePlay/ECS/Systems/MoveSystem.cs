
using System.Collections.Generic;
using UnityEngine;

public class MoveSystem : ECSSystem
{

    public override ECSMatcher Filter()
    {
        return ECSManager.Ins().AllOf(typeof(TransformComp), typeof(MoveComp));
    }

    public override void OnUpdate(List<Entity> entities)
    {
        foreach (Entity entity in entities)
        {
            MoveComp move = entity.Get<MoveComp>();

            if (move.inputForward != Vector3.zero && !entity.Has<ClimbUpComp>() && (!move.isClimb || move.isClimb && move.climbJump == 0))
            {
                if (move.isClimb)
                {
                    move.curForwad = move.inputForward;
                }
                else
                {
                    move.curForwad = move.forward;
                    move.fixedForward = move.up * move.forward;
                }
                move.nextPostition += move.fixedForward * move.speed;
            }
        }
    }

    /// <summary>
    /// ¸¨ÖúÏß
    /// </summary>
    /// <param name="entities"></param>
    public override void OnDrawGizmos(List<Entity> entities)
    {
        foreach (Entity entity in entities)
        {
            MoveComp move = entity.Get<MoveComp>();
            TransformComp transform = entity.Get<TransformComp>();

            Gizmos.DrawLine(transform.position, transform.position + move.fixedForward * 100);
        }
    }
}
