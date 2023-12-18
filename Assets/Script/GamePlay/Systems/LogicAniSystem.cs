using System.Collections.Generic;
using UnityEngine;

public class LogicAniSystem : ECSSystem
{
    private struct BufferData
    {
        public List<List<AnimationData>> aniClips;
        public Dictionary<string, List<Vector3>> positions;
        public Dictionary<string, List<Vector3>> eulers;
        public Dictionary<string, List<Vector3>> scales;
    }

    public override ECSMatcher Filter()
    {
        return ECSManager.Ins().AllOf(typeof(LogicAniComp), typeof(TransformComp), typeof(BoxComp));
    }

    public override void OnUpdate(List<Entity> entities)
    {
        for (int i = 0; i < entities.Count; i++)
        {
            Entity entity = entities[i];

            TransformComp transform = entity.Get<TransformComp>();
            LogicAniComp logicAni = entity.Get<LogicAniComp>();
            BoxComp box = entity.Get<BoxComp>();

            string curAni = AniSingleton.Ins().GetCurAni(logicAni.root.id);
            bool isForce = AniSingleton.Ins().GetForceLogic(logicAni.root.id);

            if (curAni != logicAni.lastAni)
            {
                logicAni.isChange = true;
            }

            if (logicAni.isChange || isForce)
            {
                AniSingleton.Ins().SetForceLogic(logicAni.root.id, false);
                logicAni.isChange = false;
                logicAni.lastAni = curAni;
                logicAni.frame = 0;
            }

            Play(logicAni, box, curAni);

            //±£´æµ±Ç°transform
            transform.position = box.obb.position;
            transform.rotation = box.obb.rot;
            transform.scale = box.obb.size;
        }
    }

    private void Play(LogicAniComp logicAni, BoxComp box, string curAni)
    {
        List<List<Vector3>> vec3 = logicAni.aniClips[curAni];
        //int keyframe = logicAni.frame % logicAni.aniClips[curAni][0].Count;
        int keyframe = logicAni.frame;

        if (keyframe > logicAni.aniClips[curAni][0].Count - 1) return;

        TransformComp rootTransform = logicAni.root.Get<TransformComp>();

        OBBData obb = box.obb;
        Vector3[] axes = obb.axes;

        Quaternion qua = rootTransform.rotation * Quaternion.Euler(vec3[1][keyframe]);

        axes[0] = qua * Vector3.right;
        axes[1] = qua * Vector3.up;
        axes[2] = qua * Vector3.forward;

        obb.axes = axes;

        obb.position = rootTransform.position + rootTransform.rotation * vec3[0][keyframe];

        obb.size = vec3[2][keyframe];

        logicAni.frame++;

        if (logicAni.isLoop[curAni])
        {
            if (logicAni.frame > logicAni.aniClips[curAni][0].Count - 1)
            {
                logicAni.frame = 0;
            }
        }
    }

    public override void OnDrawGizmos(List<Entity> entities)
    {
        foreach (var entity in entities)
        {
            //LogicAniComp logicAni = entity.Get<LogicAniComp>();
            TransformComp transform = entity.Get<TransformComp>();

            Gizmos.color = Color.blue;

            Matrix4x4 matrix = Gizmos.matrix;
            Matrix4x4 matrixDef = matrix;

            matrix = Matrix4x4.TRS(transform.position, transform.rotation, Vector3.one);

            Gizmos.matrix = matrix;
            Gizmos.DrawWireCube(Vector3.zero, transform.scale);

            Gizmos.matrix = matrixDef;
        }
    }
}
