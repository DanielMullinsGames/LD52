using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameDataReferences
{
    private static List<ResourceData> resources;
    private static List<PolicyData> policies;
    private static List<TechnologyData> technologies;
    private static List<GameObject> tileImprovementPrefabs;

    private static bool initialized = false;

    public static ResourceData GetResourceData(ResourceType resourceType)
    {
        TryInitialize();
        return resources.Find(x => x.resourceType == resourceType);
    }

    public static PolicyData GetPolicyData(PolicyType policyType)
    {
        TryInitialize();
        return policies.Find(x => x.policyType == policyType);
    }

    public static List<TechnologyData> GetAllTechnology()
    {
        TryInitialize();
        return technologies;
    }

    public static TechnologyData GetTechnologyData(TechnologyType techType)
    {
        TryInitialize();
        return technologies.Find(x => x.technologyType == techType);
    }

    public static GameObject GetTimeImprovementPrefab(TileImprovementType tileType)
    {
        TryInitialize();
        return tileImprovementPrefabs.Find(x => x.GetComponent<TileImprovement>().ImprovementType == tileType);
    }

    private static void TryInitialize()
    {
        if (!initialized)
        {
            resources = new List<ResourceData>(Resources.LoadAll<ResourceData>("Data/ResourceData"));
            policies = new List<PolicyData>(Resources.LoadAll<PolicyData>("Data/PolicyData"));
            technologies = new List<TechnologyData>(Resources.LoadAll<TechnologyData>("Data/TechnologyData"));
            tileImprovementPrefabs = new List<GameObject>(Resources.LoadAll<GameObject>("Prefabs/GridView/TileImprovements"));
            
            initialized = true;
        }
    }
}
