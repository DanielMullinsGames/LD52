using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable2D : Interactable
{
    protected sealed override bool CollisionIs2D => true;

    public int SortOrder => sortOrder + SortOrderAdjustment;
    public int SortOrderAdjustment { get; set; }
    [SerializeField]
    private int sortOrder = 0;

    public virtual int CompareInteractionSortOrder(Interactable2D other)
    {
        return other.SortOrder - SortOrder;
    }
}
