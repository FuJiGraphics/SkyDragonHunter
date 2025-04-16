using SkyDragonHunter.Gameplay;
using SkyDragonHunter.Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SkyDragonHunter.Managers {

    public static class AilmentMgr
    {
        // �ʵ� (Fields)
        private static readonly string s_PathBase = "Prefabs/Ailments/";

        // �Ӽ� (Properties)
        // �ܺ� ���Ӽ� �ʵ� (External dependencies field)
        // �̺�Ʈ (Events)
        // Public �޼���

        public static GameObject GetAilmentPrefab(AilmentType type)
        {
            string path = s_PathBase + $"{type}";
            GameObject ailmentPrefab = Resources.Load<GameObject>(path);
            if (ailmentPrefab == null)
            {
                Debug.LogError($"Ailment '{type}' not found at Resources/{path}");
            }

            return ailmentPrefab;
        }

        public static AilmentBase GetAilmentInstance(AilmentType type)
            => GameObject.Instantiate(GetAilmentPrefab(type))?.GetComponent<AilmentBase>();


        // Private �޼���
        // Others

    } // Scope by class AilmentMgr
} // namespace Root
