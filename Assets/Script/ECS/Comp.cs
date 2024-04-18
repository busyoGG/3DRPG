
public abstract class Comp
{
    public int compId { get; set; }
    public string compName { get; set; }

    public Comp() { Reset(); }
    public abstract void Reset();
}
