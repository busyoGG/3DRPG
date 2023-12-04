using UnityEngine;
using System.Reflection;
using System;

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
        ECSManager.Ins().Init();
        InputManager.Ins().Init();
        SkillManager.Ins().Init();
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

        UIManager.Ins().Init();
        //UIManager.Ins().Show<>("Root");

        //UIManager.Ins().AddUI<TriggerButtonUI>("TriggerButton", UI.transform);
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
