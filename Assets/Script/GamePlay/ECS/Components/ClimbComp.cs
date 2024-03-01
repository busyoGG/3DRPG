
using UnityEngine;

[CompRegister(typeof(ClimbComp))]
public class ClimbComp : Comp
{
    public float enterTime;

    public float targetTime;

    //public bool firstClimb;

    public Vector3 jumpForward;

    public Vector3 fixedForward;

    public override void Reset()
    {
        enterTime = 0f;
        targetTime = 0.1f;
        //firstClimb = false;
        jumpForward = Vector3.zero;
        fixedForward = Vector3.zero;
    }
}
