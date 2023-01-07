using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class Civilization : ManagedBehaviour
{
    public ResourcesArea Resources => resources;
    [SerializeField]
    private ResourcesArea resources;

    public PoliciesArea Policies => policies;
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
        debugAddResources.ForEach(x => resources.AddResourcePanel(x));
        debugAddPolicies.ForEach(x => AddPolicy(x));
#endif
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
