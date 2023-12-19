using System;
using UnityEditor;
using UnityEngine;

public class Launcher : MonoBehaviour
{
    private bool _inited = false;

    private int _initIndex = 0;

    private LogicWorld _logicWorld;

    private PhysicWorld _physicWorld;

    private RenderWorld _renderWorld;

    void Start()
    {
        InitNext();
    }

    private void Update()
    {
        if (_inited)
        {
            MapManager.Ins().RefreshPlayerPos(Global.Controller.transform.position);
            MapManager.Ins().Update();
            MapManager.Ins().RefreshChunk();

            _renderWorld.Update();
        }
    }

    private void FixedUpdate()
    {
        if (_inited)
        {
            _logicWorld.Update();
            _physicWorld.Update();
        }
    }

    void OnDrawGizmos()
    {
        if (_inited)
        {
            _logicWorld.DrawGrizmos();
            _physicWorld.DrawGrizmos();
            _renderWorld.DrawGrizmos();
        }
    }

    private void InitNext()
    {
        switch (_initIndex++)
        {
            case 0:
                InitUtils();
                break;
            case 1:
                LoadRes();
                break;
            case 2:
                InitManager();
                break;
            case 3:
                StartGame();
                break;
            case 4:
                InitGlobals();
                break;
            case 5:
                _inited = true;
                break;
        }
    }

    private void LoadRes()
    {
        string[] configs = FileUtils.GetFolderFiles("Configs");
        string[] mats = FileUtils.GetFolderFiles("Mats");
        string[] animator = FileUtils.GetFolderFiles("Animator");
        string[] scriptableObj = FileUtils.GetFolderFiles("ScriptableObject");
        string[] animation = FileUtils.GetFolderFiles("Animation");

        int total = configs.Length + mats.Length + animator.Length + scriptableObj.Length + animation.Length;

        ConsoleUtils.Log(configs, mats, animator, scriptableObj, animation, "一共", total);

        ResourceUtils.Load<TextAsset>(configs);
        ResourceUtils.Load<Material>(mats);
        ResourceUtils.Load<Animator>(animator);
        ResourceUtils.Load<ScriptableObject>(scriptableObj);
        ResourceUtils.Load<Animation>(animation);

        ConsoleUtils.Log("测试获取", ResourceUtils.GetRes<TextAsset>(configs[0]));

        TimerChain timer = null;

        Action CheckEnd = () =>
        {
            int num = ResourceUtils.GetCacheNum();
            ConsoleUtils.Log("加载进度", num / (float)total);
            if (num >= total)
            {
                ConsoleUtils.Log("资源加载完成");
                TimerUtils.Clear(timer);
                InitNext();
            }
        };

        timer = TimerUtils.Loop(100, CheckEnd);

    }

    private void InitGlobals()
    {
        TimerChain timer = null;

        Action Finding = () =>
        {

            Global.Cam = Camera.main.GetComponent<CameraScript>();
            Global.Controller = transform.GetChild(1).Find("Player").GetComponent<PlayerController>();

            if (Global.Cam != null && Global.Controller != null)
            {
                TimerUtils.Clear(timer);
                InitNext();
            }
        };

        timer = TimerUtils.Loop(100, Finding);
    }

    private void InitUtils()
    {
        TimerUtils.Init();
        InitNext();
    }

    private void InitManager()
    {
        PoolManager.Ins().Init();
        ECSManager.Ins().Init();
        InputManager.Ins().Init();
        SkillManager.Ins().Init();
        DialogManager.Ins().Init();
        MissionManager.Ins().Init();
        PropManager.Ins().Init();
        MapManager.Ins().Init();
        UIManager.Ins().Init();
        InitNext();
    }

    private void StartGame()
    {
        MapManager.Ins().CalculateMap(transform.GetChild(0).gameObject);
        _logicWorld = new LogicWorld();
        _physicWorld = new PhysicWorld();
        _renderWorld = new RenderWorld();

        transform.GetChild(1).gameObject.SetActive(true);

#if UNITY_EDITOR
        EditorApplication.playModeStateChanged += (PlayModeStateChange state) =>
        {
            //ConsoleUtils.Log("运行状态改变",state);
            if (state == PlayModeStateChange.ExitingPlayMode)
            {
                TimerUtils.Stop();
            }
        };
#endif
        InitNext();
    }
}
