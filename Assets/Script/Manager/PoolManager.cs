using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : Singleton<PoolManager>
{
    private Dictionary<string,Stack<GameObject>> _gameObjectPool = new Dictionary<string,Stack<GameObject>>();

    private GameObject _poolNode;

    public void Init()
    {
        _poolNode = new GameObject();
        _poolNode.name = "GameobjectPool";
        _poolNode.SetActive(false);
    }

    public GameObject Get(string name)
    {
        Stack<GameObject> objs;
        _gameObjectPool.TryGetValue(name, out objs);

        if(objs == null)
        {
            objs = new Stack<GameObject>();
            _gameObjectPool.Add(name, objs);
        }

        if(objs.Count > 0)
        {
            return objs.Pop();
        }
        else
        {
            //TODO 创建物体
            return null;
        }
    }

    public void Recover(string name,GameObject obj)
    {
        Stack<GameObject> objs;
        _gameObjectPool.TryGetValue(name, out objs);

        if (objs == null)
        {
            objs = new Stack<GameObject>();
            _gameObjectPool.Add(name, objs);
        }

        obj.transform.parent = _poolNode.transform;
        objs.Push(obj);
    }
}
