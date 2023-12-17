using FairyGUI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class TriggerList : BaseView
{
    GList _list;

    private Dictionary<int, (string, Action)> _itemsDic = new Dictionary<int, (string, Action)>();

    private List<(string, Action)> _items = new List<(string, Action)>();

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

    protected override void InitAction()
    {
        _list.onClickItem.Set(OnButtonClick);
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
        GButton button = obj.asButton;
        GTextField content = button.GetChildAt(3).asTextField;

        content.text = _items[index].Item1;

        //button.onClick.Set(OnPressButton);
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
                Action action = _items[selection].Item2;
                _items.RemoveAt(selection);
                RefreshList();
                action.Invoke();
            }
        }
    }

    private void OnButtonClick()
    {
        int selection = _list.selectedIndex;
        if (selection < _items.Count)
        {
            Action action = _items[selection].Item2;
            _items.RemoveAt(selection);
            RefreshList();
            action.Invoke();
        }
    }

    private void ChangeItem(ArrayList data)
    {
        bool isAdd = (bool)data[0];
        int id = (int)data[1];
        if (isAdd)
        {
            string target = (string)data[2];
            Action action = (Action)data[3];
            _itemsDic.Add(id, (target, action));
        }
        else
        {
            _itemsDic.Remove(id);
        }

        _items.Clear();
        foreach (var item in _itemsDic.Values)
        {
            _items.Add(item);
        }

        RefreshList();
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
