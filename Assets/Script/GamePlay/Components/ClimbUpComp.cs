[CompRegister(typeof(ClimbUpComp))]
public class ClimbUpComp : Comp
{
    public float targetY;
    public override void Reset()
    {
        targetY = 0;
    }
}
