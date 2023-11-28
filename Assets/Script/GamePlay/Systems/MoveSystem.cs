using Game;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Xml.Schema;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class MoveSystem : ECSSystem
{
    private Quaternion _up = Quaternion.identity;

    private Vector3 temp = Vector3.zero;

    public override void Init()
    {
        _up.SetLookRotation(-Vector3.up);
    }

    public override ECSMatcher Filter()
    {
        return ECSManager.Ins().AnyOf(typeof(MoveComp));
    }

    public override void OnUpdate(List<Entity> entities)
    {
        foreach (Entity entity in entities)
        {
            MoveComp move = entity.Get<MoveComp>();
            Vector3 inputForward = InputSingleton.Ins().GetForward(entity.id);


            if (move.isClimbTop == 0 && !inputForward.Equals(Vector3.zero))
            {
                if (move.isSlope && !move.isTop)
                {
                    inputForward = move.forwardOffsetQua * inputForward;
                }
                else if (move.isClimb)
                {
                    if (move.forwardOffset != Vector3.zero)
                    {
                        Vector3 originForward = InputSingleton.Ins().GetOriginForward(entity.id);

                        Quaternion climbRot = Quaternion.identity;
                        climbRot.SetLookRotation(-move.forwardOffset);

                        originForward = climbRot * originForward;

                        inputForward = move.forwardOffsetQua * originForward;
                    }
                }

                move.lastPosition = move.nextPostition;
                move.nextPostition = move.lastPosition + inputForward * move.speed;

                move.isMove = true;
            }
            else
            {
                move.isMove = false;
            }
        }
    }

    public override void OnDrawGizmos(List<Entity> entities)
    {
        foreach (Entity entity in entities)
        {
            MoveComp move = entity.Get<MoveComp>();

            Gizmos.color = Color.black;

            Vector3 inputForward = InputSingleton.Ins().GetForward(entity.id);
            if (move.isSlope && !move.isTop)
            {
                Quaternion rotation = Quaternion.identity;
                rotation.SetFromToRotation(Vector3.up, move.forwardOffset);
                inputForward = rotation * inputForward;
            }
            Gizmos.DrawLine(move.lastPosition, move.lastPosition + inputForward * 100);

            if (!move.isClimb) continue;
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(move.lastPosition, move.lastPosition + move.forwardOffset * 10);
        }
    }
}
