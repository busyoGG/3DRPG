[CompRegister(typeof(JumpComp))]
public class JumpComp : Comp
{
    public bool startJump;

    public override void Reset()
    {
        startJump = false;
    }
}

