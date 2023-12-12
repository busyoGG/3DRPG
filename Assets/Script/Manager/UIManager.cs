using FairyGUI;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class UIManager : Singleton<UIManager>
{
    private Stack<BaseView> _uiViewStack = new Stack<BaseView>();

    private Dictionary<Type, Stack<BaseUIComp>> _uiCompPool = new Dictionary<Type, Stack<BaseUIComp>>();

    private Dictionary<string, BaseView> _uiViewPool = new Dictionary<string, BaseView>();

    private Dictionary<string, bool> _packageState = new Dictionary<string, bool>();

    private Transform _poolTransform;

    private bool _inited = false;

    private int _id = 0;

    private bool _focus = false;

    public class UIPack
    {
        public static string Main = "Main";
        public static string Game = "Game";
    }

    public class UIUrl
    {
        public static string Main = "UI/Main";
        public static string Game = "UI/Game";
    }

    public void Init()
    {
        if (_inited) return;
        //��ʼ��ui���
        var stageCamera = StageCamera.main;
        var cameraData = stageCamera.GetUniversalAdditionalCameraData();
        cameraData.renderType = CameraRenderType.Overlay;
        Camera.main.GetUniversalAdditionalCameraData().cameraStack.Add(stageCamera);

        _poolTransform = new GameObject().transform;
        _poolTransform.gameObject.SetActive(false);
        _inited = true;
    }

    public void SetFocus(bool focus)
    {
        _focus = focus;
        if (_focus)
        {
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    public bool IsFocus()
    {
        return _focus;
    }

    public T Get<T>(string name) where T : BaseView
    {
        foreach (var ui in _uiViewStack)
        {
            if (ui.name == name)
            {
                return ui as T;
            }
        }
        return null;
    }

    /// <summary>
    /// ��ʾui
    /// </summary>
    /// <param name="package"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    public T Show<T>(string name) where T : BaseView, new()
    {
        //�жϰ��Ƿ����
        if (!_packageState.ContainsKey(UIUrl.Main))
        {
            UIPackage.AddPackage(UIUrl.Main);
            _packageState.Add(UIUrl.Main, true);
        }

        //��ȡui
        //T ui = Get<T>(name);
        T ui;
        BaseView baseView;
        _uiViewPool.TryGetValue(name, out baseView);

        if (baseView == null)
        {
            ui = new T();

            ui.main = UIPackage.CreateObject(UIPack.Main, name).asCom;
            ui.name = name;
            ui.id = GetId();
            //��ӵ����
            GRoot.inst.AddChild(ui.main);
            ui.OnAwake();
        }
        else
        {
            ui = baseView as T;
            //return ui;
        }

        ui.Show();

        _uiViewStack.Push(ui);
        //������ʾ�㼶
        ui.main.parent.SetChildIndex(ui.main, ui.main.parent.numChildren - 1);

        //Init();

        return ui;
    }

    /// <summary>
    /// ����ui
    /// </summary>
    /// <param name="ui"></param>
    public void Hide(BaseView ui)
    {
        int hideNum = CalculateHideNum(ui.name);
        for (int i = 0; i < hideNum; i++)
        {
            BaseView uiInStack = _uiViewStack.Pop();
            uiInStack.Hide();
            if (_uiViewPool.ContainsKey(uiInStack.name))
            {
                _uiViewPool[uiInStack.name] = uiInStack;
            }
            else
            {
                _uiViewPool.Add(uiInStack.name, uiInStack);
            }
        }
    }


    //-----���-----

    public T AddUI<T>(string name, Transform parent) where T : BaseUIComp, new()
    {
        //�жϰ��Ƿ����
        if (!_packageState.ContainsKey(UIUrl.Game))
        {
            UIPackage.AddPackage(UIUrl.Game);
            _packageState.Add(UIUrl.Game, true);
        }

        Stack<BaseUIComp> stack;
        _uiCompPool.TryGetValue(typeof(T), out stack);

        if (stack == null)
        {
            stack = new Stack<BaseUIComp>();
            _uiCompPool.Add(typeof(T), stack);
        }

        T ui;
        if (stack.Count > 0)
        {
            ui = stack.Pop() as T;
        }
        else
        {
            ui = new T();
            //ui.main = UIPackage.CreateObject(UIPack.Game, name).asCom;
            ui.name = name;
            GameObject obj = new GameObject();
            UIPanel panel = obj.AddComponent<UIPanel>();
            panel.packageName = UIPack.Game;
            panel.componentName = name;

            //������������ѡ��Ǳ��룬ע��ܶ����Զ�Ҫ��container�����ã�������UIPanel

            //����renderMode�ķ�ʽ
            //panel.container.renderMode = RenderMode.WorldSpace;

            //panel.fitScreen = FitScreen.FitSize;

            //����fairyBatching�ķ�ʽ
            panel.container.fairyBatching = true;

            //����sortingOrder�ķ�ʽ
            panel.SetSortingOrder(1, true);

            //����hitTestMode�ķ�ʽ
            panel.SetHitTestMode(HitTestMode.Default);

            panel.CreateUI();

            ui.panel = panel;
            ui.main = panel.ui;
            ui.target = obj.transform;
            ui.id = GetId();

            //panel.container.size = new Vector2(100,100);
            //panel.ui.scale = GRoot.inst.scale;
            //panel.container.scale = GRoot._inst.scale;
        }
        ui.target.parent = parent;
        //ui.main = panel.ui;
        //panel.ui.AddChild(ui.main);
        ui.OnAwake();

        return ui;
    }

    public void RemoveUI<T>(BaseUIComp ui)
    {
        ui.target.parent = _poolTransform;
        ui.Reset();

        Stack<BaseUIComp> stack;
        _uiCompPool.TryGetValue(typeof(T), out stack);

        if (stack == null)
        {
            stack = new Stack<BaseUIComp>();
            _uiCompPool.Add(typeof(T), stack);
        }

        stack.Push(ui);
    }

    //-----����-----

    /// <summary>
    /// ��������ui������
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    private int CalculateHideNum(string key)
    {
        int i = 0;
        foreach (BaseView ui in _uiViewStack)
        {
            if (ui.name == key)
            {
                break;
            }
            i++;
        }
        return _uiViewStack.Count - i;
    }

    private string GetId()
    {
        return "ui_" + _id++;
    }
}
