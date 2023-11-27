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

        //TransformComp comp = ECSManager.Ins().CreateComp(typeof(TransformComp)) as TransformComp;
        //ConsoleUtils.Log("组件 id",comp.compId);
        //RenderComp comp2 = ECSManager.Ins().CreateComp(typeof(RenderComp)) as RenderComp;
        //ConsoleUtils.Log("组件2 id", comp2.compId);

        player.camera = camera;
        player.transform.eulerAngles = new Vector3(0, 45, 0);

        Entity entity = ECSManager.Ins().CreateEntity();
        entity.Add<MoveComp>();
        entity.Add<RenderComp>();
        entity.Add<JumpComp>();

        CollideComp collidePlayer = entity.Add<CollideComp>();
        collidePlayer.type = CollisionType.AABB;

        OBBData obbPlayer = new OBBData();
        obbPlayer.position = player.transform.position;
        obbPlayer.size = player.transform.localScale;
        obbPlayer.axes = new Vector3[]
        {
            player.transform.right,
            player.transform.up,
            player.transform.forward
        };

        ConsoleUtils.Log(obbPlayer.vertexts);

        collidePlayer.obb = obbPlayer;

        AABBData aabbPlayer = new AABBData();
        aabbPlayer.position = player.transform.position;
        aabbPlayer.size = Vector3.one;
        collidePlayer.aabb = aabbPlayer;

        collidePlayer.isStatic = false;
        collidePlayer.position = player.transform.position;

        ConsoleUtils.Log(new Vector3(1, 0, 1).normalized);

        player.player = entity;

        QTreeComp qtreePlayer = entity.Add<QTreeComp>();

        qtreePlayer.qObj = QtreeManager.Ins().CreateQtreeObj(aabbPlayer, entity);
        qtreePlayer.qNode = QtreeManager.Ins().Insert(MapManager.Ins().GetIndex(player.transform.position), qtreePlayer.qObj);

        TriggerComp triggerPlayer = entity.Add<TriggerComp>();
        triggerPlayer.isPositive = true;

        //创建障碍物
        Entity entityCube = ECSManager.Ins().CreateEntity();
        //CollideComp collideCube = entityCube.Add<CollideComp>();

        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cube.transform.position = new Vector3(2, 1.5f, 2);
        cube.transform.localScale = new Vector3(1, 2, 1);

        //collideCube.position = cube.transform.position;
        //collideCube.type = CollisionType.AABB;

        AABBData aabbCube = new AABBData();
        aabbCube.position = cube.transform.position;
        aabbCube.size = cube.transform.localScale;

        //collideCube.aabb = aabbCube;

        QTreeComp qtreeCube = entityCube.Add<QTreeComp>();

        qtreeCube.qObj = QtreeManager.Ins().CreateQtreeObj(aabbCube, entityCube);
        qtreeCube.qNode = QtreeManager.Ins().Insert(MapManager.Ins().GetIndex(cube.transform.position), qtreeCube.qObj);

        TriggerComp triggerCube = entityCube.Add<TriggerComp>();
        triggerCube.OnTriggerEnter = (Entity self, Entity other) =>
        {
            ConsoleUtils.Log("进入trigger");
        };

        triggerCube.OnTriggerExit = (Entity self, Entity other) =>
        {
            ConsoleUtils.Log("退出trigger");
        };

        triggerCube.OnTriggerKeeping = (Entity self, Entity other) =>
        {
            ConsoleUtils.Log("持续trigger");
        };

        //-----
        Entity entityCube3 = ECSManager.Ins().CreateEntity();
        CollideComp collideCube3 = entityCube3.Add<CollideComp>();

        GameObject cube3 = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cube3.transform.position = new Vector3(3, 1.5f, 2);
        cube3.transform.localScale = new Vector3(1, 2, 1);

        collideCube3.position = cube.transform.position;
        collideCube3.type = CollisionType.AABB;

        AABBData aabbCube3 = new AABBData();
        aabbCube3.position = cube3.transform.position;
        aabbCube3.size = cube3.transform.localScale;

        collideCube3.aabb = aabbCube3;

        QTreeComp qtreeCube3 = entityCube3.Add<QTreeComp>();

        qtreeCube3.qObj = QtreeManager.Ins().CreateQtreeObj(aabbCube3, entityCube3);
        qtreeCube3.qNode = QtreeManager.Ins().Insert(MapManager.Ins().GetIndex(cube3.transform.position), qtreeCube3.qObj);

        //-----

        Entity entityCube2 = ECSManager.Ins().CreateEntity();
        CollideComp collideCube2 = entityCube2.Add<CollideComp>();

        GameObject cube2 = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cube2.transform.position = new Vector3(6, 1f, 1);
        cube2.transform.localScale = new Vector3(4, 4, 4);

        cube2.transform.eulerAngles = new Vector3(-60, 45, 0);

        collideCube2.position = cube.transform.position;
        collideCube2.type = CollisionType.OBB;

        AABBData aabbCube2 = new AABBData();
        aabbCube2.position = cube2.transform.position;
        aabbCube2.size = cube2.transform.localScale * 2;

        //collideCube2.aabb = aabbCube2;

        OBBData obbCube2 = new OBBData();
        obbCube2.position = cube2.transform.position;
        obbCube2.axes = new Vector3[3] {
            cube2.transform.right,
            cube2.transform.up,
            cube2.transform.forward
        };
        obbCube2.size = cube2.transform.localScale;

        ConsoleUtils.Log(obbCube2.vertexts);

        collideCube2.obb = obbCube2;

        QTreeComp qtreeCube2 = entityCube2.Add<QTreeComp>();

        qtreeCube2.qObj = QtreeManager.Ins().CreateQtreeObj(aabbCube2, entityCube2);
        qtreeCube2.qNode = QtreeManager.Ins().Insert(MapManager.Ins().GetIndex(cube2.transform.position), qtreeCube2.qObj);

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
