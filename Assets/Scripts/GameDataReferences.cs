using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDataReferences : Singleton<GameDataReferences>
{
    public List<ResourceData> resources = new List<ResourceData>();
    public List<PolicyData> policies = new List<PolicyData>();
}
