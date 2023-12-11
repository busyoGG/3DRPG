using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AnimationObj",menuName = "ScriptableObjectCreater/CreaetAnimationData")]
public class AnimationData : ScriptableObject
{
    public float frameDelta;
    public int frameCount;
    public bool isLoop;
    
    public SerializableDictionary<string, List<Trans>> transforms;
    //public SerializableDictionary<string, List<Vector3>> positions;
    //public SerializableDictionary<string, List<Vector3>> eulers;
    //public SerializableDictionary<string, List<Vector3>> scales;
}