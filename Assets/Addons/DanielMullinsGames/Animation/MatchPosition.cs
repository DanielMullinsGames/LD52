using UnityEngine;

public class MatchPosition : ManagedBehaviour
{
    public Transform target;
    public Vector3 offset;
    public bool x = true;
    public bool y = true;
    public bool z = true;
	public bool destroyIfTargetNull;

    public override void ManagedLateUpdate()
    {
		if (target != null)
		{
			Vector3 pos = new Vector3 (
				              x ? target.transform.position.x : transform.position.x,
				              y ? target.transform.position.y : transform.position.y,
				              z ? target.transform.position.z : transform.position.z
			              );
            transform.position = pos + offset;
        }
		else if (destroyIfTargetNull)
		{
			Destroy (gameObject);
		}
    }
}