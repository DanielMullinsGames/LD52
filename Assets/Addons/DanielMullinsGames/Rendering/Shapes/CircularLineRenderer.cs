using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class CircularLineRenderer : ManagedBehaviour
{
    [SerializeField]
    private int segments = 360;

    [SerializeField]
    private float width = 0.01f;

    [SerializeField]
    private Vector2 radii = Vector2.one;

    private LineRenderer lineRenderer;

    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        DrawCircle();
    }

    private void DrawCircle()
    {
        lineRenderer.useWorldSpace = false;
        lineRenderer.startWidth = width;
        lineRenderer.endWidth = width;
        lineRenderer.positionCount = segments + 1;

        var pointCount = segments + 1; // add extra point to make startpoint and endpoint the same to close the circle
        var points = new Vector3[pointCount];

        for (int i = 0; i < pointCount; i++)
        {
            var rad = Mathf.Deg2Rad * (i * 360f / segments);
            points[i] = new Vector3(Mathf.Sin(rad) * radii.x, Mathf.Cos(rad) * radii.y, 0);
        }

        lineRenderer.SetPositions(points);
    }
}
