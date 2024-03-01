using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QTreeSingleton : Singleton<QTreeSingleton>
{
    //private Dictionary<int,List<QTreeObj>> _objs = new Dictionary<int, List<QTreeObj>>();

    //public List<QTreeObj> GetQtreeObjs(int id)
    //{
    //    List<QTreeObj> objs = null;
    //    _objs.TryGetValue(id, out objs);
    //    if(objs == null)
    //    {
    //        objs = new List<QTreeObj>();
    //        _objs.Add(id, objs);
    //    }
    //    return objs;
    //}
}
