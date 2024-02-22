using Bean;
using FairyGUI;
using System.Collections;
using System.Collections.Generic;

public class ConversationView : BaseView
{
    [UICompBind("List", "3")]
    private GList _listSelection { get; set; }

    //private GTextField _textTalker;

    //private GButton _btnAuto;

    //private GButton _btnConversation;

    //-----��������-----

    [UIDataBind("List", "3")]
    private UIListProp<string> _selection { get; set; }
    [UIDataBind("TextField", "4/3")]
    private UIProp _autoBtnContent { get; set; }
    [UIDataBind("TextField", "2")]
    private UIProp _talker { get; set; }
    [UIDataBind("TextField", "0/3")]
    private UIProp _content {  get; set; }

    private string[] _strAuto = new string[2] { "�Զ�", "�Զ���" };

    private bool _isAuto = false;

    private int _selectIndex = 0;

    private DialogConfigData _curDialog;

    private TimerChain _timer;

    protected override void OnShow()
    {
        UIManager.Ins().SetFocus(true);
    }

    protected override void OnHide()
    {
        UIManager.Ins().SetFocus(false);
    }

    /// <summary>
    /// ��ʼ�Ի�
    /// </summary>
    /// <param name="index"></param>
    /// <param name="obj"></param>
    [UIActionBind("ListRender", "3")]
    private void SelectionRenderer(int index, GObject obj)
    {
        GButton btnSelection = obj.asButton;
        GTextField content = btnSelection.GetChildAt(3).asTextField;

        content.text = _selection.Get()[index];

        btnSelection.onClick.Set(OnSelectionClick);
    }

    /// <summary>
    /// ��ʼ�Ի�
    /// </summary>
    /// <param name="data"></param>
    [UIListenerBind("start_conversation")]
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
    [UIActionBind("Click","4")]
    private void OnAutoClick()
    {
        _isAuto = !_isAuto;

        _autoBtnContent.Set(_isAuto ? _strAuto[1] : _strAuto[0]);

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
        OnClickDialog();

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
    private void OnClickDialog()
    {
        _curDialog = DialogManager.Ins().Next(_curDialog, _selectIndex);
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
    private void OnSelectionClick(EventContext context)
    {
        GButton button = (GButton)context.sender;
        _listSelection.selectedIndex = _listSelection.GetChildIndex(button);
        _selectIndex = _listSelection.selectedIndex;

        if (_isAuto)
        {
            StartAuto();
        }
        else
        {
            OnClickDialog();
        }
    }

    /// <summary>
    /// ����Ի�
    /// </summary>
    [UIActionBind("Click", "0")]
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
                OnClickDialog();
            }
        }
    }

    /// <summary>
    /// �ı�ѡ������
    /// </summary>
    private void ChangeSelection()
    {
        _selectIndex = 0;
        _selection.Set(_curDialog.selection);
        RefreshSelection();
    }

    /// <summary>
    /// ˢ�¶Ի�
    /// </summary>
    private void RefreshConversation()
    {
        _talker.Set(_curDialog.target);
        _content.Set(_curDialog.content);
    }

    /// <summary>
    /// ˢ��ѡ��
    /// </summary>
    private void RefreshSelection()
    {
        _listSelection.numItems = _selection.Count;
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
