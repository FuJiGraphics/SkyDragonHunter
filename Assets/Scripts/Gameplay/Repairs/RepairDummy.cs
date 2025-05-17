using SkyDragonHunter.Database;
using SkyDragonHunter.Managers;
using SkyDragonHunter.Tables;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

namespace SkyDragonHunter.Gameplay {

    public enum RepairType
    {
        [Description("�Ϲ� ������")]
        Normal,
        [Description("����Ʈ ������")]
        Elite,
        [Description("��ȣ�� ������")]
        Shield,     // �ǵ� �ο� (Shield)
        [Description("���� ������")]
        Healer,
        [Description("���� ������")]
        Divine,     // ���� �ο� (Invincibility)
    }

    public enum RepairGrade
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

    [System.Serializable]
    public class RepairDummy
    {
        private static readonly Dictionary<(RepairType, RepairGrade), int> s_IdTable = new()
        {
            { (RepairType.Normal, RepairGrade.Normal), 61001 },
            { (RepairType.Normal, RepairGrade.Rare),   62001 },
            { (RepairType.Normal, RepairGrade.Unique), 63001 },
            { (RepairType.Normal, RepairGrade.Legend), 64001 },

            { (RepairType.Elite,  RepairGrade.Normal), 61002 },
            { (RepairType.Elite,  RepairGrade.Rare),   62002 },
            { (RepairType.Elite,  RepairGrade.Unique), 63002 },
            { (RepairType.Elite,  RepairGrade.Legend), 63002 },

            { (RepairType.Shield, RepairGrade.Normal), 61003 },
            { (RepairType.Shield, RepairGrade.Rare),   62003 },
            { (RepairType.Shield, RepairGrade.Unique), 63003 },
            { (RepairType.Shield, RepairGrade.Legend), 63003 },

            { (RepairType.Healer, RepairGrade.Normal), 61004 },
            { (RepairType.Healer, RepairGrade.Rare),   62004 },
            { (RepairType.Healer, RepairGrade.Unique), 63004 },
            { (RepairType.Healer, RepairGrade.Legend), 63004 },

            { (RepairType.Divine, RepairGrade.Normal), 61005 },
            { (RepairType.Divine, RepairGrade.Rare),   62005 },
            { (RepairType.Divine, RepairGrade.Unique), 63005 },
            { (RepairType.Divine, RepairGrade.Legend), 63005 },
        };

        // �ʵ� (Fields)
        private readonly string c_IconPath = "Prefabs/Icons/";

        [SerializeField] private int m_Id;
        [SerializeField] private int m_Level;
        [SerializeField] private RepairType m_Type;
        [SerializeField] private RepairGrade m_Grade;
        [SerializeField] private int m_Count = 0;

        private bool m_IsUnlock = false;

        // �Ӽ� (Properties)
        public int ID
        {
            get => m_Id;
            set => m_Id = value;
        }

        public int Level
        {
            get => m_Level;
            set
            {
                if (m_Level >= MaxLevel)
                    return;

                m_Level = value;
                m_OnLevelChangedEvents?.Invoke(m_Level);
            }
        }

        public int Count
        {
            get => m_Count;
            set
            {
                m_Count = Math.Max(value, 0);
            }
        }

        public bool IsUnlock
        {
            get
            {
                if (!m_IsUnlock)
                {
                    m_IsUnlock = Count > 0;
                }
                return m_IsUnlock;
            }
            // TODO: LJH
            set => m_IsUnlock = value;
            // ~TODO
        }

        public Sprite Icon
        {
            get
            {
                string filename = GetData().RepIconResourceName;
                string path = c_IconPath + filename;
                Image image = ResourcesMgr.Load<GameObject>(path)?.GetComponent<Image>();
                return image?.sprite;
            }
        }

        public Color Color => Grade switch
        {
            RepairGrade.Normal => Color.green,
            RepairGrade.Rare => Color.blue,
            RepairGrade.Unique => Color.cyan,
            RepairGrade.Legend => Color.yellow,
            _ => throw new NotImplementedException(),
        };

        public string Name => GetData().RepName;

        public int MaxLevel { get; set; } = 50;

        public RepairType Type
        {
            get => m_Type;
            set => m_Type = value;
        }

        public RepairGrade Grade
        {
            get => m_Grade;
            set => m_Grade = value;
        }

        public bool IsEquip { get; set; } = false;

        // �ܺ� ���Ӽ� �ʵ� (External dependencies field)
        private event Action<int> m_OnLevelChangedEvents;

        // �̺�Ʈ (Events)
        // ����Ƽ (MonoBehaviour �⺻ �޼���)
        // Public �޼���
        public void AddLevelChangedEvent(Action<int> action)
            => m_OnLevelChangedEvents += action;

        public void SetAutoID()
        {
            if (s_IdTable.TryGetValue((Type, Grade), out int id))
            {
                ID = id;
            }
            else
            {
                Debug.LogWarning($"[RepairDummy] ID not found for Type: {Type}, Grade: {Grade}");
                ID = -1;
            }
        }

        public RepairTableData GetData()
        {
            if (ID <= 0)
            {
                SetAutoID();
            }
            return DataTableMgr.RepairTable.Get(ID);
        }


        // Private �޼���
        // Others

    } // Scope by class RepairDummy
} // namespace SkyDragonHunter