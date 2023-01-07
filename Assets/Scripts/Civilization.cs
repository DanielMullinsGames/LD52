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
        var data = GameDataReferences.Instance.resources.Find(x => x.resourceType == resourceType);
        resources.AddResourcePanel(data);
    }

    public void AddPolicy(PolicyType policyType)
    {
        var data = GameDataReferences.Instance.policies.Find(x => x.policyType == policyType);
        policies.AddPolicyPanel(data);
    }

    public override void ManagedUpdate()
    {
        float timeStep = Time.deltaTime;
        resources.TickResources(timeStep);
        policies.TickCooldowns(timeStep);
    }
}
