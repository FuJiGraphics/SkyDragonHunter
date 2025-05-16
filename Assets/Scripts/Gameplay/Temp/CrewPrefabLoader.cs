using SkyDragonHunter.Entities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrewPrefabLoader : MonoBehaviour
{
    private Dictionary<int, NewCrewControllerBT> prefabDictionary = new Dictionary<int, NewCrewControllerBT>();

    private string prefabFilePath = "Assets/Prefabs/Crews/";

    private void Awake()
    {

        NewCrewControllerBT[] prefabs = Resources.LoadAll<NewCrewControllerBT>($"{prefabFilePath}");

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
    }

    public NewCrewControllerBT GetCrewController(int id)
    {
        if (prefabDictionary.TryGetValue(id, out NewCrewControllerBT prefab))
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
