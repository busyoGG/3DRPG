using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CompRegister(typeof(TransformComp))]
public class TransformComp : Comp
{
    public Vector3 position;

    public Quaternion rotation;

    public Vector3 scale;

    public Vector3 lastPosition;

    public Quaternion lastRotation;

    public Vector3 lastScale;

    public bool changed = false;

    //public Entity parent;

    //public List<Entity> children;
    public override void Reset()
    {
        position = Vector3.zero;
        rotation = Quaternion.identity;
        scale = Vector3.one;
        lastPosition = Vector3.zero;
        lastRotation = Quaternion.identity;
        lastScale = Vector3.one;
        changed = false;
        //parent = null;
        //children = new List<Entity>();
    }
}
