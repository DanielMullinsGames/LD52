using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class Civilization : ManagedBehaviour
{
    [SerializeField]
    private ResourcesArea resources;

    [FoldoutGroup("Debug"), SerializeField]
    private List<ResourceType> debugAddResources;

    private void Start()
    {
#if UNITY_EDITOR
        debugAddResources.ForEach(x => AddResource(x));
#endif
    }

    public void AddResource(ResourceType resourceType)
    {
        var data = GameDataReferences.Instance.resources.Find(x => x.resourceType == resourceType);
        resources.AddResourcePanel(data);
    }

    public override void ManagedUpdate()
    {
        resources.TickResources(Time.deltaTime);
    }
}
