using System.Numerics;

[CompRegister(typeof(ClimbComp))]
public class ClimbComp : Comp
{
    public float enterTime;

    public float targetTime;

    public override void Reset()
    {
        enterTime = 0f;
        targetTime = 0.5f;
    }
}
