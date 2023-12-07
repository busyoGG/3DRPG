using System;
using System.Threading;

public class TimerUtils
{
    private static TimeWheel _timeWheel;
    private static Thread _thread;
    private static bool _isRunning = false;

    public static void Init()
    {
        _timeWheel = new TimeWheel();
        _isRunning = true;
        _thread = new Thread(Update);
        _thread.Start();
    }

    public static void Stop()
    {
        _isRunning = false;
        //_thread.Abort();
    }

    private static void Update()
    {
        while (_isRunning)
        {
            _timeWheel.Update();
        }
    }

    public static TimerChain Once(int delay, Action action)
    {
        return new TimerChain(_timeWheel).Once(delay, action);
    }

    public static TimerChain Loop(int interval, Action action, int delay = 0, int loopTimes = -1)
    {
        return new TimerChain(_timeWheel).Loop(interval, action, delay, loopTimes);
    }

    public static TimerChain Clear(TimerChain chain)
    {
        chain.Clear();
        return chain;
    }
}
