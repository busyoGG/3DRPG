using UnityEngine;


[CompRegister(typeof(CollideComp))]
public class CollideComp : Comp
{
    public bool isStatic;

    public Vector3 totalOffset;
    

    public override void Reset()
    {
        isStatic = true;
        totalOffset = Vector3.zero;
    }
}
