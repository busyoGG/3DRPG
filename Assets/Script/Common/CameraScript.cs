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
        Vector3 endPos = _target.transform.position + _rotation * _direction;
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
    /// <summary>
    /// ��ʼ�������¼�
    /// </summary>
    private void InitInput()
    {
        InputManager.Ins().AddKeyboardInputCallback(KeyCode.LeftAlt, InputStatus.Down, () =>
        {
            _showCursor = true;
            Cursor.lockState = CursorLockMode.None;
        });

        InputManager.Ins().AddKeyboardInputCallback(KeyCode.LeftAlt, InputStatus.Up, () =>
        {
            _showCursor = false;
            Cursor.lockState = CursorLockMode.Locked;
        });

        InputManager.Ins().AddMouseInputCallback(0, InputStatus.Down, () =>
        {
            _holdToRotate = true;
        });

        InputManager.Ins().AddMouseInputCallback(0, InputStatus.Up, () =>
        {
            _holdToRotate = false;
        });
    }
}
