using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class Civilization : ManagedBehaviour
{
    public CivType type;
    public bool isEarth;

    public float resourceRateMultiplier = 1f;
    public float cooldownMultiplier = 1f;

    public ResourcesArea Resources => resources;
    [SerializeField]
    private ResourcesArea resources;

    public PoliciesArea Policies => policies;
    [SerializeField]
    private PoliciesArea policies;

    public TechnologyArea Technologies => technologies;
    [SerializeField]
    private TechnologyArea technologies;

    public LandGrid Grid => landGrid;
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

    public BobaMiningAnim bobaTileAnim;

    private void Start()
    {
#if UNITY_EDITOR
        debugAddResources.ForEach(x => resources.AddResourcePanel(x));
        debugAddPolicies.ForEach(x => policies.AddPolicyPanel(x));
        debugAddAvailableTech.ForEach(x => technologies.AddTechnologyToAvailable(x));
        debugAddLearnedTech.ForEach(x => technologies.AddTechnologyToLearned(x));
#endif

        if (type == CivType.Planet)
        {
            resources.AddResourcePanel(ResourceType.Boba);

            if (isEarth)
            {
                resources.GetResource(ResourceType.Boba).Rate += 0.1f;
            }
        }
        else 
        {
            resources.AddResourcePanel(ResourceType.People);
        }

        if (type == CivType.Lava || type == CivType.Robo)
        {
            technologies.AddTechnologyToAvailable(TechnologyType.UnlockBoba);
            resources.GetOrCreateResource(ResourceType.Knowledge).Rate += 0.2f;
            resources.GetOrCreateResource(ResourceType.Force).Rate += 0.2f;
            resources.GetOrCreateResource(ResourceType.Force).Maximum += 50f;
        }
    }

    public void AddImprovement(TileImprovementType improvementType)
    {
        landGrid.AddImprovementToRandomTile(improvementType, type);
    }

    public void Tick(float timeStep)
    {
        resources.TickResources(timeStep * resourceRateMultiplier);
        policies.TickCooldowns(timeStep * cooldownMultiplier);
        rulerView.TickAge(timeStep);
    }
}
