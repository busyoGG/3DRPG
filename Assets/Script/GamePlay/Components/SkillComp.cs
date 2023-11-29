using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SkillPlayStatus { 
    Idle,
    Charge,
    Play,
    End
}

[CompRegister(typeof(SkillComp))]
public class SkillComp : Comp
{
    public Dictionary<InputKey,int> id = new Dictionary<InputKey, int>();

    public SkillPlayStatus play;

    public InputKey key;

    public InputStatus status;

    public SkillTrigger trigger;

    public SkillData skill;

    public float duration;

    public override void Reset()
    {
        play = SkillPlayStatus.Idle;
        key = InputKey.None;
        skill = null;
        id.Clear();
        duration = 0;
        trigger = SkillTrigger.Click;
    }
}
