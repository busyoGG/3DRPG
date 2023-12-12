using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionSingleton : Singleton<CollisionSingleton>
{
    private Dictionary<int, List<BoxComp>> _colliders = new Dictionary<int, List<BoxComp>>();

    public List<BoxComp> GetColliders(int id)
    {
        List<BoxComp> BoxComps;
        _colliders.TryGetValue(id, out BoxComps);
        if (BoxComps == null)
        {
            BoxComps = new List<BoxComp>();
            _colliders.Add(id, BoxComps);
        }
        return BoxComps;
    }
}
