using Game;
using System.Collections.Generic;
using UnityEngine;

public class TransformSingleton : Singleton<TransformSingleton>
{
    public float lower = 1f;

    private Dictionary<int, float> _curY = new Dictionary<int, float>();

    private Dictionary<int, bool> _freeFall = new Dictionary<int, bool>();

    private Dictionary<int, float> _moveY = new Dictionary<int, float>();

    private Dictionary<int,Vector3> _calculatedPosition = new Dictionary<int,Vector3>();

    public bool GetFreeFall(int id)
    {
        bool freeFall;
        _freeFall.TryGetValue(id, out freeFall);
        return freeFall;
    }

    public void SetFreeFall(int id, bool freeFall)
    {
        if (_freeFall.ContainsKey(id))
        {
            _freeFall[id] = freeFall;
        }
        else
        {
            _freeFall.Add(id, freeFall);
        }
    }

    public float GetMoveY(int id)
    {
        float moveY;
        _moveY.TryGetValue(id, out moveY);
        return moveY;
    }

    public void SetMoveY(int id, float moveY)
    {
        if(moveY < 0)
        {
            ConsoleUtils.Log(moveY);
        }
        if (_moveY.ContainsKey(id))
        {
            _moveY[id] = moveY;
        }
        else
        {
            _moveY.Add(id, moveY);
        }
    }

    public Vector3 GetCalculatedPosition(int id)
    {
        Vector3 vector3 = new Vector3();
        _calculatedPosition.TryGetValue(id, out vector3);
        return vector3;
    }

    public void SetCalculatedPosition(int id, Vector3 position)
    {
        if(_calculatedPosition.ContainsKey(id))
        {
            _calculatedPosition[id] = position;
        }
        else
        {
            _calculatedPosition.Add(id, position);
        }
    }
}
