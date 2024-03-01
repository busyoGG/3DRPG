using Bean;
using System.Collections.Generic;

[CompRegister(typeof(SkillComp))]
public class SkillComp : Comp
{
    public Dictionary<InputKey,int> id = new Dictionary<InputKey, int>();

    public SkillPlayStatus play;

    public InputKey key;

    public InputStatus status;

    public SkillTrigger trigger;

    public SkillConfigData skill;

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
