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
        // �ʵ� (Fields)
        private static readonly string s_CanonPrefabPath = "Prefabs/Canons/";
        private static Dictionary<CanonType, Dictionary<CanonGrade, Prefab>> s_Cache;

        // �Ӽ� (Properties)
        // �ܺ� ���Ӽ� �ʵ� (External dependencies field)
        // �̺�Ʈ (Events)
        // ����Ƽ (MonoBehaviour �⺻ �޼���)
        // Public �޼���
        public static Prefab Get(CanonType type, CanonGrade grade)
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
                string canonFileName = type.ToString() + "_" + grade.ToString();
                Prefab prefab = Resources.Load<GameObject>(s_CanonPrefabPath + canonFileName);
                s_Cache[type].Add(grade, prefab);
            }
            return s_Cache[type][grade];
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

        // Private �޼���
        // Others

    } // Scope by class CanonTable
} // namespace SkyDragonHunter.Utility
