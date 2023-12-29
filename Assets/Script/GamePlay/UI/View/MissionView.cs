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
    private GList _missionList;

    private GComponent _detail;

    private GTextField _missionTitle;

    private GTextField _missionContent;

    private GList _missionRequestList;

    private GList _awardList;

    private GTextField _empty;

    private GButton _close;

    //-----data-----

    //private List<int> _test = new List<int>();
    private List<MissionConfigData> _missions;

    private Dictionary<int, List<MissionConfigData>> _branchMissions;

    private MissionConfigData _curMission;

    protected override void BindItem()
    {
        _missionList = main.GetChildAt(2).asList;
        _missionList.numItems = 0;
        _missionList.itemRenderer = MissionRenderer;
        _missionList.itemProvider = MissionProvider;
        _missionList.SetVirtual();

        _empty = main.GetChildAt(4).asTextField;
        _close = main.GetChildAt(5).asButton;

        _detail = main.GetChildAt(3).asCom;

        _missionTitle = _detail.GetChildAt(0).asTextField;
        _missionRequestList = _detail.GetChildAt(1).asList;
        _missionContent = _detail.GetChildAt(2).asTextField;
        _awardList = _detail.GetChildAt(3).asList;

        _missionRequestList.numItems = 0;
        _missionRequestList.itemRenderer = RequestRenderer;
        _missionRequestList.SetVirtual();

        _awardList.numItems = 0;
        _awardList.itemRenderer = AwardRenderer;
        _awardList.SetVirtual();
    }

    protected override void InitListener()
    {
        EventManager.AddListening(id, "show_mission_list", ShowMissionList);
    }

    protected override void InitAction()
    {
        _close.onClick.Set(OnCloseClick);
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

    /// <summary>
    /// 任务列表渲染
    /// </summary>
    /// <param name="index"></param>
    /// <param name="obj"></param>
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
            branchMissionList.numItems = _branchMissions[res.mainId].Count;

            //点击展开列表
            //btnMission.data = res;
            btnMission.onClick.Set(ResizeMissionList);
            //_missionList.onClickItem.Set(ResizeMissionList);
        }
        else
        {
            //_missionList.onClickItem.Set(OnMissionClick);
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

        MissionConfigData res = _branchMissions[_missions[(int)btnMission.parent.data].mainId][index];

        title.text = res.title;

        btnMission.data = res;
        btnMission.onClick.Set(OnMissionClick);
    }

    /// <summary>
    /// 需求道具渲染
    /// </summary>
    /// <param name="index"></param>
    /// <param name="obj"></param>
    private void RequestRenderer(int index, GObject obj)
    {
        GComponent button = obj.asCom;
        GTextField content = button.GetChildAt(1).asTextField;
        GTextField requstNum = button.GetChildAt(2).asTextField;

        int targetId = _curMission.target[index];

        content.text = _curMission.targetDescribe[targetId];
        requstNum.text = MissionManager.Ins().GetCompleteNum(_curMission, targetId) + "/" + _curMission.targetNum[targetId];
    }

    /// <summary>
    /// 奖励渲染
    /// </summary>
    /// <param name="index"></param>
    /// <param name="obj"></param>
    private void AwardRenderer(int index, GObject obj)
    {
        GButton button = obj.asButton;
        GTextField num = button.GetChildAt(5).asTextField;

        num.text = _curMission.awardNum[index].ToString();
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
            List<MissionConfigData> data = _branchMissions[_missions[_missionList.GetChildIndex(button)].mainId];
            button.height = 120 + 120 * data.Count + 10 * (data.Count - 1);
        }
        else
        {
            button.height = 120;
        }
        _missionList.RefreshVirtualList();
    }

    private void OnCloseClick()
    {
        UIManager.Ins().Hide(this);
    }

    //-----数据更新-----

    /// <summary>
    /// 显示任务列表
    /// </summary>
    /// <param name="data"></param>
    private void ShowMissionList(ArrayList data)
    {
        Dictionary<int, MissionConfigData> missionDic = (Dictionary<int, MissionConfigData>)data[0];
        List<MissionConfigData> missions = missionDic.Values.ToList();

        _missions = missions;

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


        RefreshMissionList();
    }

    private void SetDetail(MissionConfigData config)
    {
        _curMission = config;
        _missionTitle.text = config.title;
        _missionContent.text = config.desc;

        RefreshRequestList();
        RefreshAwardList();
    }

    //-----刷新-----

    private void RefreshMissionList()
    {
        _missionList.numItems = _missions.Count;
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

    private void RefreshRequestList()
    {
        _missionRequestList.numItems = _curMission.target.Count;
        _missionRequestList.height = _curMission.target.Count * _missionRequestList.GetChildAt(0).height + _missionRequestList.lineGap * (_curMission.target.Count - 1);
    }

    private void RefreshAwardList()
    {
        _awardList.numItems = _curMission.award.Count;
        _awardList.width = _curMission.target.Count * _awardList.GetChildAt(0).width + _awardList.columnGap * (_curMission.target.Count - 1) + _awardList.margin.left + _awardList.margin.right;
    }
}
