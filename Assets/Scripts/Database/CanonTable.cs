using SkyDragonHunter.Gameplay;
using SkyDragonHunter.Managers;
using SkyDragonHunter.Scriptables;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Prefab = UnityEngine.GameObject;

namespace SkyDragonHunter.Database {

    public enum CanonType
    {
        DefaultCanon,
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
        private static Dictionary<CanonType, Prefab> s_Cache;

        // 속성 (Properties)
        // 외부 종속성 필드 (External dependencies field)
        // 이벤트 (Events)
        // 유니티 (MonoBehaviour 기본 메서드)
        // Public 메서드
        public static Prefab Get(CanonType type, CanonGrade grade)
        {
            if (s_Cache == null)
            {
                s_Cache = new();
            }
            if (!s_Cache.ContainsKey(type))
            {
                string canonFileName = type.ToString() + "_" + grade.ToString();
                s_Cache.Add(type, Resources.Load<GameObject>(s_CanonPrefabPath + canonFileName));
            }
            return s_Cache[type];
        }

        public static List<Prefab> ToList()
        {
            List<Prefab> result = new List<Prefab>();
            foreach (CanonType canonType in Enum.GetValues(typeof(CanonType)))
            {
                foreach (CanonGrade canonGrade in Enum.GetValues(typeof(CanonGrade)))
                {
                    result.Add(Get(canonType, canonGrade));
                }
            }
            return result;
        }

        public static Prefab[] ToArray()
            => ToList().ToArray();

        // Private 메서드
        // Others

    } // Scope by class CanonTable
} // namespace SkyDragonHunter.Utility
