using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionSingleton : Singleton<CollisionSingleton>
{
    private Dictionary<int, List<CollideComp>> _colliders = new Dictionary<int, List<CollideComp>>();

    private Dictionary<int, List<Vector3>> _normalLists = new Dictionary<int, List<Vector3>>();

    private Dictionary<int, Vector3> _totalNomals = new Dictionary<int, Vector3>();

    private Dictionary<int, List<float>> _higherestLists = new Dictionary<int, List<float>>();

    private Dictionary<int, float> _higherests = new Dictionary<int, float>();

    private Dictionary<int, CollideComp> _higherestCollide = new Dictionary<int, CollideComp>();

    private Dictionary<int, List<Vector3>> _closestPoints = new Dictionary<int, List<Vector3>>();

    private Dictionary<int, List<Vector3>> _closestSelfPoints = new Dictionary<int, List<Vector3>>();

    private Dictionary<int, List<Vector3>> _insidePoints = new Dictionary<int, List<Vector3>>();

    public List<CollideComp> GetColliders(int id)
    {
        List<CollideComp> collideComps;
        _colliders.TryGetValue(id, out collideComps);
        if (collideComps == null)
        {
            collideComps = new List<CollideComp>();
            _colliders.Add(id, collideComps);
        }
        return collideComps;
    }

    public List<Vector3> GetNormalList(int id)
    {
        List<Vector3> normalList;
        _normalLists.TryGetValue(id, out normalList);
        if (normalList == null)
        {
            normalList = new List<Vector3>();
            _normalLists.Add(id, normalList);
        }
        return normalList;
    }

    public Vector3 GetTotalNormal(int id)
    {
        Vector3 normal;
        _totalNomals.TryGetValue(id, out normal);
        return normal;
    }

    public void SetTotalNormal(int id, Vector3 totalNormal)
    {
        if (_totalNomals.ContainsKey(id))
        {
            _totalNomals[id] = totalNormal;
        }
        else
        {
            _totalNomals.Add(id, totalNormal);
        }
    }

    public List<float> GetHigherestList(int id)
    {
        List<float> higherestList;
        _higherestLists.TryGetValue(id, out higherestList);
        if (higherestList == null)
        {
            higherestList = new List<float>();
            _higherestLists.Add(id, higherestList);
        }
        return higherestList;
    }

    public float GetHigherest(int id)
    {
        float higherest;
        _higherests.TryGetValue(id, out higherest);
        return higherest;
    }

    public void SetHigherest(int id, float higherest)
    {
        if (_higherests.ContainsKey(id))
        {
            _higherests[id] = higherest;
        }
        else
        {
            _higherests.Add(id, higherest);
        }
    }

    public CollideComp GetHigherestCollide(int id)
    {
        CollideComp higherestCollide;
        _higherestCollide.TryGetValue(id, out higherestCollide);
        return higherestCollide;
    }

    public void SetHigherestCollide(int id, CollideComp higherestCollide)
    {
        if (_higherests.ContainsKey(id))
        {
            _higherestCollide[id] = higherestCollide;
        }
        else
        {
            _higherestCollide.Add(id, higherestCollide);
        }
    }

    public List<Vector3> GetClosestPointList(int id)
    {
        List<Vector3> closestPoint;
        _closestPoints.TryGetValue(id, out closestPoint);
        if (closestPoint == null)
        {
            closestPoint = new List<Vector3>();
            _closestPoints.Add(id, closestPoint);
        }
        return closestPoint;
    }

    public List<Vector3> GetClosestPointSelfList(int id)
    {
        List<Vector3> closestPointSelf;
        _closestSelfPoints.TryGetValue(id, out closestPointSelf);
        if (closestPointSelf == null)
        {
            closestPointSelf = new List<Vector3>();
            _closestSelfPoints.Add(id, closestPointSelf);
        }
        return closestPointSelf;
    }

    public List<Vector3> GetInsidePointList(int id)
    {
        List<Vector3> insidePoint;
        _insidePoints.TryGetValue(id, out insidePoint);
        if(insidePoint == null)
        {
            insidePoint = new List<Vector3>();
            _insidePoints.Add(id, insidePoint);
        }
        return insidePoint;
    }
}
