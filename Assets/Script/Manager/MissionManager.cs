using Bean;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionManager : Singleton<MissionManager>
{
    private Dictionary<int, MissionConfigData> _lockedMission = new Dictionary<int, MissionConfigData>();

    private Dictionary<int, MissionConfigData> _unlockedMission = new Dictionary<int, MissionConfigData>();

    private Dictionary<int, MissionConfigData> _doneMission = new Dictionary<int, MissionConfigData>();

    private Dictionary<int, float> _preCount = new Dictionary<int, float>();

    private MissionConfigData _curMission = null;

    private string[] _strFilter = new string[2] { 
        "����",
        "֧��"
    };

    /// <summary>
    /// ��õ�ǰ����
    /// ��������׷��
    /// </summary>
    /// <returns></returns>
    public MissionConfigData GetCurMission()
    {
        return _curMission;
    }

    /// <summary>
    /// ���õ�ǰ����
    /// </summary>
    /// <param name="mission"></param>
    public void SetCurMission(MissionConfigData mission)
    {
        _curMission = mission;
    }

    /// <summary>
    /// ��ȡ�����ѽ�������
    /// </summary>
    /// <returns></returns>
    public Dictionary<int, MissionConfigData> GetUnlockedMission()
    {
        return _unlockedMission;
    }

    /// <summary>
    /// ��ȡ������ɵ�����
    /// </summary>
    /// <returns></returns>
    public Dictionary<int, MissionConfigData> GetDoneMission()
    {
        return _doneMission;
    }

    /// <summary>
    /// ͨ��id����ѽ�������
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public MissionConfigData GetUnlockedMissionById(int id)
    {
        MissionConfigData MissionConfigData;
        _unlockedMission.TryGetValue(id, out MissionConfigData);
        return MissionConfigData;
    }

    /// <summary>
    /// �������ɸѡ������������
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public string GetMissionFilterString(MissionFilter type)
    {
        switch (type)
        {
            case MissionFilter.Main:
            default:
                return _strFilter[0];
            case MissionFilter.Sub:
                return _strFilter[1];
        }
    }

    /// <summary>
    /// �������ɸѡ����
    /// </summary>
    /// <param name="filter"></param>
    /// <returns></returns>
    public MissionFilter GetMissionFileter(string filter)
    {
        switch (filter)
        {
            case "����":
            default:
                return MissionFilter.Main;
            case "֧��":
                return MissionFilter.Sub;
        }
    }

    /// <summary>
    /// ˢ��Ԥͳ������������������
    /// </summary>
    /// <param name="targetId"></param>
    /// <param name="num"></param>
    public void RefreshPreCountNum(int targetId, float num, bool isAdd = false)
    {
        if (!_preCount.ContainsKey(targetId))
        {
            _preCount.Add(targetId, 0);
        }
        if (isAdd)
        {
            _preCount[targetId] += num;
        }
        else
        {
            _preCount[targetId] = num;
        }
    }

    /// <summary>
    /// ���Ԥͳ������
    /// </summary>
    /// <param name="targetId"></param>
    /// <returns></returns>
    public float GetPreCountNum(int targetId)
    {
        return _preCount[targetId];
    }

    /// <summary>
    /// ���������������
    /// </summary>
    /// <param name="missionId"></param>
    /// <param name="completeNum"></param>
    /// <param name="isAdd"></param>
    public void SetCompleteNum(int missionId, int targetId, int completeNum, bool isAdd = true)
    {
        MissionConfigData mission;
        _unlockedMission.TryGetValue(missionId, out mission);
        if (mission != null)
        {
            if (!mission.completeNum.ContainsKey(targetId))
            {
                mission.completeNum.Add(targetId, 0);
            }
            if (isAdd)
            {
                mission.completeNum[targetId] += completeNum;
            }
            else
            {
                mission.completeNum[targetId] = completeNum;
            }
            //��������
            //SaveMissionData(missionId, mission.stepId, mission.completeNum, false);
        }
    }

    /// <summary>
    /// ��������Ƿ����
    /// </summary>
    /// <param name="missionId"></param>
    /// <returns></returns>
    public bool CheckComplete(MissionConfigData mission)
    {
        for (int i = 0, len = mission.target.Count; i < len; i++)
        {
            int target = mission.target[i];
            if (mission.isPreCount)
            {
                if (mission.targetNum[target] > _preCount[target])
                {
                    return false;
                }
            }
            else
            {
                if (mission.completeNum.Count == 0 || mission.targetNum[target] > mission.completeNum[target])
                {
                    return false;
                }
            }
        }
        return true;
    }

    /// <summary>
    /// ��һ����
    /// </summary>
    /// <param name="mission"></param>
    public void Next(MissionConfigData mission)
    {
        //MissionConfigData mission = _unlockedMission[missionId];
        int missionId = mission.missionId;
        MissionConfigData next = mission.next;
        if (next != null)
        {
            if (mission.unlockMission.Count > 0)
            {
                for (int i = 0, len = mission.unlockMission.Count; i < len; i++)
                {
                    int id = mission.unlockMission[i];
                    MissionConfigData unlockMission = _lockedMission[id];
                    _unlockedMission.Add(unlockMission.missionId, unlockMission);
                    _lockedMission.Remove(id);
                    //��������
                    //SaveMissionData(id, unlockMission.stepId, unlockMission.completeNum, false);
                }
            }
            //��һ����
            _unlockedMission[missionId] = next;
            //��������
            //SaveMissionData(missionId, next.stepId, next.completeNum, false);
        }
        else
        {
            _unlockedMission.Remove(missionId);
            _doneMission.Add(missionId, mission);
            //��������
            //SaveMissionData(missionId, mission.stepId, mission.completeNum, true);
        }

        //��ý���
        //GetAward(mission);

        if (mission.filter == MissionFilter.Branch)
        {
            bool missionDone = true;
            foreach (var id in mission.branch)
            {
                MissionConfigData branch = GetUnlockedMissionById(id);
                if (mission != null)
                {
                    missionDone = false;
                    break;
                }
            }
            if (!missionDone)
            {
                Next(GetUnlockedMissionById(mission.branchBelong));
            }
        }
    }

    ///// <summary>
    ///// ��ȡ����
    ///// </summary>
    ///// <param name="mission"></param>
    //private void GetAward(MissionConfigData mission)
    //{
    //    string award = "";
    //    for (int i = 0, len = mission.award.Count; i < len; i++)
    //    {
    //        award += mission.award[i] + " ����:" + mission.awardNum[i];
    //    }
    //    Debug.Log("��������� ===> " + award);
    //}

    //private void SaveMissionData(int missionId, int stepId, Dictionary<int, float> completeNum, bool isDone)
    //{

    //    //��������
    //    MissionSaveData missionSaveData;
    //    _missionSaveData.TryGetValue(missionId, out missionSaveData);
    //    if (missionSaveData == null)
    //    {
    //        missionSaveData = new MissionSaveData();
    //        missionSaveData.missionId = missionId;
    //        _missionSaveData.Add(missionId, missionSaveData);
    //    }

    //    missionSaveData.stepId = stepId;
    //    missionSaveData.completeNum = completeNum;
    //    missionSaveData.isDone = isDone;
    //}
}
