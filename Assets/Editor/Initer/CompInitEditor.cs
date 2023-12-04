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

    private SerializedProperty _collideType;

    private SerializedProperty _cam;

    private SerializedProperty _skill;

    private SerializedProperty _triggerFunc;

    private SerializedProperty _logicAni;


    private void OnEnable()
    {
        _collideType = serializedObject.FindProperty("_collideType");
        _cam = serializedObject.FindProperty("_cameraScript");
        _skill = serializedObject.FindProperty("_skillMap");
        _triggerFunc = serializedObject.FindProperty("_triggerFunc");
        _logicAni = serializedObject.FindProperty("_logicAni");
    }

    public override void OnInspectorGUI()
    {
        CompInit compInit = (CompInit)target;
        serializedObject.Update();

        compInit._isMainCharacter = EditorGUILayout.Toggle("是否主角", compInit._isMainCharacter);

        if (compInit._isMainCharacter)
        {
            EditorGUI.indentLevel++;
            EditorGUILayout.PropertyField(_cam, new GUIContent("主相机"));
            compInit._isSkill = EditorGUILayout.Toggle("是否有技能", compInit._isSkill);
            if (compInit._isSkill)
            {
                EditorGUI.indentLevel++;
                EditorGUILayout.PropertyField(_skill, new GUIContent("技能"));
                EditorGUI.indentLevel--;
            }
            EditorGUI.indentLevel--;
        }

        compInit._isTransform = EditorGUILayout.Toggle("TransformComp", compInit._isTransform) || compInit._isMove;

        compInit._isMove = EditorGUILayout.Toggle("MoveComp", compInit._isMove) || compInit._isJump || compInit._isClimb;

        if (compInit._isMove)
        {
            EditorGUI.indentLevel++;
            _moveGroup = EditorGUILayout.BeginFoldoutHeaderGroup(_moveGroup, "移动组件属性");
            if (_moveGroup)
            {
                compInit._moveSpeed = EditorGUILayout.FloatField("移动速度", compInit._moveSpeed);
                compInit._jumpSpeed = EditorGUILayout.FloatField("跳跃速度", compInit._jumpSpeed);
                compInit._gravity = EditorGUILayout.FloatField("重力", compInit._gravity);
                compInit._jumpScale = EditorGUILayout.FloatField("跳跃过程缩放", compInit._jumpScale);
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
            _collideGroup = EditorGUILayout.BeginFoldoutHeaderGroup(_collideGroup, "碰撞组件属性");
            if (_collideGroup)
            {
                EditorGUILayout.PropertyField(_collideType, new GUIContent("碰撞类型"));
                compInit._isStatic = EditorGUILayout.Toggle("是否静态物体", compInit._isStatic);
            }
            EditorGUILayout.EndFoldoutHeaderGroup();
            EditorGUI.indentLevel--;
        }

        compInit._isTrigger = EditorGUILayout.Toggle("TriggerComp", compInit._isTrigger);

        if (compInit._isTrigger)
        {
            EditorGUI.indentLevel++;
            compInit._isTriggerPositive = EditorGUILayout.Toggle("是否主动触发",compInit._isTriggerPositive);
            EditorGUILayout.PropertyField(_triggerFunc, new GUIContent("触发类型"));
            EditorGUI.indentLevel--;
        }

        compInit._isQTree = EditorGUILayout.Toggle("QTreeComp", compInit._isQTree) || compInit._isCollide;

        if (compInit._isQTree)
        {
            EditorGUI.indentLevel++;
            compInit._isCustomAABB = EditorGUILayout.Toggle("是否自定义AABB包围盒大小", compInit._isCustomAABB);
            if (compInit._isCustomAABB)
            {
                EditorGUI.indentLevel++;
                compInit._aabbSize = EditorGUILayout.Vector3Field("自定义AABB大小", compInit._aabbSize);
                EditorGUI.indentLevel--;
            }
            EditorGUI.indentLevel--;
        }

        compInit._isAni = EditorGUILayout.Toggle("是否有动画", compInit._isAni) || compInit._isSkill;
        if (compInit._isAni)
        {
            EditorGUI.indentLevel++;
            compInit._defAni = EditorGUILayout.TextField("默认动画", compInit._defAni);
            EditorGUILayout.PropertyField(_logicAni, new GUIContent("动画数据"));
            EditorGUI.indentLevel--;
        }

        serializedObject.ApplyModifiedProperties();
    }
}
