using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntersectSingleton : Singleton<IntersectSingleton>
{
    /// <summary>
    /// 相交物体
    /// </summary>
    private Dictionary<int, LinkedList<(int, Entity)>> _intersectObjs = new Dictionary<int, LinkedList<(int, Entity)>>();

    private Dictionary<int, Dictionary<int, bool>> _intersectDic = new Dictionary<int, Dictionary<int, bool>>();

    public LinkedList<(int, Entity)> GetIntersectObjs(int id)
    {
        LinkedList<(int, Entity)> intersetcObjs;
        _intersectObjs.TryGetValue(id, out intersetcObjs);
        if (intersetcObjs == null)
        {
            intersetcObjs = new LinkedList<(int, Entity)>();
            _intersectObjs.Add(id, intersetcObjs);
        }
        return intersetcObjs;
    }

    public Dictionary<int, bool> GetIntersectDic(int id)
    {
        Dictionary<int, bool> dic;
        _intersectDic.TryGetValue(id, out dic);
        if(dic == null)
        {
            dic = new Dictionary<int, bool>();
            _intersectDic.Add(id, dic);
        }
        return dic;
    }
}
