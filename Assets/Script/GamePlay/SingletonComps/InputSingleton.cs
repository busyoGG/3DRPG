using System.Collections.Generic;
using UnityEngine;

public class InputSingleton : Singleton<InputSingleton>
{
    private Dictionary<int, Vector3> _forward = new Dictionary<int, Vector3>();

    private Vector3 _temp = Vector3.zero;

    public Vector3 GetForward(int id)
    {
        Vector3 forward;
        _forward.TryGetValue(id, out forward);
        return forward;
    }

    public void SetForward(int id, Vector3 forward)
    {
        if (_forward.ContainsKey(id))
        {
            _forward[id] = forward;
        }
        else
        {
            _forward.Add(id, forward);
        }
    }

    public void SetForward(int id, float x, float z)
    {
        _temp.x = x;
        _temp.z = z;
        if (_forward.ContainsKey(id))
        {
            _forward[id] = _temp;
        }
        else
        {
            _forward.Add(id, _temp);
        }
    }
}
