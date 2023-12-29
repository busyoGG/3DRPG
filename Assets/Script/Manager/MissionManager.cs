using Bean;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class MissionManager : Singleton<MissionManager>
{
    /// <summary>
    /// ��������
    /// </summary>
    private Dictionary<int ,MissionConfigData> _allMission = new Dictionary<int, MissionConfigData> ();
    /// <summary>
    /// δ��������
    /// </summary>
    private Dictionary<int, MissionConfigData> _lockedMission = new Dictionary<int, MissionConfigData>();
    /// <summary>
    /// �ѽ�������
    /// </summary>
    private Dictionary<int, MissionConfigData> _unlockedMission = new Dictionary<int, MissionConfigData>();
    /// <summary>
    /// �Ƿ�֧���� ���������б����ʾ
    /// </summary>
    private Dictionary<int, MissionConfigData> _notBranchMission = new Dictionary<int, MissionConfigData>();
    /// <summary>
    /// ���������
    /// </summary>
    private Dictionary<int, MissionConfigData> _doneMission = new Dictionary<int, MissionConfigData>();
    /// <summary>
    /// Ԥ����
    /// </summary>
    private Dictionary<int, int> _preCount = new Dictionary<int, int>();
    /// <summary>
    /// ������� ��������׷��
    /// </summary>
    private MissionConfigData _curMission = null;

    private string[] _strFilter = new string[2] {
        "����",
        "֧��"
    };

    public void Init()
    {
        _allMission = ConfigManager.Ins().GetConfig<MissionConfigData>(ConfigsFolderConfig.Null, ConfigsNameConfig.MissionConfig);

        Dictionary<int, List<MissionConfigData>> steps = new Dictionary<int, List<MissionConfigData>>();

        foreach (var data in _allMission.Values)
        {
            if (data.stepId == 0)
            {
                if (data.isPreUnlock)
                {
                    SetUnlockedMission(data);
                }
                else
                {
                    _lockedMission.Add(data.mainId, data);
                }
            }
            else
            {
                List<MissionConfigData> stepList;
                steps.TryGetValue(data.stepId, out stepList);
                if (stepList == null)
                {
                    stepList = new List<MissionConfigData>();
                    steps.Add(data.mainId, stepList);
                }

                stepList.Add(data);
            }
        }
    }

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
    /// ��÷Ƿ�֧����
    /// </summary>
    /// <returns></returns>
    public Dictionary<int, MissionConfigData> GetNotBranchMission()
    {
        return _notBranchMission;
    }

    /// <summary>
    /// ͨ��id��÷Ƿ�֧����
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public MissionConfigData GetNotBranchMissionById(int id)
    {
        MissionConfigData MissionConfigData;
        _notBranchMission.TryGetValue(id, out MissionConfigData);
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
    public void RefreshPreCountNum(int targetId, int num, bool isAdd = false)
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
    /// ��������������
    /// </summary>
    /// <param name="config"></param>
    /// <param name="target"></param>
    /// <returns></returns>
    public int GetCompleteNum(MissionConfigData config, int target)
    {
        if (config.isPreCount)
        {
            if (_preCount.ContainsKey(target))
            {
                return _preCount[target];
            }
            return 0;
        }
        else
        {
            if (config.completeNum.ContainsKey(target))
            {
                return config.completeNum[target];
            }
            else
            {
                return 0;
            }
        }
    }

    /// <summary>
    /// ���������������
    /// </summary>
    /// <param name="mainId"></param>
    /// <param name="completeNum"></param>
    /// <param name="isAdd"></param>
    public void SetCompleteNum(int mainId, int targetId, int completeNum, bool isAdd = true)
    {
        MissionConfigData mission;
        _unlockedMission.TryGetValue(mainId, out mission);
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
            //SaveMissionData(mainId, mission.stepId, mission.completeNum, false);
            //�ж������Ƿ����
            if (CheckComplete(mission))
            {
                Next(mission);
            }
        }
    }

    /// <summary>
    /// ��������Ƿ����
    /// </summary>
    /// <param name="mainId"></param>
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
        //MissionConfigData mission = _unlockedMission[mainId];
        int mainId = mission.mainId;
        MissionConfigData next = _allMission[mission.next];
        if (next != null)
        {
            if (mission.unlockMission.Count > 0)
            {
                for (int i = 0, len = mission.unlockMission.Count; i < len; i++)
                {
                    int id = mission.unlockMission[i];
                    MissionConfigData unlockMission = _lockedMission[id];
                    SetUnlockedMission(unlockMission);
                    _lockedMission.Remove(id);
                    //��������
                    //SaveMissionData(id, unlockMission.stepId, unlockMission.completeNum, false);
                }
            }
            //��һ����
            SetUnlockedMission(next);
            //��������
            //SaveMissionData(mainId, next.stepId, next.completeNum, false);
        }
        else
        {
            SetUnlockedMission(mainId);
            _doneMission.Add(mainId, mission);
            //��������
            //SaveMissionData(mainId, mission.stepId, mission.completeNum, true);
        }

        //��ý���
        //GetAward(mission);

        if (mission.filter == MissionFilter.Branch)
        {
            MissionConfigData branchMissionRoot = GetUnlockedMissionById(mission.branchBelong);
            bool missionDone = true;
            foreach (var id in branchMissionRoot.branch)
            {
                MissionConfigData branch = GetUnlockedMissionById(id);
                if (branch != null)
                {
                    missionDone = false;
                    break;
                }
            }
            if (missionDone)
            {
                Next(branchMissionRoot);
            }
        }
    }

    /// <summary>
    /// ������������
    /// </summary>
    /// <param name="mission"></param>
    private void SetUnlockedMission(MissionConfigData mission)
    {
        if (mission.filter != MissionFilter.Branch)
        {
            if (_notBranchMission.ContainsKey(mission.mainId))
            {
                _notBranchMission[mission.mainId] = mission;
            }
            else
            {
                _notBranchMission.Add(mission.mainId, mission);
            }
        }

        if (_unlockedMission.ContainsKey(mission.mainId))
        {
            _unlockedMission[mission.mainId] = mission;
        }
        else
        {
            _unlockedMission.Add(mission.mainId, mission);
        }
    }

    /// <summary>
    /// ������������
    /// </summary>
    /// <param name="mission"></param>
    private void SetUnlockedMission(int mainId = -1)
    {
        if (_notBranchMission.ContainsKey(mainId))
        {
            _notBranchMission.Remove(mainId);
        }

        if (_unlockedMission.ContainsKey(mainId))
        {
            _unlockedMission[mainId] = null;
        }
        else
        {
            _unlockedMission.Add(mainId, null);
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

    //private void SaveMissionData(int mainId, int stepId, Dictionary<int, float> completeNum, bool isDone)
    //{

    //    //��������
    //    MissionSaveData missionSaveData;
    //    _missionSaveData.TryGetValue(mainId, out missionSaveData);
    //    if (missionSaveData == null)
    //    {
    //        missionSaveData = new MissionSaveData();
    //        missionSaveData.mainId = mainId;
    //        _missionSaveData.Add(mainId, missionSaveData);
    //    }

    //    missionSaveData.stepId = stepId;
    //    missionSaveData.completeNum = completeNum;
    //    missionSaveData.isDone = isDone;
    //}
}
