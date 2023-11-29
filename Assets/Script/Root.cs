using Game;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Root : MonoBehaviour
{
    public GameObject root;
    public GameObject pos;

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
        _world = new TestWorld();
        _physicWorld = new PhysicWorld();

        _start = true;

        ConsoleUtils.Log("����key", InputKey.None == InputKey.MouseLeft);
    }

    // Update is called once per frame
    void Update()
    {
        MapManager.Ins().RefreshPlayerPos(pos.transform.position);
        MapManager.Ins().Update();
        MapManager.Ins().RefreshChunk();

        _world.Update();
        _physicWorld.Update();
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
