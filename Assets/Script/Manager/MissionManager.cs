using Bean;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class MissionManager : Singleton<MissionManager>
{
    private Dictionary<int, MissionConfigData> _lockedMission = new Dictionary<int, MissionConfigData>();

    private Dictionary<int, MissionConfigData> _unlockedMission = new Dictionary<int, MissionConfigData>();

    private Dictionary<int, MissionConfigData> _notBranchMission = new Dictionary<int, MissionConfigData>();

    private Dictionary<int, MissionConfigData> _doneMission = new Dictionary<int, MissionConfigData>();

    private Dictionary<int, int> _preCount = new Dictionary<int, int>();

    private MissionConfigData _curMission = null;

    private string[] _strFilter = new string[2] {
        "主线",
        "支线"
    };

    public void Init()
    {
        Dictionary<int, MissionConfigData> missionDic = ConfigManager.Ins().GetConfig<MissionConfigData>(ConfigsFolderConfig.Null, ConfigsNameConfig.MissionConfig);

        Dictionary<int, List<MissionConfigData>> steps = new Dictionary<int, List<MissionConfigData>>();

        foreach (var data in missionDic.Values)
        {
            if (data.stepId == 0)
            {
                if (data.isPreUnlock)
                {
                    SetUnlockedMission(data);
                }
                else
                {
                    _lockedMission.Add(data.missionId, data);
                }
            }
            else
            {
                List<MissionConfigData> stepList;
                steps.TryGetValue(data.stepId, out stepList);
                if (stepList == null)
                {
                    stepList = new List<MissionConfigData>();
                    steps.Add(data.missionId, stepList);
                }

                stepList.Add(data);
            }
        }

        MissionConfigData temp;
        foreach (var data in steps)
        {
            data.Value.Sort((a, b) => { return a.stepId.CompareTo(b.stepId); });
            temp = _unlockedMission[data.Key];
            foreach (var mission in data.Value)
            {
                temp.next = mission;
                temp = temp.next;
            }
        }
    }

    /// <summary>
    /// 获得当前任务
    /// 用于任务追踪
    /// </summary>
    /// <returns></returns>
    public MissionConfigData GetCurMission()
    {
        return _curMission;
    }

    /// <summary>
    /// 设置当前任务
    /// </summary>
    /// <param name="mission"></param>
    public void SetCurMission(MissionConfigData mission)
    {
        _curMission = mission;
    }

    /// <summary>
    /// 获取所有已解锁任务
    /// </summary>
    /// <returns></returns>
    public Dictionary<int, MissionConfigData> GetUnlockedMission()
    {
        return _unlockedMission;
    }

    /// <summary>
    /// 获取所有完成的任务
    /// </summary>
    /// <returns></returns>
    public Dictionary<int, MissionConfigData> GetDoneMission()
    {
        return _doneMission;
    }

    /// <summary>
    /// 通过id获得已解锁任务
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
    /// 获得非分支任务
    /// </summary>
    /// <returns></returns>
    public Dictionary<int, MissionConfigData> GetNotBranchMission()
    {
        return _notBranchMission;
    }

    /// <summary>
    /// 通过id获得非分支任务
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
    /// 获得任务筛选类型文字描述
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
    /// 获得任务筛选类型
    /// </summary>
    /// <param name="filter"></param>
    /// <returns></returns>
    public MissionFilter GetMissionFileter(string filter)
    {
        switch (filter)
        {
            case "主线":
            default:
                return MissionFilter.Main;
            case "支线":
                return MissionFilter.Sub;
        }
    }

    /// <summary>
    /// 刷新预统计类型任务的完成数量
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
    /// 获得任务完成数量
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
    /// 设置任务完成数量
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
            //保存数据
            //SaveMissionData(missionId, mission.stepId, mission.completeNum, false);
            //判断任务是否完成
            if (CheckComplete(mission))
            {
                Next(mission);
            }
        }
    }

    /// <summary>
    /// 检测任务是否完成
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
    /// 下一任务
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
                    SetUnlockedMission(unlockMission);
                    _lockedMission.Remove(id);
                    //保存数据
                    //SaveMissionData(id, unlockMission.stepId, unlockMission.completeNum, false);
                }
            }
            //下一任务
            SetUnlockedMission(next);
            //保存数据
            //SaveMissionData(missionId, next.stepId, next.completeNum, false);
        }
        else
        {
            SetUnlockedMission(missionId);
            _doneMission.Add(missionId, mission);
            //保存数据
            //SaveMissionData(missionId, mission.stepId, mission.completeNum, true);
        }

        //获得奖励
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
    /// 设置任务数据
    /// </summary>
    /// <param name="mission"></param>
    private void SetUnlockedMission(MissionConfigData mission)
    {
        if (mission.filter != MissionFilter.Branch)
        {
            if (_notBranchMission.ContainsKey(mission.missionId))
            {
                _notBranchMission[mission.missionId] = mission;
            }
            else
            {
                _notBranchMission.Add(mission.missionId, mission);
            }
        }

        if (_unlockedMission.ContainsKey(mission.missionId))
        {
            _unlockedMission[mission.missionId] = mission;
        }
        else
        {
            _unlockedMission.Add(mission.missionId, mission);
        }
    }

    /// <summary>
    /// 设置任务数据
    /// </summary>
    /// <param name="mission"></param>
    private void SetUnlockedMission(int missionId = -1)
    {
        if (_notBranchMission.ContainsKey(missionId))
        {
            _notBranchMission.Remove(missionId);
        }

        if (_unlockedMission.ContainsKey(missionId))
        {
            _unlockedMission[missionId] = null;
        }
        else
        {
            _unlockedMission.Add(missionId, null);
        }
    }

    ///// <summary>
    ///// 获取奖励
    ///// </summary>
    ///// <param name="mission"></param>
    //private void GetAward(MissionConfigData mission)
    //{
    //    string award = "";
    //    for (int i = 0, len = mission.award.Count; i < len; i++)
    //    {
    //        award += mission.award[i] + " 数量:" + mission.awardNum[i];
    //    }
    //    Debug.Log("获得任务奖励 ===> " + award);
    //}

    //private void SaveMissionData(int missionId, int stepId, Dictionary<int, float> completeNum, bool isDone)
    //{

    //    //保存数据
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
