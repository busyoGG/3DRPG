using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CompRegister(typeof(BuffComp))]
public class BuffComp : Comp
{
    public BuffType buffType;

    public BuffFilter filter;

    public int interval;

    public int duration;

    public int endTime;

    public int delay;

    public int value;

    public override void Reset()
    {
        buffType = BuffType.Damage;
        filter = BuffFilter.Directly;
        interval = 0;
        duration = 0;
        endTime = 0;
        delay = 0;
        value = 0;
    }
}
