using UnityEngine;

public class AutoRotate : ManagedBehaviour
{
    public Vector3 rotationSpeed = default;
    public bool local = default;

    public override void ManagedUpdate()
    {
        Vector3 rotateAmount = rotationSpeed * Time.deltaTime;
        if (local)
        {
            transform.localRotation = Quaternion.Euler(transform.localEulerAngles + rotateAmount);
        }
        else
        {
            transform.rotation = Quaternion.Euler(transform.localEulerAngles + rotateAmount);
        }
    }
}
