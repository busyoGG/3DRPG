using System.Numerics;

[CompRegister(typeof(ClimbComp))]
public class ClimbComp : Comp
{
    public float speed;

    public float jumpStep;

    public Vector3 lastPosition;

    public Vector3 nextPosition;

    public float enterTime;

    public bool isClimb;

    public override void Reset()
    {
        enterTime = 0f;
        speed = 0f;
        jumpStep = 0f;
        isClimb = false;
        lastPosition = Vector3.Zero;
        nextPosition = Vector3.Zero;
    }
}
