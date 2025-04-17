using SkyDragonHunter.Gameplay;
using SkyDragonHunter.Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SkyDragonHunter.Managers {

    public static class AilmentMgr
    {
        // 필드 (Fields)
        private static readonly string s_PathBase = "Prefabs/Ailments/";

        // 속성 (Properties)
        // 외부 종속성 필드 (External dependencies field)
        // 이벤트 (Events)
        // Public 메서드

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


        // Private 메서드
        // Others

    } // Scope by class AilmentMgr
} // namespace Root
