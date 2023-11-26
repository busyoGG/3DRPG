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
}
