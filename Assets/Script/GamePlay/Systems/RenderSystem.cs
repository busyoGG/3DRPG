
using Game;
using System.Collections.Generic;
using UnityEngine;

public class RenderSystem : ECSSystem
{
    private Quaternion _rotation = Quaternion.identity;
    private Vector3 _forward = Vector3.zero;
    public override ECSMatcher Filter()
    {
        return ECSManager.Ins().AllOf(typeof(RenderComp), typeof(MoveComp));
    }

    public override void OnUpdate(List<Entity> entities)
    {
        foreach (var entity in entities)
        {
            RenderComp render = entity.Get<RenderComp>();
            MoveComp move = entity.Get<MoveComp>();
            Vector3 inputForward = InputSingleton.Ins().GetForward(entity.id);

            Transform transform = render.node.transform;

            //转向
            if (!inputForward.Equals(Vector3.zero))
            {
                Vector3 target = move.nextPostition;
                target.y = transform.position.y;
                //设置转向方向
                _forward.x = inputForward.x;
                _forward.z = inputForward.z;
                //转向
                if (!_forward.Equals(Vector3.zero))
                {
                    _rotation.SetLookRotation(_forward);
                    transform.rotation = _rotation;
                }
            }
            if (move.nextPostition != move.lastPosition)
            {
                transform.position = move.nextPostition;
            }
            //transform.position = calculatedPosition;
            //ConsoleUtils.Log("渲染坐标", nextPosition, move.nextPostition, moveY);

        }
    }
}

