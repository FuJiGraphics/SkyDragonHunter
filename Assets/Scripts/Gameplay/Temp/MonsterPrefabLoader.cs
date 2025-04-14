using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterPrefabLoader : MonoBehaviour
{
    private Dictionary<int, TestAniController> prefabDictionary = new Dictionary<int, TestAniController>();
    private Dictionary<int, TestAniController> ragdollDictionary = new Dictionary<int, TestAniController>();

    private string prefabFilePath = "Prefabs/Monsters_Prefab/";
    private string ragdollFilePath = "Prefabs/Monsters_Ragdoll/";
    private string[] fileNames =
    {
        "BabyDragons",
        "Bats",
        "Bees",
        "DeathAngel",
        "Dragons"
    };

    private void Awake()
    {
        foreach (var file in fileNames)
        {
            TestAniController[] prefabs = Resources.LoadAll<TestAniController>($"{prefabFilePath}{file}");
            TestAniController[] ragdolls = Resources.LoadAll<TestAniController>($"{ragdollFilePath}{file}");

            // Debug.Log($"Loading Path: {filePath}{file}");
            foreach (var prefab in prefabs)
            {
                // Debug.Log($"Loading File id: {prefab.ID}");
                if (prefab.ID == 0)
                    continue;
                
                if (!prefabDictionary.ContainsKey(prefab.ID))
                {
                    prefabDictionary.Add(prefab.ID, prefab);
                }
                else
                {
                    // Debug.LogWarning($"ID {prefab.ID} exists already");
                }
            }

            foreach (var prefab in ragdolls)
            {
                // Debug.Log($"Loading File id: {prefab.ID}");
                if (prefab.ID == 0)
                    continue;

                if (!ragdollDictionary.ContainsKey(prefab.ID))
                {
                    ragdollDictionary.Add(prefab.ID, prefab);
                }
                else
                {
                    // Debug.LogWarning($"ID {prefab.ID} exists already");
                }
            }
        }
        
    }

    public TestAniController GetMonsterAnimController(int id)
    {
        if (prefabDictionary.TryGetValue(id, out TestAniController prefab))
        {            
            return prefab;
        }
        else
        {
            Debug.LogError($"Could not find prefab ID {id}.");
            return null;
        }
    }

    public TestAniController GetRagdollAnimController(int id)
    {
        if (ragdollDictionary.TryGetValue(id, out TestAniController prefab))
        {
            return prefab;
        }
        else
        {
            Debug.LogError($"Could not find prefab ID {id}.");
            return null;
        }
    }
}
