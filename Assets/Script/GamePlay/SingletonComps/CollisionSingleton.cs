using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionSingleton : Singleton<CollisionSingleton>
{
    private Dictionary<int, List<CollideComp>> _colliders = new Dictionary<int, List<CollideComp>>();

    private Dictionary<int, List<Vector3>> _offsets = new Dictionary<int, List<Vector3>>();

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

    public List<Vector3> GetOffsets(int id)
    {
        List<Vector3> offsets;
        _offsets.TryGetValue(id, out offsets);
        if (offsets == null)
        {
            offsets = new List<Vector3>();
            _offsets.Add(id, offsets);
        }
        return offsets;
    }
}
