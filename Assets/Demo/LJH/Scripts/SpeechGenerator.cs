using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SkyDragonHunter {

    public static class SpeechGenerator
    {
        public static void Text(string str, Transform owner)
        {
            GameObject prefab = Resources.Load<GameObject>("Prefabs/UISpeechBubble");
            if (prefab == null)
                Debug.LogError($"Did not found prefab {"Prefabs/UISpeechBubble"}");
            GameObject.Instantiate(prefab, owner);
        }


    } // Scope by class SpeechGenerator
} // namespace Root