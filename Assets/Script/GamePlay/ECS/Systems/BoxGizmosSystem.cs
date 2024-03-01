using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxGizmosSystem : ECSSystem
{
    public override ECSMatcher Filter()
    {
        return ECSManager.Ins().AllOf(typeof(BoxComp));
    }

    public override void OnUpdate(List<Entity> entities)
    {

    }

    public override void OnDrawGizmos(List<Entity> entities)
    {
        foreach (var entity in entities)
        {
            BoxComp box = entity.Get<BoxComp>();

            Gizmos.color = Color.white;
            switch (box.type)
            {
                case CollisionType.AABB:
                    Gizmos.DrawWireCube(box.position, box.aabb.size);
                    break;
                case CollisionType.OBB:
                    Matrix4x4 matrix = Gizmos.matrix;
                    Matrix4x4 matrixDef = matrix;

                    matrix = Matrix4x4.TRS(box.position, box.obb.rot, Vector3.one);

                    Gizmos.matrix = matrix;
                    Gizmos.DrawWireCube(Vector3.zero, box.obb.size);


                    Gizmos.matrix = matrixDef;
                    break;
            }
        }
    }
}
