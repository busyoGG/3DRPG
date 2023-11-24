
using UnityEngine;
public class RayData
{
    public Vector3 origin;

    public Vector3 forward;

    public float length;

    public Vector3 target
    {
        get
        {
            if(_target == null)
            {
                _target = origin + forward * length;
            }
            return _target;
        }
    }

    private Vector3 _target;
}
