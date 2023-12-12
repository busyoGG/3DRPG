using UnityEngine;
using System.Reflection;
using System;
using UnityEditor;
using System.Collections.Generic;
using LitJson;
using Bean;

public class Root : MonoBehaviour
{
    public GameObject root;
    public GameObject pos;
    public GameObject UI;

    public PlayerController player;

    public CameraScript camera;

    private LogicWorld _logicWorld;

    private PhysicWorld _physicWorld;

    private RenderWorld _renderWorld;

    private bool _start;
    // Start is called before the first frame update
    void Awake()
    {
        TimerUtils.Init();
        ECSManager.Ins().Init();
        InputManager.Ins().Init();
        SkillManager.Ins().Init();
        DialogManager.Ins().Init();
        MissionManager.Ins().Init();
        //InputManager.Ins().AddKeyboardInputCallback(KeyCode.Space,InputStatus.Up, () =>
        //{
        //    ConsoleUtils.Log("按下了空格");
        //});

        MapManager.Ins().Init();
        MapManager.Ins().CalculateMap(root);
        //Thread t = new Thread(() =>
        //{
        //    while (true)
        //    {
        //        MapManager.Ins().Update();
        //    }
        //});
        //t.Start();
        _logicWorld = new LogicWorld();
        _physicWorld = new PhysicWorld();
        _renderWorld = new RenderWorld();

        _start = true;

        //UIManager.Ins().Show<>("Root");

        //UIManager.Ins().AddUI<TriggerButtonUI>("TriggerButton", UI.transform);
        EditorApplication.playModeStateChanged += (PlayModeStateChange state) =>
        {
            //ConsoleUtils.Log("运行状态改变",state);
            if (state == PlayModeStateChange.ExitingPlayMode)
            {
                TimerUtils.Stop();
            }
        };


        Test();
    }

    private void Test()
    {
        //test
        ConsoleUtils.Log("开始等待", DateTime.Now);
        TimerChain chain = null;
        chain = TimerUtils.Loop(1000, () =>
        {
            ConsoleUtils.Log("测试等待循环", DateTime.Now);
        }, 0, 3).Once(5000, () =>
        {
            ConsoleUtils.Log("测试等待", DateTime.Now);
        });

        TimerUtils.Once(2100, () =>
        {
            ConsoleUtils.Log("测试停止", DateTime.Now, chain.GetId());
            TimerUtils.Clear(chain);
        });

        //JsonReader jr = new JsonReader("2");
        //MissionConfigData data = JsonMapper.ToObject<MissionConfigData>("{ 'filter':2 }");
        //ConsoleUtils.Log("测试枚举", data);
        //ConsoleUtils.Log("测试枚举", JsonMapper.ToObject<MissionFilter>("2"));
    }

    private void Start()
    {

        UIManager.Ins().Init();
    }

    // Update is called once per frame
    void Update()
    {
        //Application.targetFrameRate = 60;
        MapManager.Ins().RefreshPlayerPos(pos.transform.position);
        MapManager.Ins().Update();
        MapManager.Ins().RefreshChunk();

        _renderWorld.Update();
    }

    private void FixedUpdate()
    {
        _logicWorld.Update();
        _physicWorld.Update();
    }

    void OnDrawGizmos()
    {
        if (_start)
        {
            //_logicWorld.DrawGrizmos();
            //_physicWorld.DrawGrizmos();
            //_renderWorld.DrawGrizmos();
        }
    }
}
