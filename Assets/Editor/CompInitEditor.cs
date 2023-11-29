using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

[CanEditMultipleObjects]
[CustomEditor(typeof(CompInit))]
public class CompInitEditor : Editor
{
    private bool _moveGroup = true;
    private bool _collideGroup = true;

    private SerializedProperty _collideType;

    private SerializedProperty _cam;

    private SerializedProperty _skill;

    private void OnEnable()
    {
        _collideType = serializedObject.FindProperty("_collideType");
        _cam = serializedObject.FindProperty("_cameraScript");
        _skill = serializedObject.FindProperty("_skillMap");
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
            compInit._isSkill = EditorGUILayout.Toggle("�Ƿ��м���", compInit._isSkill);
            if (compInit._isSkill)
            {
                EditorGUI.indentLevel++;
                EditorGUILayout.PropertyField(_skill, new GUIContent("����"));
                EditorGUI.indentLevel--;
            }
            EditorGUI.indentLevel--;
        }

        compInit._isMove = EditorGUILayout.Toggle("MoveComp", compInit._isMove);

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
            _collideGroup = EditorGUILayout.BeginFoldoutHeaderGroup(_collideGroup, "��ײ�������");
            if (_collideGroup)
            {
                EditorGUILayout.PropertyField(_collideType, new GUIContent("��ײ����"));
                compInit._isStatic = EditorGUILayout.Toggle("�Ƿ�̬����", compInit._isStatic);
            }
            EditorGUILayout.EndFoldoutHeaderGroup();
            EditorGUI.indentLevel--;
        }

        compInit._isTrigger = EditorGUILayout.Toggle("TriggerComp", compInit._isTrigger);

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

        serializedObject.ApplyModifiedProperties();
    }
}
