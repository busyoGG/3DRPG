using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class TimerTask
{
    public int id;

    public int delay;

    public int interval;

    public int loopTimes;

    public Action action;

    public DateTime dateTime;

    public TimerTask(int id, int interval, Action action, int loopTimes, int delay)
    {
        this.id = id;
        this.interval = interval;
        this.action = action;
        this.loopTimes = loopTimes;
        this.delay = delay;
        dateTime = DateTime.Now.AddMilliseconds(interval + delay);
    }

    public void Run()
    {
        action.Invoke();
    }

    public async Task RunAsync()
    {
        await Task.Run(() =>
        {
            action.Invoke();
        });
    }

    public bool CheckLoop()
    {
        if (loopTimes < 0)
        {
            dateTime = DateTime.Now.AddMilliseconds(interval);
            return true;
        }
        else
        {
            loopTimes--;
            dateTime = DateTime.Now.AddMilliseconds(interval);
            return loopTimes > 0;
        }
    }
}
