
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Xml.Schema;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class MoveSystem : ECSSystem
{
    private Quaternion _rotation = Quaternion.identity;
    private Vector3 _forward = Vector3.zero;

    public override ECSMatcher Filter()
    {
        return ECSManager.Ins().AllOf(typeof(TransformComp), typeof(MoveComp));
    }

    public override void OnUpdate(List<Entity> entities)
    {
        foreach (Entity entity in entities)
        {
            MoveComp move = entity.Get<MoveComp>();
            TransformComp transform = entity.Get<TransformComp>();

            Vector3 inputForward = InputSingleton.Ins().GetForward(entity.id);

            if (move.isClimbTop == 0 && !inputForward.Equals(Vector3.zero))
            {
                if (move.isSlope && !move.isTop)
                {
                    inputForward = move.forwardOffsetQua * inputForward;
                }
                else if (move.isClimb)
                {
                    if (move.climbOffset != Vector3.zero)
                    {
                        Vector3 originForward = InputSingleton.Ins().GetOriginForward(entity.id);

                        Quaternion climbRot = Quaternion.identity;
                        climbRot.SetLookRotation(-move.climbOffset);

                        originForward = climbRot * originForward;

                        inputForward = move.climbOffsetQua * originForward;
                    }
                }

                transform.position = move.nextPostition;
                move.nextPostition = transform.position + inputForward * move.speed;

                move.isMove = true;

                //ת��
                if (!inputForward.Equals(Vector3.zero))
                {
                    Vector3 target = transform.position;
                    target.y = transform.position.y;
                    //����ת����
                    _forward.x = inputForward.x;
                    _forward.z = inputForward.z;
                    //ת��
                    if (!_forward.Equals(Vector3.zero))
                    {
                        _rotation.SetLookRotation(_forward);
                        transform.rotation = _rotation;
                    }
                }
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
            TransformComp transform = entity.Get<TransformComp>();

            Gizmos.color = Color.black;

            Vector3 inputForward = InputSingleton.Ins().GetForward(entity.id);
            if (move.isSlope && !move.isTop)
            {
                Quaternion rotation = Quaternion.identity;
                rotation.SetFromToRotation(Vector3.up, move.forwardOffset);
                inputForward = rotation * inputForward;
            }
            Gizmos.DrawLine(transform.position, transform.position + inputForward * 100);

            if (!move.isClimb) continue;
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(transform.position, transform.position + move.forwardOffset * 10);
        }
    }
}
