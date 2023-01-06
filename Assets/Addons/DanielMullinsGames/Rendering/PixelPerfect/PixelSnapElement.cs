using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PixelSnapElement : MonoBehaviour
{
    protected Vector3 cachedLocalPos;
    protected bool shouldRestorePosition;

    private bool didAdjustThisFrame;
    private bool didRestoreThisFrame;

    protected abstract void SnapToPixelGrid();

    public void AdjustPosition()
    {
        didRestoreThisFrame = false;

        if (!didAdjustThisFrame)
        {
            didAdjustThisFrame = true;
            SnapToPixelGrid();
        }
    }

    public void RestorePosition()
    {
        didAdjustThisFrame = false;

        if (!didRestoreThisFrame)
        {
            didRestoreThisFrame = true;

            if (shouldRestorePosition)
            {
                shouldRestorePosition = false;
                transform.localPosition = cachedLocalPos;
            }
        }
    }

    public int GetHierarchyDepth()
    {
        int depth = 0;
        Transform currentTransform = transform;
        while (currentTransform.parent != null)
        {
            currentTransform = currentTransform.parent;
            depth++;
        }

        return depth;
    }

    List<PixelSnapElement> hierarchyInstances = new List<PixelSnapElement>();
    private List<PixelSnapElement> GetSortedHierarchyInstances()
    {
        hierarchyInstances.Clear();

        if (transform.parent != null)
        {
            hierarchyInstances.AddRange(transform.parent.GetComponentsInParent<PixelSnapElement>());
        }
        hierarchyInstances.AddRange(GetComponentsInChildren<PixelSnapElement>());

        hierarchyInstances.Sort((PixelSnapElement a, PixelSnapElement b) => a.GetHierarchyDepth() - b.GetHierarchyDepth());

        return hierarchyInstances;
    }

    protected void TryAdjustPositionsForHierarchy()
    {
        if (!didAdjustThisFrame)
        {
            foreach (PixelSnapElement p in GetSortedHierarchyInstances())
            {
                p.AdjustPosition();
            }
        }
    }

    void OnRenderObject()
    {
        if (!didRestoreThisFrame)
        {
            var hierarchy = GetSortedHierarchyInstances();
            hierarchy.Reverse();

            foreach (PixelSnapElement p in hierarchy)
            {
                p.RestorePosition();
            }
        }
    }
}
