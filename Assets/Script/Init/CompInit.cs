using System;
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
    public bool _isAni = false;
    public bool _isTransform = false;

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

    //-----ani-start-----
    public string _defAni;
    public SerializableDictionary<string, AnimationData> _logicAni;
    //-----ani-end-----

    //-----trigger-start-----

    public TriggerFunction _triggerFunc;

    public bool _isTriggerPositive;
    //-----trigger-end-----

    void Start()
    {
        Bounds bound = GetComponent<BoxCollider>().bounds;

        Entity entity = ECSManager.Ins().CreateEntity();

        bool isRender = false;

        if (transform.GetComponent<MeshRenderer>() || transform.GetComponent<SkinnedMeshRenderer>())
        {
            isRender = true;
        }

        if (isRender)
        {
            RenderComp render = entity.Add<RenderComp>();
            render.node = transform;
        }

        if (_isMove)
        {
            MoveComp move = entity.Add<MoveComp>();
            move.speed = _moveSpeed;
            move.jumpSpeed = _jumpSpeed;
            move.gravity = _gravity;
            move.jumpScale = _jumpScale;
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
            TriggerComp trigger = entity.Add<TriggerComp>();

            TriggerBase script = TriggerFuncInit.Ins().GetTriggerFunc(_triggerFunc);

            trigger.OnTriggerEnter = script.OnTriggerEnter;
            trigger.OnTriggerKeeping = script.OnTriggerKeeping;
            trigger.OnTriggerExit = script.OnTriggerExit;

            trigger.isPositive = _isTriggerPositive;
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


        if (_isTransform)
        {
            TransformComp transformComp = entity.Add<TransformComp>();
            transformComp.position = transform.position;
            transformComp.rotation = transform.rotation;
            transformComp.scale = transform.localScale;
        }

        if (_isAni)
        {
            AniComp ani = entity.Add<AniComp>();
            ani.curAni = _defAni;
            ani.animator = transform.GetComponent<Animator>();

            LogicAniComp logicAni = entity.Add<LogicAniComp>();

            //OBBData obb = new OBBData();
            //obb.position = transform.position;
            //obb.size = transform.localScale;
            //obb.axes = new Vector3[3]
            //{
            //    transform.right,
            //    transform.up,
            //    transform.forward,
            //};
            //obb.rot = transform.rotation;

            //logicAni.aniBox.Add(("", obb));
            List<Trans> parentVec3 = new List<Trans>();
            InitChild(transform, ref logicAni.aniBox, _logicAni, ref logicAni.aniClips, parentVec3);
        }
    }

    private void InitChild(Transform parent, ref List<(string, OBBData)> aniBox, SerializableDictionary<string, AnimationData> aniDic,
        ref Dictionary<string, Dictionary<string, List<List<Vector3>>>> aniData, List<Trans> parentVec3, int depth = 1)
    {
        for (int i = 0; i < parent.childCount; i++)
        {
            Transform child = parent.GetChild(i);
            string path = depth == 1 ? child.name : parent.name + "/" + child.name;
            //保存包围盒
            OBBData obb = new OBBData();
            obb.position = child.position;
            obb.size = child.localScale;
            obb.axes = new Vector3[3]
            {
                child.right,
                child.up,
                child.forward,
            };
            obb.rot = child.rotation;

            aniBox.Add((path, obb));

            List<Trans> vec3 = null;
            //计算坐标
            foreach (var data in aniDic)
            {
                string aniName = data.Key;
                AnimationData ani = data.Value;

                Dictionary<string, List<List<Vector3>>> childDic;
                aniData.TryGetValue(aniName, out childDic);

                if (childDic == null)
                {
                    childDic = new Dictionary<string, List<List<Vector3>>>();
                    aniData.Add(aniName, childDic);
                }

                if (!childDic.ContainsKey(path))
                {
                    ani.transforms.TryGetValue(path, out vec3);

                    if (vec3 != null)
                    {
                        List<List<Vector3>> res = new List<List<Vector3>>();
                        for (int j = 0; j < vec3.Count; j++)
                        {
                            res.Add(new List<Vector3>());
                            //for (int k = 0; k < vec3[j].Count; k++)
                            //{
                            //    int index = k;
                            //    if (k > parentVec3[j].Count - 1)
                            //    {
                            //        index = parentVec3[j].Count - 1;
                            //    }
                            //    res[j][k] = parentVec3[j][index] + vec3[j][k];
                            //}
                            if (parentVec3.Count > 0)
                            {
                                res[j].Add(parentVec3[j].position + vec3[j].position);
                                res[j].Add(parentVec3[j].euler + vec3[j].euler);
                                res[j].Add(parentVec3[j].scale + vec3[j].scale);
                            }
                            else
                            {
                                res[j].Add(vec3[j].position);
                                res[j].Add(vec3[j].euler);
                                res[j].Add(vec3[j].scale);
                            }

                        }
                        childDic.Add(path, res);
                    }
                }
            }

            InitChild(child, ref aniBox, aniDic, ref aniData, vec3,depth + 1);
        }
    }
}
