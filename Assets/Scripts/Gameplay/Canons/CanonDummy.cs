using SkyDragonHunter.Database;
using System;
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
                m_Level = value;
                m_OnLevelChangedEvents?.Invoke(m_Level);
            }
        }

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
        {
            GameObject prefab = CanonTable.Get(Type, Grade);
            GameObject instance = null;
            if (prefab != null)
            {
                instance = GameObject.Instantiate(prefab);
            }
            return instance;
        }

        // Private �޼���
        // Others

    } // Scope by class Item
} // namespace SkyDragonHunter