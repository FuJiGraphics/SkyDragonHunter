using SkyDragonHunter.Database;
using SkyDragonHunter.Managers;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace SkyDragonHunter.Gameplay {

    [System.Serializable]
    public class CanonDummy
    {
        // �ʵ� (Fields)
        [SerializeField] private int m_Id;
        [SerializeField] private int m_Level;
        [SerializeField] private CanonType m_Type;
        [SerializeField] private CanonGrade m_Grade;
        [SerializeField] private int m_Count = 0;

        private bool m_IsUnlock = false;

        // �Ӽ� (Properties)
        public int ID 
        { 
            get => m_Id; 
            set => m_Id = value; 
        }

        public string Name => m_Type.GetDescription();

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

        public int MaxLevel { get; set; } = 50;

        public CanonType Type 
        { 
            get => m_Type; 
            set => m_Type = value; 
        }

        public CanonGrade Grade { 
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

        public GameObject GetCanonInstance()
            => CanonTable.GetInstance(m_Type, m_Grade);

        // Private �޼���
        // Others

    } // Scope by class Item
} // namespace SkyDragonHunter