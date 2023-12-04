using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using static AnimationData;

public class ConvertAnimationDataEditor : EditorWindow
{
    private static EditorWindow window;

    [MenuItem("PreUtils/ConverAnimationData")]
    static void Execute()
    {
        if (window == null)
            window = (ConvertAnimationDataEditor)GetWindow(typeof(ConvertAnimationDataEditor));
        window.minSize = new Vector2(300, 200);
        window.name = "动画转换工具";
        window.Show();
    }

    private AnimationClip clip;
    private AnimationData animAsset;

    private void OnGUI()
    {
        using (new GUILayout.HorizontalScope("box"))
        {
            GUILayout.Label("AnimClip:", GUILayout.Width(60f));
            clip = EditorGUILayout.ObjectField(clip, typeof(AnimationClip), false) as AnimationClip;
        }

        using (new GUILayout.HorizontalScope())
        {
            GUILayout.Label("SaveAsset:", GUILayout.Width(60f));
            animAsset = EditorGUILayout.ObjectField(animAsset, typeof(AnimationData), false) as AnimationData;
        }

        if (GUILayout.Button("Save"))
        {
            Save();
        }

    }

    private void Save()
    {
        var path = AssetDatabase.GetAssetPath(animAsset);
        var asset = AssetDatabase.LoadAssetAtPath<AnimationData>(path);

        asset.frameDelta = 1f / 60f;
        asset.frameCount = Mathf.CeilToInt(clip.length / asset.frameDelta) + 1;
        //asset.positions = new SerializableDictionary<string, List<Vector3>>();
        //asset.eulers = new SerializableDictionary<string, List<Vector3>>();
        //asset.scales = new SerializableDictionary<string, List<Vector3>>();
        //for (int i = 0; i < asset.frameCount; ++i)
        //{
        //    asset.positions.Add(Vector3.zero);
        //    asset.scales.Add(Vector3.one);
        //    asset.eulers.Add(Vector3.zero);
        //}
        asset.transforms = new SerializableDictionary<string, List<Trans>>();
        foreach (var binding in AnimationUtility.GetCurveBindings(clip))
        {
            if (!asset.transforms.ContainsKey(binding.path))
            {
                List<Trans> transList = new List<Trans>();
                for (int i = 0; i < asset.frameCount; ++i)
                {
                    Trans trans = new Trans();
                    trans.position = Vector3.zero;
                    trans.euler = Vector3.zero;
                    trans.scale = Vector3.one;
                    transList.Add(trans);
                }
                asset.transforms.Add(binding.path, transList);
            }

            AnimationCurve curve = AnimationUtility.GetEditorCurve(clip, binding);

            string propName = binding.propertyName;

            float timer = 0;
            float maxTime = clip.length;
            int index = 0;

            while (index < asset.frameCount - 1)
            {
                var trans = asset.transforms[binding.path][index];
                switch (propName)
                {
                    case "m_LocalPosition.x":
                        {
                            trans.position.x = GetValue(curve.keys, timer);
                            asset.transforms[binding.path][index] = trans;

                        }
                        break;
                    case "m_LocalPosition.y":
                        {

                            trans.position.y = GetValue(curve.keys, timer);
                            asset.transforms[binding.path][index] = trans;
                        }
                        break;
                    case "m_LocalPosition.z":
                        {

                            trans.position.z = GetValue(curve.keys, timer);
                            asset.transforms[binding.path][index] = trans;
                        }
                        break;
                    case "m_LocalScale.x":
                        {
                            trans.scale.x = GetValue(curve.keys, timer);
                            asset.transforms[binding.path][index] = trans;
                        }
                        break;
                    case "m_LocalScale.y":
                        {
                            trans.scale.y = GetValue(curve.keys, timer);
                            asset.transforms[binding.path][index] = trans;
                        }
                        break;
                    case "m_LocalScale.z":
                        {
                            trans.scale.z = GetValue(curve.keys, timer);
                            asset.transforms[binding.path][index] = trans;
                        }
                        break;
                    case "localEulerAnglesRaw.x":
                        {
                            trans.euler.x = GetValue(curve.keys, timer);
                            asset.transforms[binding.path][index] = trans;
                        }
                        break;
                    case "localEulerAnglesRaw.y":
                        {
                            trans.euler.y = GetValue(curve.keys, timer);
                            asset.transforms[binding.path][index] = trans;
                        }
                        break;
                    case "localEulerAnglesRaw.z":
                        {
                            trans.euler.z = GetValue(curve.keys, timer);
                            asset.transforms[binding.path][index] = trans;
                        }
                        break;
                }

                timer += asset.frameDelta;
                index++;
            }

            var finalTrans = asset.transforms[binding.path][asset.frameCount - 1];
            switch (propName)
            {
                case "m_LocalPosition.x":
                    {
                        finalTrans.position.x = GetValue(curve.keys, timer);
                        asset.transforms[binding.path][index] = finalTrans;

                    }
                    break;
                case "m_LocalPosition.y":
                    {

                        finalTrans.position.y = GetValue(curve.keys, timer);
                        asset.transforms[binding.path][index] = finalTrans;
                    }
                    break;
                case "m_LocalPosition.z":
                    {

                        finalTrans.position.z = GetValue(curve.keys, timer);
                        asset.transforms[binding.path][index] = finalTrans;
                    }
                    break;
                case "m_LocalScale.x":
                    {
                        finalTrans.scale.x = GetValue(curve.keys, timer);
                        asset.transforms[binding.path][index] = finalTrans;
                    }
                    break;
                case "m_LocalScale.y":
                    {
                        finalTrans.scale.y = GetValue(curve.keys, timer);
                        asset.transforms[binding.path][index] = finalTrans;
                    }
                    break;
                case "m_LocalScale.z":
                    {
                        finalTrans.scale.z = GetValue(curve.keys, timer);
                        asset.transforms[binding.path][index] = finalTrans;
                    }
                    break;
                case "localEulerAnglesRaw.x":
                    {
                        finalTrans.euler.x = GetValue(curve.keys, timer);
                        asset.transforms[binding.path][index] = finalTrans;
                    }
                    break;
                case "localEulerAnglesRaw.y":
                    {
                        finalTrans.euler.y = GetValue(curve.keys, timer);
                        asset.transforms[binding.path][index] = finalTrans;
                    }
                    break;
                case "localEulerAnglesRaw.z":
                    {
                        finalTrans.euler.z = GetValue(curve.keys, timer);
                        asset.transforms[binding.path][index] = finalTrans;
                    }
                    break;
            }
        }

        EditorUtility.SetDirty(asset);
        AssetDatabase.SaveAssets();
    }

    private float GetValue(Keyframe[] frames, float time)
    {
        int pre = 0;
        int next = 0;
        for (int i = 0; i < frames.Length; ++i)
        {
            var frame = frames[i];
            //if (time <= frame.time)
            //{
            //    next = i;
            //    break;
            //}
            if (time >= frame.time)
            {
                next = i + 1;
            }
        }
        //pre = Mathf.Max(0, next - 1);
        pre = next - 1;
        next = Mathf.Min(frames.Length - 1, next);

        var preFrame = frames[pre];
        var nextFrame = frames[next];

        float ratio = time - preFrame.time;
        float total = nextFrame.time - preFrame.time;
        if (ratio > total)
        {
            ratio = total;
        }

        float ret;
        if (total == 0)
        {
            ret = preFrame.value;
        }
        else
        {
            ret = preFrame.value + (nextFrame.value - preFrame.value) * ratio / total;
        }
        return ret;
    }

}