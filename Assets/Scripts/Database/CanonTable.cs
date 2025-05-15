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

        #region Resources Only
        // private static Dictionary<CanonType, Dictionary<CanonGrade, GameObject>> s_Cache;
        #endregion

        // 속성 (Properties)
        // 외부 종속성 필드 (External dependencies field)
        // 이벤트 (Events)
        // Public 메서드
        public static GameObject GetPrefab(CanonType type, CanonGrade grade)
        {
            string filename = s_CanonPrefabFileName + $"{type}" + "_" + $"{grade}";
            string path = Path.Combine(s_CanonPrefabPath, filename);
            return ResourcesMgr.Load<GameObject>(path);

            #region Resources Only
            // if (s_Cache == null)
            // {
            //     s_Cache = new();
            // }
            // 
            // if (!s_Cache.ContainsKey(type))
            // {
            //     s_Cache.Add(type, new());
            // }
            // if (!s_Cache[type].ContainsKey(grade))
            // {
            //     string filename = s_CanonPrefabFileName + $"{type}" + "_" + $"{grade}";
            //     string path = Path.Combine(s_CanonPrefabPath, filename);
            //     GameObject prefab = ResourcesMgr.Load<GameObject>(path);
            //     s_Cache[type].Add(grade, prefab);
            // }
            // return s_Cache[type][grade];
            #endregion
        }

        public static CanonDummy[] GetAllCanonDummyTypes()
        {
            var allCanonDummys = new List<CanonDummy>();

            foreach (CanonGrade grade in Enum.GetValues(typeof(CanonGrade)))
            {
                foreach (CanonType type in Enum.GetValues(typeof(CanonType)))
                {
                    CanonDummy newCanonDummy = new CanonDummy
                    {
                        Grade = grade,
                        Type = type,
                        Count = 0,
                        Level = 1
                    };
                    allCanonDummys.Add(newCanonDummy);
                }
            }

            return allCanonDummys.ToArray();
        }

        // Private 메서드
        // Others

    } // Scope by class CanonTable
} // namespace SkyDragonHunter.Utility
