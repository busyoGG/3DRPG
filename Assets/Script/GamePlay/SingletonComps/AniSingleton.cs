using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AniSingleton : Singleton<AniSingleton>
{
    private Dictionary<int, string> _curAni = new Dictionary<int, string>();

    public string GetCurAni(int id)
    {
        string ani;
        _curAni.TryGetValue(id, out ani);
        return ani;
    }

    public void SetCurAni(int id, string ani)
    {
        if (_curAni.ContainsKey(id))
        {
            _curAni[id] = ani;
        }
        else
        {
            _curAni.Add(id, ani);
        }
    }
}
