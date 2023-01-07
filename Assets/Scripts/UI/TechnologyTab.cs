using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TechnologyTab : Interactable2D
{
    [SerializeField]
    private List<GameObject> toDisable;

    [SerializeField]
    private List<GameObject> toEnable;

    [SerializeField]
    private bool startEnabled;

    [SerializeField]
    private TechnologyTab otherTab;

    protected override void ManagedInitialize()
    {
        SetSelected(startEnabled);
    }

    protected override void OnCursorSelectStart()
    {
        SetSelected(true);
        otherTab.SetSelected(false);
    }

    public void SetSelected(bool selected)
    {
        toEnable.ForEach(x => x.gameObject.SetActive(selected));
        toDisable.ForEach(x => x.gameObject.SetActive(!selected));
    }
}
