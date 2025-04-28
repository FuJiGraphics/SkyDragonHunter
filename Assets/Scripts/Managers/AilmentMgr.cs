using SkyDragonHunter.Gameplay;
using SkyDragonHunter.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SkyDragonHunter.Managers {

    public static class AilmentMgr
    {
        // �ʵ� (Fields)
        private static readonly string s_PathBase = "Prefabs/Ailments/";
        private static Dictionary<AilmentType, GameObject> s_Cache;

        // �Ӽ� (Properties)
        // �ܺ� ���Ӽ� �ʵ� (External dependencies field)
        // �̺�Ʈ (Events)
        // Public �޼���

        public static AilmentType? FindAilmentType(int id)
        {
            AilmentType? result = null;
            foreach (AilmentType type in Enum.GetValues(typeof(AilmentType)))
            {
                if (type != AilmentType.Max)
                {
                    GameObject ailmentGo = GetAilmentPrefab(type);
                    if (ailmentGo.TryGetComponent<AilmentBase>(out var ailmentBase))
                    {
                        if (ailmentBase.ID == id)
                        {
                            result = type;
                        }
                    }
                }
            }
            return result;
        }

        public static GameObject GetAilmentPrefab(AilmentType type)
        {
            if (s_Cache == null)
            {
                s_Cache = new();
            }

            if (!s_Cache.ContainsKey(type))
            {
                string path = s_PathBase + $"{type}";
                GameObject ailmentPrefab = ResourcesMgr.Load<GameObject>(path);
                if (ailmentPrefab == null)
                {
                    Debug.LogError($"Ailment '{type}' not found at Resources/{path}");
                }
                else
                {
                    s_Cache.Add(type, ailmentPrefab);
                }
            }
            return s_Cache[type];
        }

        public static AilmentBase GetAilmentInstance(AilmentType type)
            => GameObject.Instantiate(GetAilmentPrefab(type))?.GetComponent<AilmentBase>();


        // Private �޼���
        // Others

    } // Scope by class AilmentMgr
} // namespace Root
