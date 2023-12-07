using Bean;
using FairyGUI;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Progress;

public class ConversationView : BaseView
{
    private GList _listSelection;

    private GTextField _textTalker;

    private GButton _btnAuto;

    private GButton _btnConversation;

    //-----保存数据-----

    private List<string> _selection = new List<string>();

    private string[] _strAuto = new string[2] { "自动", "自动中" };

    private bool _isAuto = false;

    private DialogConfigData _curDialog;

    protected override void BindItem()
    {
        _btnConversation = main.GetChildAt(0).asButton;

        _textTalker = main.GetChildAt(2).asTextField;

        _listSelection = main.GetChildAt(3).asList;

        _btnAuto = main.GetChildAt(4).asButton;

        _listSelection.SetVirtual();
        _listSelection.itemRenderer = SelectionRenderer;
        _listSelection.numItems = 0;
    }

    protected override void InitAction()
    {
        _btnAuto.onClick.Set(OnAutoClick);
        _btnConversation.onClick.Set(OnConversationClick);
    }

    protected override void InitListener()
    {
        EventManager.AddListening(id, "start_conversation", StartConversation);
    }

    protected override void OnShow()
    {
        base.OnShow();
        UIManager.Ins().SetFocus(true);
    }

    protected override void OnHide()
    {
        base.OnHide();
        UIManager.Ins().SetFocus(false);
    }

    private void SelectionRenderer(int index, GObject obj)
    {
        GButton btnSelection = obj.asButton;
        GTextField content = btnSelection.GetChildAt(3).asTextField;

        content.text = _selection[index];

        btnSelection.onClick.Set(OnSelectionClick);
    }

    /// <summary>
    /// 开始对话
    /// </summary>
    /// <param name="data"></param>
    private void StartConversation(ArrayList data)
    {
        DialogConfigData config = (DialogConfigData)data[0];
        _curDialog = config;
        RefreshConversation();
        ChangeSelection();
    }

    /// <summary>
    /// 点击自动对话
    /// </summary>
    private void OnAutoClick()
    {
        _isAuto = !_isAuto;

        GTextField content = _btnAuto.GetChildAt(3).asTextField;
        content.text = _isAuto ? _strAuto[1] : _strAuto[0];
    }

    private void OnSelectionClick()
    {
        _curDialog = DialogManager.Ins().Next(_curDialog, _listSelection.selectedIndex);
        bool next = _curDialog != null;
        if (next)
        {
            RefreshConversation();
            ChangeSelection();
        }
        else
        {
            UIManager.Ins().Hide(this);
        }

        if (_isAuto)
        {
            //_dialogCo = StartCoroutine(AutoDialogue());
        }
    }

    private void OnConversationClick()
    {
        if (!_listSelection.visible)
        {
            _curDialog = DialogManager.Ins().Next(_curDialog, 0);
            bool next = _curDialog != null;
            if (next)
            {
                RefreshConversation();
                ChangeSelection();
            }
            else
            {
                UIManager.Ins().Hide(this);
            }

            if (_isAuto)
            {
                //StopCoroutine(_dialogCo);
                //_dialogCo = StartCoroutine(AutoDialogue());
            }
        }
    }

    /// <summary>
    /// 改变选项数据
    /// </summary>
    private void ChangeSelection()
    {
        _selection = _curDialog.selection;
        RefreshSelection();
    }

    /// <summary>
    /// 刷新对话
    /// </summary>
    private void RefreshConversation()
    {
        _textTalker.text = _curDialog.target;
        GTextField content = _btnConversation.GetChildAt(3).asTextField;
        content.text = _curDialog.content;
    }

    /// <summary>
    /// 刷新选项
    /// </summary>
    private void RefreshSelection()
    {
        if (_curDialog.selection.Count > 0)
        {
            _listSelection.visible = true;
            _listSelection.height = _selection.Count * _listSelection.GetChildAt(0).height + _listSelection.lineGap * (_selection.Count - 1);
        }
        else
        {
            _listSelection.visible = false;
        }
    }
}
