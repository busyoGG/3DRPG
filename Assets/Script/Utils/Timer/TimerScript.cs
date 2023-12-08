using System;
using System.Collections.Generic;
using UnityEngine;

public class TimerScript : MonoBehaviour
{
    private Queue<Action> _actions = new Queue<Action>();

    void Update()
    {
        while (_actions.Count > 0)
        {
            Run(_actions.Dequeue());
        }
    }

    public void AddAction(Action action)
    {
        _actions.Enqueue(action);
    }

    private void Run(Action action)
    {
        action.Invoke();
    }
}
