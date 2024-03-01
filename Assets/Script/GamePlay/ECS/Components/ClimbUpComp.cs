using UnityEngine;

[CompRegister(typeof(ClimbUpComp))]
public class ClimbUpComp : Comp
{
    public float targetY;
    public Vector3 climbOffset;
    public float progress;
    public override void Reset()
    {
        targetY = 0;
        climbOffset = Vector3.zero;
        progress = 0;
    }
}
