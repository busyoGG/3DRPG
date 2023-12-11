using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformSystem : ECSSystem
{
    private Quaternion _rotation = Quaternion.identity;
    private Vector3 _forward = Vector3.zero;
    public override ECSMatcher Filter()
    {
        return ECSManager.Ins().AllOf(typeof(TransformComp), typeof(MoveComp));
    }

    public override void OnUpdate(List<Entity> entities)
    {
        foreach (var entity in entities)
        {
            TransformComp transform = entity.Get<TransformComp>();
            MoveComp move = entity.Get<MoveComp>();

            Vector3 inputForward = InputSingleton.Ins().GetForward(entity.id);

            if (transform.position != move.nextPostition)
            {
                transform.changed = true;
                transform.position = move.nextPostition;
            }

            //转向
            if (!inputForward.Equals(Vector3.zero))
            {
                Vector3 target = transform.position;
                target.y = transform.position.y;
                //设置转向方向
                _forward.x = inputForward.x;
                _forward.z = inputForward.z;
                //转向
                if (_forward.x != 0 && _forward.z != 0)
                {
                    _rotation.SetLookRotation(_forward);
                    transform.rotation = _rotation;
                }
                transform.changed = true;
            }
        }
    }
}
