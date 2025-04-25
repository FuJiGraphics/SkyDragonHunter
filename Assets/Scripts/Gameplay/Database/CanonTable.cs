using SkyDragonHunter.Managers;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Prefab = UnityEngine.GameObject;

namespace SkyDragonHunter.Database {

    public enum CanonType
    {
        Normal_DefaultCanon,
        Rare_DefaultCanon,
        Unique_DefaultCanon,
        Legend_DefaultCanon,
    }

    public static class CanonTable
    {
        // 필드 (Fields)
        private static readonly string s_CanonPrefabPath = "Prefabs/Canons/";
        private static Dictionary<CanonType, Prefab> s_CanonPrefabMap;

        // 속성 (Properties)
        // 외부 종속성 필드 (External dependencies field)
        // 이벤트 (Events)
        // 유니티 (MonoBehaviour 기본 메서드)
        // Public 메서드
        public static void Init()
        {
            s_CanonPrefabMap = new Dictionary<CanonType, Prefab>()
            {
                { CanonType.Normal_DefaultCanon, ResourcesMgr.Load<Prefab>(s_CanonPrefabPath + "Canon(Normal)") },
                { CanonType.Rare_DefaultCanon, ResourcesMgr.Load<Prefab>(s_CanonPrefabPath + "Canon(Rare)") },
                { CanonType.Unique_DefaultCanon, ResourcesMgr.Load<Prefab>(s_CanonPrefabPath + "Canon(Unique)") },
                { CanonType.Legend_DefaultCanon, ResourcesMgr.Load<Prefab>(s_CanonPrefabPath + "Canon(Legend)") },
            };
        }

        public static void Release()
        {
            if (s_CanonPrefabMap == null)
                return;
            s_CanonPrefabMap.Clear();
        }

        public static Prefab Get(CanonType type)
            => s_CanonPrefabMap[type];

        public static List<Prefab> ToList()
            => s_CanonPrefabMap.Values.ToList();

        public static Prefab[] ToArray()
            => ToList().ToArray();

        // Private 메서드
        // Others

    } // Scope by class CanonTable
} // namespace SkyDragonHunter.Utility
