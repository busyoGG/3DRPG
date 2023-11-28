using System.Numerics;

[CompRegister(typeof(ClimbComp))]
public class ClimbComp : Comp
{
    public float enterTime;

    public float targetTime;

    public bool firstClimb;

    public override void Reset()
    {
        enterTime = 0f;
        targetTime = 0.5f;
        firstClimb = false;
    }
}
