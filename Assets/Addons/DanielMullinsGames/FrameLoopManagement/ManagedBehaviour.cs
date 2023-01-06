using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagedBehaviour : ManagedBehaviourBase
{
    private bool initialized = false;

    public void Initialize()
    {
        if (!initialized)
        {
            initialized = true;
            ManagedInitialize();
        }
    }

    public virtual int ExecutionOrder => 0;

    public virtual bool UpdateWhenPaused { get { return false; } }

    public virtual void ManagedUpdate() { }

    public virtual void ManagedFixedUpdate() { }

    public virtual void ManagedLateUpdate() { }

    protected virtual void ManagedInitialize() { }
    protected virtual void ManagedOnEnable() { }

    // Seal default Unity calls so they cannot be accidentally used in child classes.
    public sealed override void Update() { }
    public sealed override void FixedUpdate() { }
    public sealed override void LateUpdate() { }

    protected sealed override void Awake()
    {
        if (!initialized)
        {
            initialized = true;
            ManagedInitialize();
        }
    }

    protected sealed override void OnEnable()
    {
        FrameLoopManager.Instance.RegisterBehaviour(this);
        ManagedOnEnable();
    }
}
