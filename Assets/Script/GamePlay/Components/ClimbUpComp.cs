using UnityEngine;

[CompRegister(typeof(ClimbUpComp))]
public class ClimbUpComp : Comp
{
    public float targetY;
    public Vector3 climbOffset;
    public override void Reset()
    {
        targetY = 0;
        climbOffset = Vector3.zero;
    }
}
