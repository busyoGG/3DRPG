using FairyGUI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingView : BaseView
{
    private GGraph _progress;

    private GTextField _hint;

    private float _ratio;

    private float _defWidth;

    private int _strIndex = 0;

    private TimerChain _timer;

    private string[] _loadingStr = { 
        "加载中",
        "加载中.",
        "加载中..",
        "加载中..."
    };

    //protected override void BindItem()
    //{
    //    _progress = main.GetChildAt(1).asCom.GetChildAt(1).asGraph;
    //    _hint = main.GetChildAt(2).asTextField;

    //    _defWidth = _progress.width;
    //    _progress.width = 0;
    //}

    protected override void OnShow()
    {
        _timer = TimerUtils.Loop(100, RefreshProgress);
    }

    protected override void OnHide()
    {
        TimerUtils.Clear(_timer);
    }

    public void SetProgress(float ratio)
    {
        _ratio = ratio;
    }

    private void RefreshProgress()
    {
        _progress.width = _ratio * _defWidth;
        _hint.text = _loadingStr[_strIndex];
        _strIndex++;
        if(_strIndex >= _loadingStr.Length)
        {
            _strIndex = 0;
        }
    }
}
