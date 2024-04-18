using Unity.VisualScripting;
using UnityEngine;


[CompRegister(typeof(CollideComp))]
public class CollideComp : Comp
{
    //public bool isStatic;

    public float stepHeight;

    public float slopeAngle;

    public bool isCanClimb;

    public Vector3 totalOffset;

    public Vector3 lastTotalOffset;

    public Vector3 closestTop;

    public Vector3 closestCenter;

    public bool isStatic;

    public float powerToMove;

    public float powerToBeMove;

    public override void Reset()
    {
        //isStatic = true;
        isCanClimb = true;
        stepHeight = 0;
        slopeAngle = 0.8f;
        totalOffset = Vector3.zero;
        lastTotalOffset = Vector3.zero;
        closestTop = Vector3.zero;
        closestCenter = Vector3.zero;
        isStatic = false;
        powerToMove = 0;
        powerToBeMove = 0;
    }
}
