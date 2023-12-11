using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AniSingleton : Singleton<AniSingleton>
{
    private Dictionary<int, string> _curAni = new Dictionary<int, string>();

    private Dictionary<int, bool> _forceAni = new Dictionary<int, bool>();

    private Dictionary<int, bool> _forceAniLogic = new Dictionary<int, bool>();

    public string GetCurAni(int id)
    {
        string ani;
        _curAni.TryGetValue(id, out ani);
        return ani;
    }

    public bool GetForce(int id)
    {
        bool force;
        _forceAni.TryGetValue(id, out force);
        return force;
    }

    public void SetForce(int id,bool force)
    {
        if(_forceAni.ContainsKey(id))
        {
            _forceAni[id] = force;
        }
        else
        {
            _forceAni.Add(id, force);
        }
    }

    public bool GetForceLogic(int id)
    {
        bool force;
        _forceAniLogic.TryGetValue(id, out force);
        return force;
    }


    public void SetForceLogic(int id,bool force)
    {
        if(_forceAniLogic.ContainsKey(id))
        {
            _forceAniLogic[id] = force;
        }
        else
        {
            _forceAniLogic.Add(id, force);
        }
    }

    public void SetCurAni(int id, string ani,bool force = false)
    {
        if (_curAni.ContainsKey(id))
        {
            _curAni[id] = ani;
        }
        else
        {
            _curAni.Add(id, ani);
        }

        SetForce(id, force);

        SetForceLogic(id, force);
    }
}
