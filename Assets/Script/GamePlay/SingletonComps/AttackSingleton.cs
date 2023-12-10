using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackSingleton : Singleton<AttackSingleton>
{
    private Dictionary<int, bool> _attackEnable = new Dictionary<int, bool>();

    public bool GetAttackEnable(int id)
    {
        bool isEnable;
        _attackEnable.TryGetValue(id, out isEnable);
        return isEnable;
    }

    public void SetAttackEnable(int id,bool isEnable)
    {
        if(_attackEnable.ContainsKey(id))
        {
            _attackEnable[id] = isEnable;
        }
        else
        {
            _attackEnable.Add(id, isEnable);   
        }
    }
}
