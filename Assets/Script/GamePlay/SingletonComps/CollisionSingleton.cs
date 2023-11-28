using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionSingleton : Singleton<CollisionSingleton>
{
    private Dictionary<int, List<CollideComp>> _colliders = new Dictionary<int, List<CollideComp>>();

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
}
