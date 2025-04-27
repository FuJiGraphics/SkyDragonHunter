using SkyDragonHunter.Gameplay;
using SkyDragonHunter.Managers;
using SkyDragonHunter.Scriptables;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using Prefab = UnityEngine.GameObject;

namespace SkyDragonHunter.Database {

    public enum CanonType
    {
        Normal,
        Repeater,
        Slow,
        Burn,
        Freeze,
    }

    public enum CanonGrade
    {
        Normal,
        Rare,
        Unique,
        Legend,
    }

    public static class CanonTable
    {
        // 필드 (Fields)
        private static readonly string s_CanonPrefabPath = "Prefabs/Canons/";
        private static readonly string s_CanonPrefabFileName = "Canon";
        private static Dictionary<CanonType, Dictionary<CanonGrade, GameObject>> s_Cache;

        // 속성 (Properties)
        // 외부 종속성 필드 (External dependencies field)
        // 이벤트 (Events)
        // Public 메서드
        public static GameObject Get(CanonType type, CanonGrade grade)
        {
            if (s_Cache == null)
            {
                s_Cache = new();
            }

            if (!s_Cache.ContainsKey(type))
            {
                s_Cache.Add(type, new());
            }
            if (!s_Cache[type].ContainsKey(grade))
            {
                string filename = s_CanonPrefabFileName + $"{type}" + "_" + $"{grade}";
                string path = Path.Combine(s_CanonPrefabPath, filename);
                GameObject prefab = ResourcesMgr.Load<GameObject>(path);
                s_Cache[type].Add(grade, prefab);
            }
            return s_Cache[type][grade];
        }

        // Private 메서드
        // Others

    } // Scope by class CanonTable
} // namespace SkyDragonHunter.Utility
