using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionManager : Singleton<MissionManager>
{
    //private Dictionary<int, MissionTree> _lockedMission = new Dictionary<int, MissionTree>();

    //private Dictionary<int, MissionTree> _unlockedMission = new Dictionary<int, MissionTree>();

    //private Dictionary<int, MissionTree> _doneMission = new Dictionary<int, MissionTree>();

    //private Dictionary<int, MissionSaveData> _missionSaveData = new Dictionary<int, MissionSaveData>();

    private Dictionary<int, float> _preCount = new Dictionary<int, float>();
}
