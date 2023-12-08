using UnityEngine;
using System.Reflection;
using System;
using UnityEditor;
using System.Collections.Generic;

public class Root : MonoBehaviour
{
    public GameObject root;
    public GameObject pos;
    public GameObject UI;

    public PlayerController player;

    public CameraScript camera;

    private TestWorld _world;

    private PhysicWorld _physicWorld;

    private bool _start;
    // Start is called before the first frame update
    void Awake()
    {
        TimerUtils.Init();
        ECSManager.Ins().Init();
        InputManager.Ins().Init();
        SkillManager.Ins().Init();
        DialogManager.Ins().Init();
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
        _world = new TestWorld();
        _physicWorld = new PhysicWorld();

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

        _world.Update();
        _physicWorld.Update();
    }

    private void FixedUpdate()
    {
    }

    void OnDrawGizmos()
    {
        if (_start)
        {
            _world.DrawGrizmos();
            _physicWorld.DrawGrizmos();
        }
    }
}
