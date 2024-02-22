using Bean;
using FairyGUI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing.Text;
using System.Linq;
using Unity.VisualScripting.FullSerializer;

public class MissionView : BaseView
{
    [UICompBind("List","2")]
    private GList _missionList { get; set; }

    [UICompBind("Comp","3")]
    private GComponent _detail { get; set; }

    [UICompBind("TextField", "4")]
    private GTextField _empty { get; set; }

    //-----data-----

    [UIDataBind("TextFeild", "3/0")]
    private UIProp _missionTitle { get; set; }

    [UIDataBind("TextFeild", "3/2")]
    private UIProp _missionContent { get; set; }

    [UIDataBind("List", "2")]
    private UIListProp<MissionConfigData> _missions { get; set; }

    [UIDataBind("List", "3/1", "height")]
    private UIListProp<int> _requests { get; set; }
    [UIDataBind("List", "3/3", "width")]
    private UIListProp<int> _awards { get; set; }

    private Dictionary<int, List<MissionConfigData>> _branchMissions;

    private MissionConfigData _curMission;

    protected override void OnShow()
    {
        UIManager.Ins().SetFocus(true);
    }

    protected override void OnHide()
    {
        ConsoleUtils.Log("关闭任务");
        UIManager.Ins().SetFocus(false);
    }

    [UIActionBind("ListProvider", "2")]
    private string MissionProvider(int index)
    {
        MissionConfigData res = _missions.Get()[index];

        if (res.branch.Count > 0)
        {
            return UIPackage.GetItemURL(UIManager.UIPack.Main, "BranchButton");
        }
        else
        {
            return UIPackage.GetItemURL(UIManager.UIPack.Main, "NormalButton");
        }
    }

    /// <summary>
    /// 任务列表渲染
    /// </summary>
    /// <param name="index"></param>
    /// <param name="obj"></param>
    [UIActionBind("ListRender", "2")]
    private void MissionRenderer(int index, GObject obj)
    {
        MissionConfigData res = _missions.Get()[index];
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
            branchMissionList.numItems = _branchMissions[res.mainId].Count;

            //点击展开列表
            btnMission.onClick.Set(ResizeMissionList);
        }
        else
        {
            //点击任务详情 监听按钮才能阻止冒泡
            btnMission.data = res;
            btnMission.onClick.Set(OnMissionClick);
        }
    }

    /// <summary>
    /// 分支任务渲染
    /// </summary>
    /// <param name="index"></param>
    /// <param name="obj"></param>
    private void BranchMissionRenderer(int index, GObject obj)
    {
        GButton btnMission = obj.asButton;
        GTextField title = btnMission.GetChildAt(3).asTextField;

        MissionConfigData res = _branchMissions[_missions.Get()[(int)btnMission.parent.data].mainId][index];

        title.text = res.title;

        btnMission.data = res;
        btnMission.onClick.Set(OnMissionClick);
    }

    /// <summary>
    /// 需求道具渲染
    /// </summary>
    /// <param name="index"></param>
    /// <param name="obj"></param>
    [UIActionBind("ListRender", "3/1")]
    private void RequestRenderer(int index, GObject obj)
    {
        GComponent button = obj.asCom;
        GTextField content = button.GetChildAt(1).asTextField;
        GTextField requstNum = button.GetChildAt(2).asTextField;

        int targetId = _requests.Get()[index];

        content.text = _curMission.targetDescribe[targetId];
        requstNum.text = MissionManager.Ins().GetCompleteNum(_curMission, targetId) + "/" + _curMission.targetNum[targetId];
    }

    /// <summary>
    /// 奖励渲染
    /// </summary>
    /// <param name="index"></param>
    /// <param name="obj"></param>
    [UIActionBind("ListRender", "3/3")]
    private void AwardRenderer(int index, GObject obj)
    {
        GButton button = obj.asButton;
        GTextField num = button.GetChildAt(5).asTextField;

        num.text = _awards.Get()[index].ToString();
    }

    //-----点击事件-----

    /// <summary>
    /// 点击任务详情
    /// </summary>
    /// <param name="context"></param>
    private void OnMissionClick(EventContext context)
    {
        GButton button = context.sender as GButton;
        MissionConfigData res = (MissionConfigData)button.data;
        SetDetail(res);
        context.StopPropagation();
    }

    private void ResizeMissionList(EventContext context)
    {
        GButton button = context.sender as GButton;
        button.selected = !button.selected;
        if (button.selected)
        {
            List<MissionConfigData> data = _branchMissions[_missions.Get()[_missionList.GetChildIndex(button)].mainId];
            button.height = 120 + 120 * data.Count + 10 * (data.Count - 1);
        }
        else
        {
            button.height = 120;
        }
        _missionList.RefreshVirtualList();
    }

    [UIActionBind("Click", "5")]
    private void OnCloseClick()
    {
        UIManager.Ins().Hide(this);
    }

    //-----数据更新-----

    /// <summary>
    /// 显示任务列表
    /// </summary>
    /// <param name="data"></param>
    [UIListenerBind("show_mission_list")]
    private void ShowMissionList(ArrayList data)
    {
        Dictionary<int, MissionConfigData> missionDic = (Dictionary<int, MissionConfigData>)data[0];
        List<MissionConfigData> missions = missionDic.Values.ToList();

        if (_branchMissions == null)
        {
            _branchMissions = new Dictionary<int, List<MissionConfigData>>();
        }

        bool initDetail = false;
        //初始化分支任务
        foreach (var mission in missions)
        {
            if (!_branchMissions.ContainsKey(mission.mainId))
            {
                _branchMissions.Add(mission.mainId, new List<MissionConfigData>());
            }
            _branchMissions[mission.mainId].Clear();

            if (!initDetail && mission.branch.Count == 0)
            {
                initDetail = true;
                SetDetail(mission);
            }

            foreach (var branchMission in mission.branch)
            {
                MissionConfigData branchMissionData = MissionManager.Ins().GetUnlockedMissionById(branchMission);
                _branchMissions[mission.mainId].Add(branchMissionData);

                if (!initDetail)
                {
                    initDetail = true;
                    SetDetail(branchMissionData);
                }
            }
        }

        _missions.Set(missions);

        RefreshMissionList();
    }

    private void SetDetail(MissionConfigData config)
    {
        _curMission = config;
        _requests.Set(config.target);
        _awards.Set(config.awardNum);
        _missionTitle.Set(config.title);
        _missionContent.Set(config.desc);

        //RefreshRequestList();
        //RefreshAwardList();
    }

    //-----刷新-----

    private void RefreshMissionList()
    {
        //_missionList.numItems = _missions.Count;
        if (_missions.Count > 0)
        {
            _empty.visible = false;
            _detail.visible = true;
        }
        else
        {
            _empty.visible = true;
            _detail.visible = false;
        }
    }
}
