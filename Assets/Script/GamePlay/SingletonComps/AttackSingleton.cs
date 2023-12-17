using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackSingleton : Singleton<AttackSingleton>
{
    private Dictionary<int, bool> _attackEnable = new Dictionary<int, bool>();

    private Dictionary<int, PropData> _propDatas = new Dictionary<int, PropData>();

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

    public Dictionary<int, PropData> GetPropData()
    {
        return _propDatas;
    }

    public PropData GetPropData(int id)
    {
        PropData prop;
        _propDatas.TryGetValue(id, out prop);
        if(prop == null)
        {
            prop = new PropData();
            _propDatas.Add(id, prop);
        }
        return prop;
    }

    //public void SetPropData(int id, PropData prop)
    //{
    //    if (_propDatas.ContainsKey(id))
    //    {
    //        _propDatas[id] = prop;
    //    }
    //    else
    //    {
    //        _propDatas.Add(id, prop);
    //    }
    //}
}
