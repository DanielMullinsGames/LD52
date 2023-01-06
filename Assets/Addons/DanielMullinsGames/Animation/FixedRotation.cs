using UnityEngine;
using Sirenix.OdinInspector;

public class FixedRotation : ManagedBehaviour
{
    public bool fixX;
    [ShowIf("fixX")]
    public float xRotation;

    public bool fixY;
    [ShowIf("fixY")]
    public float yRotation;

    public bool fixZ;
    [ShowIf("fixZ")]
    public float zRotation;

    public override void ManagedUpdate()
    {
        float x = fixX ? xRotation : transform.eulerAngles.x;
        float y = fixY ? yRotation : transform.eulerAngles.y;
        float z = fixZ ? zRotation : transform.eulerAngles.z;

        transform.eulerAngles = new Vector3(x, y, z);
	}
}
