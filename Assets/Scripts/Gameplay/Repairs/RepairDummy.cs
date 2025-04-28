using SkyDragonHunter.Database;
using SkyDragonHunter.Managers;
using SkyDragonHunter.Tables;
using System;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

namespace SkyDragonHunter.Gameplay {

    public enum RepairType
    {
        Normal,
        Elite,
        Shield,     // �ǵ� �ο� (Shield)
        Healer,
        Divine,     // ���� �ο� (Invincibility)
    }

    public enum RepairGrade
    {
        Normal,
        Rare,
        Unique,
        Legend,
    }

    [System.Serializable]
    public class RepairDummy
    {
        // �ʵ� (Fields)
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
        }

        public Sprite Icon
        {
            get
            {
                string filename = GetData().RepIconResourceName;
                string path = Path.Combine("Prefabs/Icons", filename);
                Image image = ResourcesMgr.Load<Image>(path);
                return image?.sprite;
            }
        }

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

        public RepairTableData GetData()
            => DataTableMgr.RepairTable.Get(ID);


        // Private �޼���
        // Others

    } // Scope by class RepairDummy
} // namespace SkyDragonHunter