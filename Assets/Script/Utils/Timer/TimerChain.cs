using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerChain
{
    private TimeWheel _timeWheel;

    private int _id = -1;

    private int _delay = 0;

    public TimerChain(TimeWheel wheel)
    {
        _timeWheel = wheel;
    }

    public TimerChain Once(int delay, Action action)
    {
        _id = _timeWheel.SetTimeout(_id, delay + _delay, action);
        _delay = delay;
        return this;
    }

    public TimerChain Loop(int interval, Action action, int delay = 0, int loopTimes = -1)
    {
        _id = _timeWheel.SetInterval(_id, interval, action, delay + _delay, loopTimes);
        _delay = interval * loopTimes + delay;
        return this;
    }

    public TimerChain Clear()
    {
        _timeWheel.ClearInterval(_id, true);
        return this;
    }

    //public TimerChain Clear(int id)
    //{
    //    _timeWheel.ClearInterval(id);
    //    return this;
    //}

    public int GetId()
    {
        return _id;
    }
}
