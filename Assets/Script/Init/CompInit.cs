using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

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
    public bool _isDialog = false;
    public bool _isAttack = false;
    public bool _isWeapon = false;
    public bool _isRole = false;
    public bool _isHealthBar = false;

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

    //-----box-start-----
    public CollisionType _boxType = CollisionType.AABB;
    /// <summary>
    /// 是否自定义包围盒
    /// </summary>
    public bool _isCustomBox = false;

    public Vector3 _boxOffset = Vector3.zero;

    public Vector3 _boxSize = Vector3.one;
    //-----box-end-----

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
    public GameObject _logicAniRoot;
    //-----ani-end-----

    //-----trigger-start-----

    public List<TriggerFunction> _triggerFunc;

    public bool _isTriggerPositive;
    //-----trigger-end-----

    //-----dialog-start-----
    public List<int> _randomId;

    //public float _maxDelta;

    //public float _minDelta;

    public string _targetName;
    //-----dialog-end-----

    //-----attack-start-----
    public int _group;
    //-----attack-end-----

    //-----role-start-----
    public int _hp;
    public int _mp;
    public int _shield;
    public int _attack;
    //-----role-end-----

    private Entity _entity;

    void Start()
    {
        if (_logicAniRoot != null)
        {
            StartCoroutine(LateInit());
        }
        else
        {
            Init();
        }
    }

    private IEnumerator LateInit()
    {
        yield return new WaitForSeconds(0.1f);
        Init();
    }

    private void Init()
    {
        Bounds bound = GetComponent<BoxCollider>().bounds;

        Entity entity = ECSManager.Ins().CreateEntity();
        _entity = entity;

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
            //collide.isStatic = _isStatic;
        }

        if (_isTrigger)
        {
            TriggerComp trigger = entity.Add<TriggerComp>();

            foreach (var data in _triggerFunc)
            {
                TriggerBase script = TriggerFuncInit.Ins().GetTriggerFunc(data);

                trigger.OnTriggerEnter.Add(data, script.OnTriggerEnter);
                trigger.OnTriggerKeeping.Add(data, script.OnTriggerKeeping);
                trigger.OnTriggerExit.Add(data, script.OnTriggerExit);

                trigger.triggerFunc.Add(data);
            }

            trigger.isPositive = _isTriggerPositive;
        }

        if (_isTrigger || _isCollide)
        {
            BoxComp box = entity.Add<BoxComp>();
            box.type = _boxType;
            switch (_boxType)
            {
                case CollisionType.AABB:
                    AABBData aabb = new AABBData();
                    if (_isCustomBox)
                    {
                        aabb.position = bound.center + _boxOffset;
                        aabb.size = _boxSize;
                    }
                    else
                    {
                        aabb.position = bound.center;
                        aabb.size = bound.size;
                    }
                    box.aabb = aabb;
                    box.position = aabb.position;
                    break;
                case CollisionType.OBB:
                    OBBData obb = new OBBData();
                    if (_isCustomBox)
                    {
                        obb.position = transform.position + _boxOffset;
                        obb.size = _boxSize;
                    }
                    else
                    {
                        obb.position = transform.position;
                        obb.size = transform.localScale;
                    }
                    obb.axes = new Vector3[3]
                    {
                        transform.right,
                        transform.up,
                        transform.forward,
                    };
                    obb.rot = transform.rotation;
                    box.obb = obb;
                    box.position = obb.position;
                    break;
            }
            box.isPositive = _isTriggerPositive;
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
            player.player = entity;
        }

        if (_isRole)
        {
            //PropComp prop = entity.Add<PropComp>();
            PropData prop = AttackSingleton.Ins().GetPropData(entity.id);
            prop.hp = _hp;
            prop.mp = _mp;
            prop.curHp = _hp;
            prop.curMp = _mp;
            prop.attack = _attack;
            //prop.sheild = _shield;
            //prop.group = _group;
            //prop.hp = _hp;
            //prop.mp = _mp;
            //prop.sheild = _shield;
            //prop.group = _group;
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
            //ani.curAni = _defAni;
            ani.animator = transform.GetComponent<Animator>();


        }

        if (_logicAniRoot != null)
        {
            LogicAniComp logicAni = entity.Add<LogicAniComp>();
            InitLogicAni(_logicAniRoot, logicAni);
        }

        if (_isAttack)
        {
            AttackComp attackComp = entity.Add<AttackComp>();
            attackComp.group = _group;
            if (_logicAniRoot)
            {
                attackComp.entityId = _logicAniRoot.GetComponent<CompInit>()._entity.id;
            }
            else
            {
                attackComp.entityId = entity.id;
            }
        }

        if (_isWeapon)
        {
            WeaponComp weapon = entity.Add<WeaponComp>();
            weapon.entityId = _logicAniRoot.GetComponent<CompInit>()._entity.id;
        }

        if (_isDialog)
        {
            DialogComp dialog = entity.Add<DialogComp>();
            dialog.name = _targetName;
            dialog.randomIds = _randomId;
        }

        if (_isHealthBar)
        {
            entity.Add<HealthBarComp>();
        }
    }

    private void InitLogicAni(GameObject root, LogicAniComp logicAniComp)
    {
        Vector3 vecMax = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);

        CompInit rootInit = root.GetComponent<CompInit>();

        SerializableDictionary<string, AnimationData> logicAni = rootInit._logicAni;

        string path = string.Empty;
        List<string> paths = new List<string>() { "" };

        List<string> names = new List<string>();

        GameObject obj = gameObject;
        while (obj != root)
        {
            //path = "/" + obj.name;
            names.Insert(0, obj.name);
            obj = gameObject.transform.parent.gameObject;
        }

        for (int i = 0; i < names.Count; i++)
        {
            if (i == 0)
            {
                path = names[i];
            }
            else
            {
                path += "/" + names[i];
            }
            paths.Add(path);
        }

        Dictionary<string, List<List<Vector3>>> aniClips = new Dictionary<string, List<List<Vector3>>>();
        Dictionary<string, bool> loopStatus = new Dictionary<string, bool>();

        foreach (var data in logicAni)
        {
            string key = data.Key;
            AnimationData aniClip = data.Value;

            aniClips.Add(key, new List<List<Vector3>>());
            loopStatus.Add(key, aniClip.isLoop);

            List<Vector3> resPos = new List<Vector3>();
            List<Vector3> resEuler = new List<Vector3>();
            List<Vector3> resScale = new List<Vector3>();

            aniClips[key].Add(resPos);
            aniClips[key].Add(resEuler);
            aniClips[key].Add(resScale);

            for (int i = 0; i < paths.Count; i++)
            {
                List<Trans> trans;
                aniClip.transforms.TryGetValue(paths[i], out trans);

                if (i == 0)
                {
                    if (trans == null)
                    {
                        for (int j = 0; j < aniClip.transforms[aniClip.transforms.Keys.ToList()[0]].Count; j++)
                        {
                            resPos.Add(Vector3.zero);
                            resEuler.Add(Vector3.zero);
                            resScale.Add(Vector3.one);
                        }
                    }
                    else
                    {
                        for (int j = 0; j < trans.Count; j++)
                        {
                            resPos.Add(trans[j].position == vecMax ? transform.localPosition : trans[j].position);
                            resEuler.Add(trans[j].euler == vecMax ? transform.localEulerAngles : trans[j].euler);
                            resScale.Add(trans[j].scale == vecMax ? transform.localScale : trans[j].scale);
                        }
                    }
                }
                else
                {
                    for (int j = 0; j < trans.Count; j++)
                    {
                        resPos[j] += trans[j].position == vecMax ? transform.localPosition : trans[j].position;
                        resEuler[j] += trans[j].euler == vecMax ? transform.localEulerAngles : trans[j].euler;
                        resScale[j] = Vector3.Scale(trans[j].scale == vecMax ? transform.localScale : trans[j].scale, resScale[j]);
                    }
                }
            }
        }

        logicAniComp.aniClips = aniClips;

        logicAniComp.root = rootInit._entity;
        logicAniComp.isLoop = loopStatus;
    }

    private void OnDrawGizmosSelected()
    {
        if (gameObject.activeInHierarchy && _isCustomBox)
        {
            Gizmos.color = Color.red;
            Vector3 finalPos = transform.position + _boxOffset;
            switch (_boxType)
            {
                case CollisionType.AABB:
                    Gizmos.DrawWireCube(finalPos, _boxSize);
                    break;
                case CollisionType.OBB:
                    Matrix4x4 defMat = Gizmos.matrix;

                    Matrix4x4 mat = Matrix4x4.TRS(finalPos, transform.rotation, Vector3.one);
                    Gizmos.matrix = mat;

                    Gizmos.DrawWireCube(Vector3.zero, _boxSize);

                    Gizmos.matrix = defMat;

                    break;
            }
            Gizmos.color = Color.white;
        }
    }
}
