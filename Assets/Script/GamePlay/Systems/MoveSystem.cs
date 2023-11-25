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



            if (!inputForward.Equals(Vector3.zero))
            {
                if (move.isSlope && !move.isTop)
                {
                    //inputForward = move.forwardOffset * inputForward;
                    Quaternion rotation = Quaternion.identity;
                    rotation.SetFromToRotation(Vector3.up, move.forwardOffset);
                    inputForward = rotation * inputForward;
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
            Vector3 inputForward = InputSingleton.Ins().GetForward(entity.id);
            //if (move.isTop)
            //{
            //    //inputForward = move.forwardOffset * inputForward;
            //    inputForward = move.forwardOffset;
            //}
            if (move.isSlope)
            {
                inputForward = move.forwardOffset;
                Gizmos.DrawLine(move.lastPosition, move.lastPosition + inputForward * 5);

            }
        }
    }
}
