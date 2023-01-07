using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class Civilization : ManagedBehaviour
{
    [SerializeField]
    private ResourcesArea resources;

    [SerializeField]
    private PoliciesArea policies;

    [SerializeField]
    private LandGrid landGrid;

    [FoldoutGroup("Debug"), SerializeField]
    private List<ResourceType> debugAddResources;

    [FoldoutGroup("Debug"), SerializeField]
    private List<PolicyType> debugAddPolicies;

    private void Start()
    {
#if UNITY_EDITOR
        debugAddResources.ForEach(x => AddResource(x));
        debugAddPolicies.ForEach(x => AddPolicy(x));
#endif
    }

    public void AddResource(ResourceType resourceType)
    {
        var data = GameDataReferences.GetResourceData(resourceType);
        resources.AddResourcePanel(data);
    }

    public void AddPolicy(PolicyType policyType)
    {
        var data = GameDataReferences.GetPolicyData(policyType);
        policies.AddPolicyPanel(data);
    }

    public void AddImprovement(TileImprovementType improvementType)
    {
        landGrid.AddImprovementToRandomTile(improvementType);
    }

    public override void ManagedUpdate()
    {
        float timeStep = Time.deltaTime;
        resources.TickResources(timeStep);
        policies.TickCooldowns(timeStep);
    }
}
