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

    private TestWorld world;

    private bool _start;
    // Start is called before the first frame update
    void Awake()
    {
        ECSManager.Ins().Init();
        InputManager.Ins().Init();
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
        world = new TestWorld();

        _start = true;
    }

    // Update is called once per frame
    void Update()
    {
        MapManager.Ins().RefreshPlayerPos(pos.transform.position);
        MapManager.Ins().Update();
        MapManager.Ins().RefreshChunk();

        world.Update();
    }

    void OnDrawGizmos()
    {
        if (_start)
        {
            world.DrawGrizmos();
        }
    }
}
