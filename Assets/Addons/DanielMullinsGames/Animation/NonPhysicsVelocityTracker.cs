using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NonPhysicsVelocityTracker : ManagedBehaviour
{
    public Vector3 AverageVelocity { get; private set; }

    [SerializeField]
    private float averagingPeriodLength = 1f;

    [SerializeField]
    private int numSamples = 10;

    private List<Vector3> previousVelocities;
    private Vector3 prevPosition;
    private float lastSampleTime;

    private void Start()
    {
        previousVelocities = new List<Vector3>();
    }

    public override void ManagedUpdate()
    {
        Vector3 sum = Vector3.zero;
        foreach(Vector3 sample in previousVelocities)
        {
            sum += sample;
        }
        AverageVelocity = sum / numSamples;
        
        if (Time.time - lastSampleTime > averagingPeriodLength / numSamples)
        {
            lastSampleTime = Time.time;
            previousVelocities.Add(transform.position - prevPosition);
            if (previousVelocities.Count > numSamples)
            {
                previousVelocities.RemoveAt(0);
            }
        }
        prevPosition = transform.position;
    }
}
