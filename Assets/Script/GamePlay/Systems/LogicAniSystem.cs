using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEditor.AnimatedValues;
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
        return ECSManager.Ins().AllOf(typeof(LogicAniComp), typeof(TransformComp));
    }

    public override void OnUpdate(List<Entity> entities)
    {
        for (int i = 0; i < entities.Count; i++)
        {
            Entity entity = entities[i];

            TransformComp transform = entity.Get<TransformComp>();
            LogicAniComp logicAni = entity.Get<LogicAniComp>();

            if (logicAni.curAni != logicAni.lastAni)
            {
                logicAni.isChange = true;
            }

            if (logicAni.isChange)
            {
                logicAni.isChange = false;
                logicAni.lastAni = logicAni.curAni;
                logicAni.frame = 0;
            }

            Play(logicAni, transform);
        }
    }

    private void Play(LogicAniComp logicAni,TransformComp transform)
    {
        Dictionary<string, List<List<Vector3>>> aniClip;
        logicAni.aniClips.TryGetValue(logicAni.curAni, out aniClip);
        if (aniClip == null) return;

        Quaternion tranQ = Quaternion.identity;

        foreach (var data in logicAni.aniBox)
        {
            string path = data.Item1;
            List<List<Vector3>> vec3;
            aniClip.TryGetValue(path, out vec3);

            if (vec3 == null) continue;

            int keyframe = logicAni.frame % aniClip[path].Count;

            OBBData obb = data.Item2;
            Vector3[] axes = obb.axes;


            Quaternion qua = transform.rotation * Quaternion.Euler(vec3[keyframe][1]);

            axes[0] = qua * Vector3.right;
            axes[1] = qua * Vector3.up;
            axes[2] = qua * Vector3.forward;

            obb.axes = axes;

            obb.position = transform.position + qua * vec3[keyframe][0];

            //obb.size = vec3[keyframe][2];
        }
        logicAni.frame++;
    }

    public override void OnDrawGizmos(List<Entity> entities)
    {
        foreach (var entity in entities)
        {
            LogicAniComp logicAni = entity.Get<LogicAniComp>();

            Gizmos.color = Color.blue;

            for (int i = 0; i < logicAni.aniBox.Count; i++)
            {
                Matrix4x4 matrix = Gizmos.matrix;
                Matrix4x4 matrixDef = matrix;

                matrix = Matrix4x4.TRS(logicAni.aniBox[i].Item2.position, logicAni.aniBox[i].Item2.rot, Vector3.one);

                Gizmos.matrix = matrix;
                Gizmos.DrawWireCube(Vector3.zero, logicAni.aniBox[i].Item2.size);

                Gizmos.matrix = matrixDef;
            }

        }
    }
}
