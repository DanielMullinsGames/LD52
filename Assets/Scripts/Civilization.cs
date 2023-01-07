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

    public TechnologyArea Technologies => technologies;
    [SerializeField]
    private TechnologyArea technologies;

    [SerializeField]
    private LandGrid landGrid;

    [SerializeField]
    private RulerView rulerView;

    [FoldoutGroup("Debug"), SerializeField]
    private List<ResourceType> debugAddResources;

    [FoldoutGroup("Debug"), SerializeField]
    private List<PolicyType> debugAddPolicies;

    [FoldoutGroup("Debug"), SerializeField]
    private List<TechnologyType> debugAddAvailableTech;

    [FoldoutGroup("Debug"), SerializeField]
    private List<TechnologyType> debugAddLearnedTech;

    private void Start()
    {
#if UNITY_EDITOR
        debugAddResources.ForEach(x => resources.AddResourcePanel(x));
        debugAddPolicies.ForEach(x => policies.AddPolicyPanel(x));
        debugAddAvailableTech.ForEach(x => technologies.AddTechnologyToAvailable(x));
        debugAddLearnedTech.ForEach(x => technologies.AddTechnologyToLearned(x));
#endif
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
        rulerView.TickAge(timeStep);
    }
}
