using FairyGUI;
using System;
using System.Collections;
using System.Collections.Generic;


public class TriggerList : BaseView
{
    [UICompBind("List","0")]
    GList _list { get; set; }

    private Dictionary<int, (string, Action)> _itemsDic = new Dictionary<int, (string, Action)>();

    [UIDataBind("List","0","height")]
    private UIListProp<(string, Action)> _items { get; set; }

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

    [UIActionBind("ListRender","0")]
    private void ItemRenderer(int index, GObject obj)
    {
        GButton button = obj.asButton;
        GTextField content = button.GetChildAt(3).asTextField;

        content.text = _items.Get()[index].Item1;

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
                List<(string, Action)> listData = _items.Get();
                Action action = listData[selection].Item2;
                listData.RemoveAt(selection);
                _items.Set(listData);

                RefreshList();
                action.Invoke();
            }
        }
    }

    [UIActionBind("ListClick","0")]
    private void OnButtonClick()
    {
        int selection = _list.selectedIndex;
        if (selection < _items.Count)
        {
            List<(string,Action)> listData = _items.Get();
            Action action = listData[selection].Item2;
            listData.RemoveAt(selection);
            _items.Set(listData);

            RefreshList();
            action.Invoke();
        }
    }

    [UIListenerBind("ChangeItem")]
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

        List<(string,Action)> newList = new List<(string, Action)> ();
        foreach (var item in _itemsDic.Values)
        {
            newList.Add(item);
        }

        _items.Set(newList);

        RefreshList();
    }

    private void RefreshList()
    {
        if (_items.Count > 0)
        {
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
