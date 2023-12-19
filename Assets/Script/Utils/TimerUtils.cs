using System;
using System.Threading;
using UnityEngine;

public class TimerUtils
{
    private static TimeWheel _timeWheel;
    private static Thread _thread;
    private static bool _isRunning = false;
    private static TimerScript _timerScript;

    public static void Init()
    {
        _timeWheel = new TimeWheel();
        _isRunning = true;
        _thread = new Thread(Update);
        _thread.Start();

        GameObject obj = new GameObject();
        obj.name = "TimerUtils";
        _timerScript = obj.AddComponent<TimerScript>();
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

    /// <summary>
    /// ͬ����ʱ��
    /// </summary>
    /// <param name="delay">��ʱ ��λ����</param>
    /// <param name="action">��Ϊ</param>
    /// <returns></returns>
    public static TimerChain Once(int delay, Action action)
    {
        return new TimerChain(_timeWheel).Once(delay, action);
    }

    /// <summary>
    /// ͬ��ѭ�� ��һ�δ���ʱ��Ϊ interval + delay
    /// </summary>
    /// <param name="interval">ѭ�����ʱ��</param>
    /// <param name="action">��Ϊ</param>
    /// <param name="delay">��ʱ</param>
    /// <param name="loopTimes">ѭ������ Ĭ��Ϊ-1 ����ѭ��</param>
    /// <returns></returns>
    public static TimerChain Loop(int interval, Action action, int delay = 0, int loopTimes = -1)
    {
        return new TimerChain(_timeWheel).Loop(interval, action, delay, loopTimes);
    }

    /// <summary>
    /// �첽��ʱ��
    /// </summary>
    /// <param name="delay">��ʱ ��λ����</param>
    /// <param name="action">��Ϊ</param>
    /// <returns></returns>
    public static TimerChain OnceAsync(int delay, Action action)
    {
        return new TimerChain(_timeWheel).OnceAsync(delay, action);
    }


    /// <summary>
    /// �첽ѭ�� ��һ�δ���ʱ��Ϊ interval + delay
    /// </summary>
    /// <param name="interval">ѭ�����ʱ��</param>
    /// <param name="action">��Ϊ</param>
    /// <param name="delay">��ʱ</param>
    /// <param name="loopTimes">ѭ������ Ĭ��Ϊ-1 ����ѭ��</param>
    /// <returns></returns>
    public static TimerChain LoopAsync(int interval, Action action, int delay = 0, int loopTimes = -1)
    {
        return new TimerChain(_timeWheel).LoopAsync(interval, action, delay, loopTimes);
    }

    public static TimerChain Clear(TimerChain chain)
    {
        chain.Clear();
        return chain;
    }

    public static void AddAction(int id, Action action)
    {
        _timerScript.AddAction(id, action);
    }
}
