using System;

/// <summary>
/// 绑定UI组件和动作事件，使其支持交互
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
public class UIActionBind : Attribute
{
    public string _type;

    public string _path;

    public UIActionBind(string type,string path)
    {
        _type = type;
        _path = path;
    }
}
