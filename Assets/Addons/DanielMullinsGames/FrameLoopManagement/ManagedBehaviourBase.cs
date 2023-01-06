using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagedBehaviourBase : MonoBehaviour
{
    public virtual void Update() { }
    public virtual void FixedUpdate() { }
    public virtual void LateUpdate() { }
    protected virtual void Awake() { }
    protected virtual void OnEnable() { }
}
