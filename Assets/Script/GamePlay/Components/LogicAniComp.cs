using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CompRegister(typeof(LogicAniComp))]
public class LogicAniComp : Comp
{
    public string curAni;

    public string lastAni;

    public float speed;

    public bool isChange;

    public int frame;

    //public string path;

    public Dictionary<string, Dictionary<string, List<List<Vector3>>>> aniClips;

    //public List<AnimationData> aniClips;

    public List<(string, OBBData)> aniBox;
    public override void Reset()
    {
        //path = "";
        aniClips = new Dictionary<string, Dictionary<string, List<List<Vector3>>>>();
        curAni = "Idle";
        lastAni = "Idle";
        speed = 1;
        isChange = true;
        aniBox = new List<(string, OBBData)>();
    }
}
