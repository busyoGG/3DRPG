
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    /// <summary>
    /// �������Ŀ��
    /// </summary>
    public GameObject _target;
    /// <summary>
    /// ��������ٶ� �������ת�����������Ϊ1
    /// </summary>
    public float _followSpeed = 0.5f;
    /// <summary>
    /// �����ת�ٶ�
    /// </summary>
    public float _rotateSpeed = 10f;
    /// <summary>
    /// �����Ŀ�����
    /// </summary>
    private Vector3 _direction = Vector3.zero;
    /// <summary>
    /// �����ת�Ƕ�
    /// </summary>
    private Quaternion _rotation = new Quaternion();
    /// <summary>
    /// ����ƶ�����
    /// </summary>
    private Vector2 _mouseMove = Vector2.zero;
    /// <summary>
    /// Y��������
    /// </summary>
    private Vector2 _limitY = new Vector2(-80, 80);
    /// <summary>
    /// �Ƿ�չʾ���
    /// </summary>
    private bool _showCursor = false;
    /// <summary>
    /// �Ƿ���԰��������ת
    /// </summary>
    private bool _holdToRotate = false;
    /// <summary>
    /// ��Ұ����
    /// </summary>
    private float _scale = 1f;

    void Start()
    {
        _direction.z = -(transform.position - _target.transform.position).magnitude;
        _rotation = transform.rotation;
        Vector3 angle = transform.eulerAngles;
        _mouseMove.x = angle.y;
        _mouseMove.y = angle.x;

        Cursor.lockState = CursorLockMode.Locked;

        InitInput();
    }

    void LateUpdate()
    {
        if (!_showCursor || _holdToRotate)
        {
            UpdateScale();
            UpdatePosition();
            UpdateRotation();
            transform.LookAt(_target.transform.position);
        }
    }

    public Quaternion GetRotation(bool needX = true)
    {
        //return _rotation;
        Vector3 rot = transform.eulerAngles;
        if (!needX)
        {
            rot.x = 0;
        }
        return Quaternion.Euler(rot);
    }

    /// <summary>
    /// ����λ��
    /// </summary>
    private void UpdatePosition()
    {
        Vector3 endPos = _target.transform.position + _rotation * _direction * _scale;
        if (_followSpeed == 1)
        {
            transform.position = endPos;
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, endPos, _followSpeed);
        }
    }
    /// <summary>
    /// ������ת
    /// </summary>
    private void UpdateRotation()
    {
        _mouseMove.x += Input.GetAxis("Mouse X") * _rotateSpeed;
        _mouseMove.y -= Input.GetAxis("Mouse Y") * _rotateSpeed;

        _mouseMove.y = Mathf.Clamp(_mouseMove.y, _limitY.x, _limitY.y);

        _rotation = Quaternion.Euler(_mouseMove.y, _mouseMove.x, 0);
    }

    private void UpdateScale()
    {
        _scale -= Input.GetAxis("Mouse ScrollWheel");
        _scale = Mathf.Clamp(_scale, 0.3f, 1);
    }

    /// <summary>
    /// ��ʼ�������¼�
    /// </summary>
    private void InitInput()
    {
        InputManager.Ins().AddEventListener(() =>
        {
            Dictionary<InputKey, InputStatus> curKey = InputManager.Ins().GetKey();
            foreach (var data in curKey)
            {
                InputKey key = data.Key;
                InputStatus status = data.Value;
                if (status == InputStatus.Hold || status == InputStatus.Down)
                {
                    switch (key)
                    {
                        case InputKey.LeftAlt:
                            _showCursor = true;
                            Cursor.lockState = CursorLockMode.None;
                            break;
                        case InputKey.MouseLeft:
                            _holdToRotate = true;
                            break;
                    }
                }
                else
                {
                    switch (key)
                    {
                        case InputKey.LeftAlt:
                            _showCursor = false;
                            Cursor.lockState = CursorLockMode.Locked;
                            break;
                        case InputKey.MouseLeft:
                            _holdToRotate = false;
                            break;
                    }
                }
            }
        });
    }
}
