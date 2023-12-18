using UnityEngine;
using System.Reflection;
using System;
using UnityEditor;
using System.Collections.Generic;
using LitJson;
using Bean;
using UnityEngine.Rendering.Universal;

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

    //private void TestType<T>(IBaseTest<T> insTest)
    //{
    //    ConsoleUtils.Log("����ibasetype");
    //}

    // Start is called before the first frame update
    void Awake()
    {
        Global.Cam = camera;

        PoolManager.Ins().Init();
        TimerUtils.Init();
        ECSManager.Ins().Init();
        InputManager.Ins().Init();
        SkillManager.Ins().Init();
        DialogManager.Ins().Init();
        MissionManager.Ins().Init();
        PropManager.Ins().Init();   
        //InputManager.Ins().AddKeyboardInputCallback(KeyCode.Space,InputStatus.Up, () =>
        //{
        //    ConsoleUtils.Log("�����˿ո�");
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
#if UNITY_EDITOR
        EditorApplication.playModeStateChanged += (PlayModeStateChange state) =>
        {
            //ConsoleUtils.Log("����״̬�ı�",state);
            if (state == PlayModeStateChange.ExitingPlayMode)
            {
                TimerUtils.Stop();
            }
        };
#endif

        Test();
    }

    private void Test()
    {
        //test
        ConsoleUtils.Log("��ʼ�ȴ�", DateTime.Now);
        TimerChain chain = null;
        chain = TimerUtils.Loop(1000, () =>
        {
            ConsoleUtils.Log("���Եȴ�ѭ��", DateTime.Now);
        }, 0, 3).Once(5000, () =>
        {
            ConsoleUtils.Log("���Եȴ�", DateTime.Now);
        });

        TimerUtils.Once(2100, () =>
        {
            ConsoleUtils.Log("����ֹͣ", DateTime.Now, chain.GetId());
            TimerUtils.Clear(chain);
        });

        //JsonReader jr = new JsonReader("2");
        //MissionConfigData data = JsonMapper.ToObject<MissionConfigData>("{ 'filter':2 }");
        //ConsoleUtils.Log("����ö��", data);
        //ConsoleUtils.Log("����ö��", JsonMapper.ToObject<MissionFilter>("2"));

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
            _logicWorld.DrawGrizmos();
            _physicWorld.DrawGrizmos();
            _renderWorld.DrawGrizmos();
        }
    }
}
