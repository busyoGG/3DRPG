using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

[CanEditMultipleObjects]
[CustomEditor(typeof(CompInit), true)]
public class CompInitEditor : Editor
{
    private bool _moveGroup = true;
    private bool _collideGroup = true;

    private SerializedProperty _boxType;

    private SerializedProperty _cam;

    private SerializedProperty _skill;

    private SerializedProperty _triggerFunc;

    private SerializedProperty _logicAni;

    private SerializedProperty _randomId;

    private SerializedProperty _logicAniRoot;


    private void OnEnable()
    {
        _boxType = serializedObject.FindProperty("_boxType");
        _cam = serializedObject.FindProperty("_cameraScript");
        _skill = serializedObject.FindProperty("_skillMap");
        _triggerFunc = serializedObject.FindProperty("_triggerFunc");
        _logicAni = serializedObject.FindProperty("_logicAni");
        _randomId = serializedObject.FindProperty("_randomId");
        _logicAniRoot = serializedObject.FindProperty("_logicAniRoot");
    }

    public override void OnInspectorGUI()
    {
        CompInit compInit = (CompInit)target;
        serializedObject.Update();

        compInit._isMainCharacter = EditorGUILayout.Toggle("�Ƿ�����", compInit._isMainCharacter);

        if (compInit._isMainCharacter)
        {
            EditorGUI.indentLevel++;
            EditorGUILayout.PropertyField(_cam, new GUIContent("�����"));

            EditorGUI.indentLevel--;
        }

        compInit._isRole = EditorGUILayout.Toggle("�Ƿ��ɫ", compInit._isRole);
        if(compInit._isRole)
        {
            EditorGUI.indentLevel++;
            compInit._hp = EditorGUILayout.IntField("����ֵ", compInit._hp);
            compInit._mp = EditorGUILayout.IntField("����ֵ", compInit._mp);
            compInit._shield = EditorGUILayout.IntField("����ֵ", compInit._shield);
            compInit._attack = EditorGUILayout.IntField("������", compInit._attack);
            EditorGUI.indentLevel--;
        }

        compInit._isHealthBar = EditorGUILayout.Toggle("�Ƿ���ʾѪ��",compInit._isHealthBar);

        compInit._isSkill = EditorGUILayout.Toggle("�Ƿ��м���", compInit._isSkill);
        if (compInit._isSkill)
        {
            EditorGUI.indentLevel++;
            EditorGUILayout.PropertyField(_skill, new GUIContent("����"));
            EditorGUI.indentLevel--;
        }

        compInit._isTransform = EditorGUILayout.Toggle("TransformComp", compInit._isTransform) || compInit._isMove;

        compInit._isMove = EditorGUILayout.Toggle("MoveComp", compInit._isMove) || compInit._isJump || compInit._isClimb;

        if (compInit._isMove)
        {
            EditorGUI.indentLevel++;
            _moveGroup = EditorGUILayout.BeginFoldoutHeaderGroup(_moveGroup, "�ƶ��������");
            if (_moveGroup)
            {
                compInit._moveSpeed = EditorGUILayout.FloatField("�ƶ��ٶ�", compInit._moveSpeed);
                compInit._jumpSpeed = EditorGUILayout.FloatField("��Ծ�ٶ�", compInit._jumpSpeed);
                compInit._gravity = EditorGUILayout.FloatField("����", compInit._gravity);
                compInit._jumpScale = EditorGUILayout.FloatField("��Ծ��������", compInit._jumpScale);
            }
            EditorGUILayout.EndFoldoutHeaderGroup();
            EditorGUI.indentLevel--;
        }

        compInit._isJump = EditorGUILayout.Toggle("JumpComp", compInit._isJump);

        compInit._isClimb = EditorGUILayout.Toggle("ClimbComp", compInit._isClimb);

        compInit._isCollide = EditorGUILayout.Toggle("CollideComp", compInit._isCollide);

        if (compInit._isCollide)
        {
            EditorGUI.indentLevel++;
            //_collideGroup = EditorGUILayout.BeginFoldoutHeaderGroup(_collideGroup, "��ײ�������");
            //if (_collideGroup)
            //{
            //    compInit._isStatic = EditorGUILayout.Toggle("�Ƿ�̬����", compInit._isStatic);
            //}
            //EditorGUILayout.EndFoldoutHeaderGroup();
            EditorGUI.indentLevel--;
        }

        compInit._isTrigger = EditorGUILayout.Toggle("TriggerComp", compInit._isTrigger);

        if (compInit._isTrigger)
        {
            EditorGUI.indentLevel++;
            EditorGUILayout.PropertyField(_triggerFunc, new GUIContent("��������"));
            EditorGUI.indentLevel--;
        }

        if(compInit._isCollide || compInit._isTrigger)
        {
            EditorGUILayout.PropertyField(_boxType, new GUIContent("��ײ����"));
            compInit._isTriggerPositive = EditorGUILayout.Toggle("�Ƿ���������", compInit._isTriggerPositive);
        }

        compInit._isQTree = EditorGUILayout.Toggle("QTreeComp", compInit._isQTree) || compInit._isCollide;

        if (compInit._isQTree)
        {
            EditorGUI.indentLevel++;
            compInit._isCustomAABB = EditorGUILayout.Toggle("�Ƿ��Զ���AABB��Χ�д�С", compInit._isCustomAABB);
            if (compInit._isCustomAABB)
            {
                EditorGUI.indentLevel++;
                compInit._aabbSize = EditorGUILayout.Vector3Field("�Զ���AABB��С", compInit._aabbSize);
                EditorGUI.indentLevel--;
            }
            EditorGUI.indentLevel--;
        }

        compInit._isAni = EditorGUILayout.Toggle("�Ƿ��ж���", compInit._isAni) || compInit._isSkill && !compInit._logicAniRoot;
        if (compInit._isAni)
        {
            EditorGUI.indentLevel++;
            compInit._defAni = EditorGUILayout.TextField("Ĭ�϶���", compInit._defAni);
            EditorGUILayout.PropertyField(_logicAni, new GUIContent("��������"));
            EditorGUI.indentLevel--;
        }

        compInit._isDialog = EditorGUILayout.Toggle("�Ƿ��жԻ�", compInit._isDialog);
        if (compInit._isDialog)
        {
            EditorGUI.indentLevel++;
            compInit._targetName = EditorGUILayout.TextField("�Ի���ɫ", compInit._targetName);
            EditorGUILayout.PropertyField(_randomId, new GUIContent("����Ի�����"));
            //compInit._maxDelta = EditorGUILayout.FloatField("����", compInit._maxDelta);
            //compInit._minDelta = EditorGUILayout.FloatField("��̼��", compInit._minDelta);
            EditorGUI.indentLevel--;
        }

        EditorGUILayout.PropertyField(_logicAniRoot, new GUIContent("�߼��������ڵ�"));

        compInit._isAttack = EditorGUILayout.Toggle("�Ƿ�����˺�(����/������)", compInit._isAttack) || compInit._triggerFunc.Contains(TriggerFunction.Attack);
        if (compInit._isAttack || compInit._isRole)
        {
            EditorGUI.indentLevel++;
            compInit._group = EditorGUILayout.IntField("���",compInit._group);
            EditorGUI.indentLevel--;
        }

        compInit._isWeapon = EditorGUILayout.Toggle("�Ƿ�����", compInit._isWeapon);

        serializedObject.ApplyModifiedProperties();
    }
}
