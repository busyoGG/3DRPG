using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

public class PropManager : Singleton<PropManager>
{
    public void Init()
    {
        Dictionary<int, PropData> data = AttackSingleton.Ins().GetPropData();
        foreach (var prop in data.Values)
        {
            prop.curHp = prop.hp;
            prop.curMp = prop.mp;
        }
    }

    public PropData GetPropData(int id)
    {
        return AttackSingleton.Ins().GetPropData(id);
    }

    public void AddHp(int id, int hp)
    {
        PropData data = GetPropData(id);
        data.curHp += hp;
        if (data.curHp > data.hp)
        {
            data.curHp = data.hp;
        }

        if (data.curHp < 0)
        {
            data.hp = 0;
        }
    }

    public void AddMp(int id, int mp)
    {
        PropData data = GetPropData(id);
        int res = data.curMp + mp;
        if (res <= data.mp && res >= 0)
        {
            data.curMp += mp;
        }
    }

    public void AddSheild(int id, int sheild)
    {
        PropData data = GetPropData(id);
        data.curSheild += sheild;
        if(data.curSheild < 0)
        {
            AddHp(id, data.curSheild);
            data.curSheild = 0;
        }
    }

    public int GetDamage(int id)
    {
        PropData data = GetPropData(id);

        int res = 0;
        //TODO ÉËº¦¼ÆËã¹«Ê½
        res = data.attack;
        return res;
    }
}
