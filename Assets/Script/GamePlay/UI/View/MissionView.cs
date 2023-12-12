using Bean;
using FairyGUI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class MissionView : BaseView
{
    private GList _missionList;

    //-----data-----

    //private List<int> _test = new List<int>();
    private List<MissionConfigData> _missions;

    private Dictionary<int, List<MissionConfigData>> _branchMissions;

    protected override void BindItem()
    {
        _missionList = main.GetChildAt(2).asList;
        _missionList.numItems = 0;
        _missionList.itemRenderer = MissionRenderer;
        _missionList.itemProvider = MissionProvider;
        _missionList.SetVirtual();
    }

    protected override void InitListener()
    {
        EventManager.AddListening(id, "show_mission", ShowMission);
    }

    protected override void OnShow()
    {
        UIManager.Ins().SetFocus(true);
    }

    protected override void OnHide()
    {
        ConsoleUtils.Log("关闭任务");
        UIManager.Ins().SetFocus(false);
    }

    private string MissionProvider(int index)
    {
        MissionConfigData res = _missions[index];

        if (res.branch.Count > 0)
        {
            return UIPackage.GetItemURL(UIManager.UIPack.Main, "BranchButton");
        }
        else
        {
            return UIPackage.GetItemURL(UIManager.UIPack.Main, "NormalButton");
        }
    }

    private void MissionRenderer(int index, GObject obj)
    {
        MissionConfigData res = _missions[index];
        GButton btnMission = obj.asButton;
        GTextField title = btnMission.GetChildAt(3).asTextField;

        title.text = res.title;

        if (res.branch.Count > 0)
        {
            //获取列表组件
            GList branchMissionList = btnMission.GetChildAt(4).asList;
            branchMissionList.data = index;
            branchMissionList.SetVirtual();
            branchMissionList.itemRenderer = BranchMissionRenderer;
            branchMissionList.numItems = _branchMissions[res.missionId].Count;

            //点击展开列表
            btnMission.onClick.Set(ResizeMissionList);
            //_missionList.onClickItem.Set(ResizeMissionList);
        }
        else
        {
            //_missionList.onClickItem.Set(OnMissionClick);
            //点击任务详情 监听按钮才能阻止冒泡
            btnMission.onClick.Set(OnMissionClick);
        }
    }

    private void BranchMissionRenderer(int index, GObject obj)
    {
        GButton btnMission = obj.asButton;
        GTextField title = btnMission.GetChildAt(3).asTextField;

        MissionConfigData res = _branchMissions[_missions[(int)btnMission.parent.data].missionId][index];

        title.text = res.title;

        btnMission.onClick.Set(OnMissionClick);
    }

    private void OnMissionClick(EventContext context)
    {
        context.StopPropagation();
    }

    private void ShowMission(ArrayList data)
    {
        Dictionary<int, MissionConfigData> missionDic = (Dictionary<int, MissionConfigData>)data[0];
        List<MissionConfigData> missions = missionDic.Values.ToList();

        _missions = missions;

        if (_branchMissions == null)
        {
            _branchMissions = new Dictionary<int, List<MissionConfigData>>();
        }

        //初始化分支任务
        foreach (var mission in missions)
        {
            foreach (var branchMission in mission.branch)
            {
                if(!_branchMissions.ContainsKey(mission.missionId))
                {
                    _branchMissions.Add(mission.missionId, new List<MissionConfigData>());
                }
                MissionConfigData branchMissionData = MissionManager.Ins().GetUnlockedMissionById(branchMission);
                _branchMissions[mission.missionId].Add(branchMissionData);
            }
        }

        RefreshMissionList();
    }

    private void RefreshMissionList()
    {
        _missionList.numItems = _missions.Count;
    }

    private void ResizeMissionList(EventContext context)
    {
        GButton button = context.sender as GButton;
        button.selected = !button.selected;
        if (button.selected)
        {
            //button.height = 240;
            List<MissionConfigData> data = _branchMissions[_missions[_missionList.GetChildIndex(button)].missionId];
            button.height = 120 + 120 * data.Count + 10 * (data.Count - 1);
        }
        else
        {
            button.height = 120;
        }
        _missionList.RefreshVirtualList();
    }
}
