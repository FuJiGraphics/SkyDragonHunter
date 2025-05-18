using SkyDragonHunter.Gameplay;
using SkyDragonHunter.Managers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using UnityEngine;
using Prefab = UnityEngine.GameObject;

namespace SkyDragonHunter.Database {

    public static class EnumExtensions
    {
        public static string GetDescription(this Enum value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());

            if (fi != null)
            {
                DescriptionAttribute[] attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);

                if (attributes != null && attributes.Length > 0)
                    return attributes[0].Description;
            }

            return value.ToString();
        }
    }

    public enum CanonType
    {
        [Description("�Ϲ� ����")]
        Normal,
        [Description("���� ����")]
        Repeater,
        [Description("��ȭ ����")]
        Slow,
        [Description("ȭ�� ����")]
        Burn,
        [Description("���� ����")]
        Freeze,
    }

    public enum CanonGrade
    {
        [Description("�븻")]
        Normal,
        [Description("����")]
        Rare,
        [Description("����ũ")]
        Unique,
        [Description("������")]
        Legend,
    }

    public static class CanonTable
    {
        // �ʵ� (Fields)
        private static readonly string s_CanonPrefabPath = "Prefabs/Canons/";
        private static readonly string s_CanonPrefabFileName = "Canon";

        static private Dictionary<CanonType, Dictionary<CanonGrade, GameObject>> s_InstanceMap = new();

        #region Resources Only
        // private static Dictionary<CanonType, Dictionary<CanonGrade, GameObject>> s_Cache;
        #endregion

        // �Ӽ� (Properties)
        // �ܺ� ���Ӽ� �ʵ� (External dependencies field)
        // �̺�Ʈ (Events)
        // Public �޼���
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

        public static Sprite GetIcon(CanonType type)
            => type switch
            {
                CanonType.Normal => ResourcesMgr.Load<Sprite>("NormalCannon"),
                CanonType.Repeater => ResourcesMgr.Load<Sprite>("RapidFireCannon"),
                CanonType.Slow => ResourcesMgr.Load<Sprite>("SlowCannon"),
                CanonType.Burn => ResourcesMgr.Load<Sprite>("BurnCannon"),
                CanonType.Freeze => ResourcesMgr.Load<Sprite>("FreezeCannon"),
                _ => throw new NotImplementedException(),
            };

        public static Sprite GetGradeOutline(CanonGrade grade)
            => grade switch
            {
                CanonGrade.Normal => ResourcesMgr.Load<Sprite>("UI_Atlas[UI_Atlas_108]"),
                CanonGrade.Rare => ResourcesMgr.Load<Sprite>("UI_Atlas[UI_Atlas_98]"),
                CanonGrade.Unique => ResourcesMgr.Load<Sprite>("UI_Atlas[UI_Atlas_93]"),
                CanonGrade.Legend => ResourcesMgr.Load<Sprite>("UI_Atlas[UI_Atlas_103]"),
                _ => throw new NotImplementedException(),
            };

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

        public static Prefab GetInstance(CanonType type, CanonGrade grade)
        {
            if (!s_InstanceMap.ContainsKey(type))
            {
                s_InstanceMap.Add(type, new());
            }

            if (!s_InstanceMap[type].ContainsKey(grade))
            {
                s_InstanceMap[type].Add(grade, GetPrefab(type, grade));
            }

            return s_InstanceMap[type][grade];
        }


        // Private �޼���
        // Others

    } // Scope by class CanonTable
} // namespace SkyDragonHunter.Utility
