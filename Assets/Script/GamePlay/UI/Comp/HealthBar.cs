using FairyGUI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : BaseUIComp
{
    private GGraph _mask;

    private int _totalHp;

    private int _curHp;

    private float _baseWidth;

    protected override void BindItem()
    {
        _mask = main.GetChildAt(1).asCom.GetChildAt(1).asGraph;
        _baseWidth = _mask.width;
    }

    public void SetOffsetY(int y)
    {
        main.y += y;
    }

    public void SetProp(int hp)
    {
        _totalHp = hp;
        Refresh();
    }

    public void SetHp(int hp)
    {
        if (_curHp == hp) return;
        _curHp = hp;
        Refresh();
    }

    private void Refresh()
    {
        float ratio = _curHp / (float)_totalHp;
        _mask.width = _baseWidth * ratio;
    }

    public override void Reset()
    {
        main.y = 0;
    }
}
