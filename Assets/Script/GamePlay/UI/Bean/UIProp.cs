using System;

public class UIProp
{
    public string _value;

    private string val
    {
        get
        {
            return _value;
        }
        set
        {
            _value = value;
            Invoke();
        }
    }

    private Action<string> _onValueChange = null;

    public UIProp() { }

    public UIProp(string value)
    {
        _value = value;
    }

    public void Set(string value)
    {
        this.val = value;
    }

    public void Set<T>(T value)
    {
        this.val = value.ToString();
    }

    public void Invoke()
    {
        _onValueChange?.Invoke(val);
    }

    public override string ToString()
    {
        return val;
    }
}
