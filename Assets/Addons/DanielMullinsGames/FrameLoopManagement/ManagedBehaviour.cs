using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagedBehaviour : ManagedBehaviourBase
{
    public static ReferenceSetToggle PauseAll = new ReferenceSetToggle();

    private bool initialized = false;

    public void Initialize()
    {
        if (!initialized)
        {
            initialized = true;
            ManagedInitialize();
        }
    }

    public virtual bool UpdateWhenPaused { get { return false; } }

    public virtual void ManagedUpdate() { }
    public virtual void ManagedFixedUpdate() { }
    public virtual void ManagedLateUpdate() { }

    protected virtual void ManagedInitialize() { }

    public sealed override void Update() 
    {
        if (CanUpdate())
        {
            ManagedUpdate();
        }
    }

    public sealed override void FixedUpdate() 
    {
        if (CanUpdate())
        {
            ManagedFixedUpdate();
        }
    }
    public sealed override void LateUpdate()
    {
        if (CanUpdate())
        {
            ManagedLateUpdate();
        }
    }

    protected sealed override void Awake()
    {
        if (!initialized)
        {
            initialized = true;
            ManagedInitialize();
        }
    }

    private bool CanUpdate()
    {
        return UpdateWhenPaused || !PauseAll.True;
    }
}
