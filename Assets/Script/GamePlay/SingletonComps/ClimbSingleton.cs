

using System.Collections.Generic;

public class ClimbSingleton : Singleton<ClimbSingleton>
{
    private Dictionary<int, bool> _climbState = new Dictionary<int, bool>();

    public bool GetClimbState(int id)
    {
        bool isClimb;
        _climbState.TryGetValue(id, out isClimb);
        return isClimb || false;
    }

    public void SetClimbState(int id, bool isClimb)
    {
        if (_climbState.ContainsKey(id))
        {
            _climbState[id] = isClimb;
        }
        else
        {
            _climbState.Add(id, isClimb);
        }
    }
}
