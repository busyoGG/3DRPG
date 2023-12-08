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

    //-----��������-----

    private List<string> _selection = new List<string>();

    private string[] _strAuto = new string[2] { "�Զ�", "�Զ���" };

    private bool _isAuto = false;

    private DialogConfigData _curDialog;

    private TimerChain _timer;

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
        UIManager.Ins().SetFocus(true);
    }

    protected override void OnHide()
    {
        ConsoleUtils.Log("�رնԻ�");
        UIManager.Ins().SetFocus(false);
    }

    /// <summary>
    /// ��ʼ�Ի�
    /// </summary>
    /// <param name="index"></param>
    /// <param name="obj"></param>
    private void SelectionRenderer(int index, GObject obj)
    {
        GButton btnSelection = obj.asButton;
        GTextField content = btnSelection.GetChildAt(3).asTextField;

        content.text = _selection[index];

        btnSelection.onClick.Set(OnSelectionClick);
    }

    /// <summary>
    /// ��ʼ�Ի�
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
    /// ����Զ��Ի�
    /// </summary>
    private void OnAutoClick()
    {
        _isAuto = !_isAuto;

        GTextField content = _btnAuto.GetChildAt(3).asTextField;
        content.text = _isAuto ? _strAuto[1] : _strAuto[0];

        if (_isAuto)
        {
            StartAuto();
        }
        else
        {
            StopAuto();
        }
    }

    /// <summary>
    /// �Զ�����
    /// </summary>
    private void OnAutoDialog()
    {
        OnClickDialog(0);

        if (_isAuto && _curDialog != null && _curDialog.selection.Count == 0)
        {
            if (_timer == null)
            {
                _timer = TimerUtils.Once((int)(_curDialog.autoSpeed * 1000), OnAutoDialog);
            }
            else
            {
                _timer.Once((int)(_curDialog.autoSpeed * 1000), OnAutoDialog);
            }
        }
        else
        {
            StopAuto();
        }
    }

    /// <summary>
    /// �ֶ�����
    /// </summary>
    /// <param name="selection"></param>
    private void OnClickDialog(int selection)
    {
        _curDialog = DialogManager.Ins().Next(_curDialog, selection);
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
    }

    /// <summary>
    /// ��ʼ�Զ�
    /// </summary>
    private void StartAuto()
    {
        if (_timer == null)
        {
            _timer = TimerUtils.Once((int)(_curDialog.autoSpeed * 1000), OnAutoDialog);
        }
        else
        {
            _timer.Once((int)(_curDialog.autoSpeed * 1000), OnAutoDialog);
        }
    }

    private void StopAuto()
    {
        if (_timer != null)
        {
            _timer.Clear();
            _timer = null;
        }
    }

    /// <summary>
    /// ���ѡ��
    /// </summary>
    private void OnSelectionClick()
    {
        if (_isAuto)
        {
            StartAuto();
        }
        else
        {
            OnClickDialog(_listSelection.selectedIndex);
        }
    }

    /// <summary>
    /// ����Ի�
    /// </summary>
    private void OnConversationClick()
    {
        if (!_listSelection.visible)
        {
            if (_isAuto)
            {
                StartAuto();
            }
            else
            {
                OnClickDialog(0);
            }
        }
    }

    /// <summary>
    /// �ı�ѡ������
    /// </summary>
    private void ChangeSelection()
    {
        _selection = _curDialog.selection;
        RefreshSelection();
    }

    /// <summary>
    /// ˢ�¶Ի�
    /// </summary>
    private void RefreshConversation()
    {
        _textTalker.text = _curDialog.target;
        GTextField content = _btnConversation.GetChildAt(3).asTextField;
        content.text = _curDialog.content;
    }

    /// <summary>
    /// ˢ��ѡ��
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
