using System.Collections.Generic;
using UnityEngine;

public class InputSingleton : Singleton<InputSingleton>
{
    private Dictionary<int, Vector3> _forward = new Dictionary<int, Vector3>();

    private Vector3 _temp = Vector3.zero;

    private Dictionary<int, Quaternion> _rot = new Dictionary<int, Quaternion>();

    private Dictionary<int, Vector3> _originForward = new Dictionary<int, Vector3>();

    public Vector3 GetForward(int id)
    {
        Vector3 forward;
        _forward.TryGetValue(id, out forward);
        return forward;
    }

    public Vector3 GetOriginForward(int id)
    {
        Vector3 forward;
        _originForward.TryGetValue(id, out forward);
        return forward;
    }

    public void SetForward(int id, Vector3 forward, Quaternion rot)
    {
        if (_originForward.ContainsKey(id))
        {
            _originForward[id] = forward;
        }
        else
        {
            _originForward.Add(id, forward);
        }

        if (_rot.ContainsKey(id))
        {
            _rot[id] = rot;
        }
        else
        {
            _rot.Add(id, rot);
        }

        Vector3 realForward = rot * forward;

        if (_forward.ContainsKey(id))
        {
            _forward[id] = realForward;
        }
        else
        {
            _forward.Add(id, realForward);
        }
    }

    public void SetForward(int id, float x, float z,Quaternion rot)
    {
        _temp.x = x;
        _temp.z = z;

        if (_originForward.ContainsKey(id))
        {
            _originForward[id] = _temp;
        }
        else
        {
            _originForward.Add(id, _temp);
        }

        if (_rot.ContainsKey(id))
        {
            _rot[id] = rot;
        }
        else
        {
            _rot.Add(id, rot);
        }

        Vector3 forward = rot * _temp;

        if (_forward.ContainsKey(id))
        {
            _forward[id] = forward;
        }
        else
        {
            _forward.Add(id, forward);
        }
    }
}
