using UnityEngine;


[CompRegister(typeof(CollideComp))]
public class CollideComp : Comp
{
    //public bool isStatic;

    public float stepHeight;

    public float slopeAngle;

    public Vector3 totalOffset;

    public Vector3 closestTop;

    public Vector3 closestCenter;
    public override void Reset()
    {
        //isStatic = true;
        stepHeight = 0;
        slopeAngle = 0.8f;
        totalOffset = Vector3.zero;
        closestTop = Vector3.zero;
        closestCenter = Vector3.zero;
    }
}
