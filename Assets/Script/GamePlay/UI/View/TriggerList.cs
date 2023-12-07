using FairyGUI;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TriggerList : BaseView
{
    GList _list;

    private List<Action> _items = new List<Action>();


    protected override void BindItem()
    {
        _list = main.GetChildAt(0).asList;
        _list.SetVirtual();
        _list.itemRenderer = ItemRenderer;
        _list.numItems = 0;
    }

    protected override void InitListener()
    {
        EventManager.AddListening(id, "ChangeItem", ChangeItem);
    }

    protected override void OnShow()
    {
        base.OnShow();
        InputManager.Ins().AddEventListener(OnPressButton);
    }

    protected override void OnHide()
    {
        base.OnHide();
        InputManager.Ins().RemoveEventListener(OnPressButton);
    }

    private void ItemRenderer(int index, GObject obj)
    {

    }

    private void OnPressButton()
    {
        Dictionary<InputKey, InputStatus> curKey = InputManager.Ins().GetKey();
        InputStatus keyF;
        curKey.TryGetValue(InputKey.F, out keyF);
        if (keyF == InputStatus.Down)
        {
            int selection = _list.selectedIndex;
            if (selection < _items.Count)
            {
                Action action = _items[selection];
                _items.RemoveAt(selection);
                RefreshList();
                action.Invoke();
            }
        }
    }

    private void ChangeItem(ArrayList data)
    {
        bool isAdd = (bool)data[0];
        Action action = (Action)data[1];
        if (isAdd)
        {
            _items.Add(action);
        }
        else
        {
            if (_items.Contains(action))
            {
                _items.Remove(action);
            }
        }

        RefreshList();
        //else
        //{
        //    UIManager.Ins().Show<TriggerList>(name);
        //}
    }

    private void RefreshList()
    {
        _list.numItems = _items.Count;
        if (_items.Count > 0)
        {
            _list.height = _items.Count * _list.GetChildAt(0).height + _list.lineGap * (_items.Count - 1);
            if (_items.Count == 1)
            {
                _list.selectedIndex = 0;
            }
        }
        else
        {
            UIManager.Ins().Hide(this);
        }
    }
}
