using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrameLoopManager : MonoBehaviour
{
    private enum IterationType
    {
        Update,
        FixedUpdate,
        LateUpdate,
    }

    public static FrameLoopManager Instance
    {
        get
        {
            if (shuttingDown)
            {
                return null;
            }

            if (instance == null)
            {
                var singletonObject = new GameObject();
                instance = singletonObject.AddComponent<FrameLoopManager>();
                singletonObject.name = "Frame Loop Manager";
                DontDestroyOnLoad(singletonObject);
            }

            return instance;
        }
    }
    private static FrameLoopManager instance;
    private static bool shuttingDown = false;

    private List<ManagedBehaviour> activeBehaviours = new List<ManagedBehaviour>();
    private List<ManagedBehaviour> behavioursForLoop = new List<ManagedBehaviour>();

    private bool iterationDisabled;

    public void RegisterBehaviour(ManagedBehaviour behaviour)
    {
        if (!activeBehaviours.Contains(behaviour))
        {
            // Simple implementation of Execution Order for now - it is not used much.
            if (behaviour.ExecutionOrder < 0)
            {
                activeBehaviours.Insert(0, behaviour);
            }
            else
            {
                activeBehaviours.Add(behaviour);
            }
        }
    }

    public void SetIterationDisabled(bool disabled)
    {
        iterationDisabled = disabled;
    }

    private void OnApplicationQuit()
    {
        shuttingDown = true;
    }

    private void OnDestroy()
    {
        shuttingDown = true;
    }

    private void Update()
    {
        ExecuteIteration(IterationType.Update);
    }

    private void FixedUpdate()
    {
        ExecuteIteration(IterationType.FixedUpdate);
    }

    private void LateUpdate()
    {
        ExecuteIteration(IterationType.LateUpdate);
    }

    private void ExecuteIteration(IterationType type)
    {
        activeBehaviours.RemoveAll(x => x == null);

        behavioursForLoop.Clear();
        behavioursForLoop.AddRange(activeBehaviours);

        foreach (ManagedBehaviour b in behavioursForLoop)
        {
            if (!iterationDisabled || b.UpdateWhenPaused)
            {
                if (b != null && b.enabled && b.gameObject.activeInHierarchy)
                {
                    switch (type)
                    {
                        case IterationType.Update:
                            b.ManagedUpdate();
                            break;
                        case IterationType.FixedUpdate:
                            b.ManagedFixedUpdate();
                            break;
                        case IterationType.LateUpdate:
                            b.ManagedLateUpdate();
                            break;
                    }
                }
            }
        }
    }
}
