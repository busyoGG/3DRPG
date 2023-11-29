using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class CompInit : MonoBehaviour
{
    public bool _isMainCharacter = false;
    public bool _isMove = false;
    public bool _isJump = false;
    public bool _isCollide = false;
    public bool _isTrigger = false;
    public bool _isQTree = false;
    public bool _isClimb = false;
    public bool _isSkill = false;

    //-----move-start-----
    /// <summary>
    /// 移动速度
    /// </summary>
    public float _moveSpeed = 0.1f;
    /// <summary>
    /// 跳跃初速度
    /// </summary>
    public float _jumpSpeed = 1f;
    /// <summary>
    /// 重力
    /// </summary>
    public float _gravity = 9.8f;
    /// <summary>
    /// 跳跃过程倍率
    /// </summary>
    public float _jumpScale = 1f;
    //-----move-end-----

    //-----collide-start-----
    public CollisionType _collideType = CollisionType.AABB;

    public bool _isStatic = true;
    //-----collide-end-----

    //-----qtree-start-----
    public bool _isCustomAABB = false;

    public Vector3 _aabbSize = Vector3.one;
    //-----qtree-end-----

    //-----main_character-start-----

    public CameraScript _cameraScript;

    //-----main_character-end-----

    //-----skill-start-----
    public SerializableDictionary<InputKey, int> _skillMap;
    //-----skill-end-----

    void Start()
    {
        Bounds bound = GetComponent<BoxCollider>().bounds;

        Entity entity = ECSManager.Ins().CreateEntity();

        RenderComp render = entity.Add<RenderComp>();
        render.node = transform;

        if (_isMove)
        {
            MoveComp move = entity.Add<MoveComp>();
            move.speed = _moveSpeed;
            move.jumpSpeed = _jumpSpeed;
            move.gravity = _gravity;
            move.jumpScale = _jumpScale;
            move.lastPosition = transform.position;
            move.nextPostition = transform.position;
        }

        if (_isJump)
        {
            entity.Add<JumpComp>();
        }

        if (_isCollide)
        {
            CollideComp collide = entity.Add<CollideComp>();
            collide.type = _collideType;
            BoxCollider box = GetComponent<BoxCollider>();
            switch (_collideType)
            {
                case CollisionType.AABB:
                    AABBData aabb = new AABBData();
                    aabb.position = bound.center;
                    aabb.size = bound.size;
                    collide.aabb = aabb;
                    collide.position = aabb.position;
                    break;
                case CollisionType.OBB:
                    OBBData obb = new OBBData();
                    obb.position = transform.position;
                    obb.size = transform.localScale;
                    obb.axes = new Vector3[3]
                    {
                        transform.right,
                        transform.up,
                        transform.forward,
                    };
                    obb.rot = transform.rotation;
                    collide.obb = obb;
                    collide.position = obb.position;
                    break;
            }
            collide.isStatic = _isStatic;
        }

        if (_isTrigger)
        {
            entity.Add<TriggerComp>();
        }

        if (_isQTree)
        {
            QTreeComp qtree = entity.Add<QTreeComp>();

            AABBData aabb;
            if (_isCustomAABB)
            {
                aabb = QtreeManager.Ins().CreateBounds(bound.center, _aabbSize);
            }
            else
            {
                aabb = QtreeManager.Ins().CreateBounds(bound.center, bound.size);
            }

            qtree.qObj = QtreeManager.Ins().CreateQtreeObj(aabb, entity);
            qtree.qNode = QtreeManager.Ins().Insert(MapManager.Ins().GetIndex(transform.position), qtree.qObj);
        }

        if (_isMainCharacter)
        {
            PlayerController player = this.AddComponent<PlayerController>();
            player.cam = _cameraScript;
            player.player = entity;
        }

        if (_isClimb)
        {
            entity.Add<ClimbComp>();
        }

        if (_isSkill)
        {
            SkillComp skill = entity.Add<SkillComp>();
            skill.id = _skillMap.ToDictionary();
        }
    }

}
