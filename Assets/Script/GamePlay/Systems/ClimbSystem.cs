
using System.Collections.Generic;
using UnityEngine;

public class ClimbSystem : ECSSystem
{
    public override ECSMatcher Filter()
    {
        return ECSManager.Ins().AllOf(typeof(ClimbComp));
    }

    public override void OnUpdate(List<Entity> entities)
    {
        foreach (var entity in entities)
        {
            ClimbComp climb = entity.Get<ClimbComp>();

            Vector3 inputForward = InputSingleton.Ins().GetForward(entity.id);

            if (inputForward.x != 0 || inputForward.z != 0)
            {
                List<CollideComp> colliders = CollisionSingleton.Ins().GetColliders(entity.id);

                bool isCollide = colliders != null && colliders.Count > 0;

                if (isCollide)
                {
                    if (!climb.isClimb)
                    {
                        climb.enterTime += _dt;
                        if (climb.enterTime > 0.5f)
                        {
                            climb.isClimb = true;
                            ClimbSingleton.Ins().SetClimbState(entity.id, climb.isClimb);
                        }
                    }
                    else
                    {
                        //≈ ≈¿¬ﬂº≠
                        
                    }
                }
            }
        }
    }
}
