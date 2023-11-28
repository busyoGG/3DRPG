using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollideGizmosSystem : ECSSystem
{
    public override ECSMatcher Filter()
    {
        return ECSManager.Ins().AllOf(typeof(CollideComp));
    }

    public override void OnUpdate(List<Entity> entities)
    {

    }

    public override void OnDrawGizmos(List<Entity> entities)
    {
        foreach (var entity in entities)
        {
            CollideComp collider = entity.Get<CollideComp>();

            Gizmos.color = Color.white;
            switch (collider.type)
            {
                case CollisionType.AABB:
                    Gizmos.DrawWireCube(collider.position, collider.aabb.size);
                    break;
                case CollisionType.OBB:
                    Matrix4x4 matrix = Gizmos.matrix;
                    Matrix4x4 matrixDef = matrix;

                    matrix = Matrix4x4.TRS(collider.position, collider.obb.rot, Vector3.one);

                    Gizmos.matrix = matrix;
                    Gizmos.DrawWireCube(Vector3.zero, collider.obb.size);


                    Gizmos.matrix = matrixDef;
                    break;
            }
        }
    }
}
