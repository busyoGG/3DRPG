using UnityEngine;
/// <summary>
/// ��������
/// </summary>
public enum ControlType
{
    MouseAndKeyboard,
    KeyboardOnly,
    ControllerOnly
}

/// <summary>
/// ECS����ƥ������
/// </summary>
public enum ECSRuleType
{
    AllOf,
    AnyOf,
    ExcludeOf
}

/// <summary>
/// ��ײ������
/// </summary>
public enum CollisionType
{
    Circle,
    AABB,
    OBB,
    Ray
}

/// <summary>
/// �����ͷ�״̬
/// </summary>
public enum SkillPlayStatus
{
    Idle,
    Charge,
    Play,
    End
}

/// <summary>
/// ������״̬
/// </summary>
public enum TriggerStatus
{
    Idle,
    Enter,
    Keeping,
    Exit
}

/// <summary>
/// �����λ
/// </summary>
public enum InputKey
{
    None = KeyCode.None,
    MouseLeft = -1,
    MouseRight = -2,
    MouseMid = -3,
    A = KeyCode.A,
    B = KeyCode.B,
    C = KeyCode.C,
    D = KeyCode.D,
    E = KeyCode.E,
    F = KeyCode.F,
    G = KeyCode.G,
    H = KeyCode.H,
    I = KeyCode.I,
    J = KeyCode.J,
    K = KeyCode.K,
    L = KeyCode.L,
    M = KeyCode.M,
    N = KeyCode.N,
    O = KeyCode.O,
    P = KeyCode.P,
    Q = KeyCode.Q,
    R = KeyCode.R,
    S = KeyCode.S,
    T = KeyCode.T,
    U = KeyCode.U,
    V = KeyCode.V,
    W = KeyCode.W,
    X = KeyCode.X,
    Y = KeyCode.Y,
    Z = KeyCode.Z,
    LeftAlt = KeyCode.LeftAlt,
    RightAlt = KeyCode.RightAlt,
    LeftCtrl = KeyCode.LeftControl,
    RightCtrl = KeyCode.RightControl,
    LeftShift = KeyCode.LeftShift,
    RightShift = KeyCode.LeftShift,
    ESC = KeyCode.Escape,
    Alpha0 = KeyCode.Alpha0,
    Alpha1 = KeyCode.Alpha1,
    Alpha2 = KeyCode.Alpha2,
    Alpha3 = KeyCode.Alpha3,
    Alpha4 = KeyCode.Alpha4,
    Alpha5 = KeyCode.Alpha5,
    Alpha6 = KeyCode.Alpha6,
    Alpha7 = KeyCode.Alpha7,
    Alpha8 = KeyCode.Alpha8,
    Alpha9 = KeyCode.Alpha9,
    Keypad0 = KeyCode.Keypad0,
    Keypad1 = KeyCode.Keypad1,
    Keypad2 = KeyCode.Keypad2,
    Keypad3 = KeyCode.Keypad3,
    Keypad4 = KeyCode.Keypad4,
    Keypad5 = KeyCode.Keypad5,
    Keypad6 = KeyCode.Keypad6,
    Keypad7 = KeyCode.Keypad7,
    Keypad8 = KeyCode.Keypad8,
    Keypad9 = KeyCode.Keypad9,
    Space = KeyCode.Space,
    Return = KeyCode.Return
}

/// <summary>
/// ��������
/// </summary>
public enum InputType
{
    Mouse,
    Keyboard,
    Controller
}

/// <summary>
/// ����״̬
/// </summary>
public enum InputStatus
{
    None,
    Down,
    Up,
    Hold
}

/// <summary>
/// ��ͼ״̬
/// </summary>
public enum MapStatus
{
    Inactive,
    Active,
    Cache
}

/// <summary>
/// ���ܴ�������
/// </summary>
public enum SkillTrigger
{
    Click,
    Hold,
    Charge
}

public enum TriggerFunction
{
    [InspectorName("�޴���")] None,
    [InspectorName("����")] Interactive
}

public enum SkillEntity
{
    Cube
}

public enum InteractionType
{
    None,
    Dialog,
    PickUp,
    Interaction
}

public enum TimerType
{
    Sync,
    Async
}
