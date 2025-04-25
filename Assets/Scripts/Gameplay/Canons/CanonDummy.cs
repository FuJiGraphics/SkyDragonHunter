using SkyDragonHunter.Database;
using System;
using UnityEngine;

namespace SkyDragonHunter.Gameplay {

    public class CanonDummy
    {
        // �ʵ� (Fields)
        private int m_Level;

        // �Ӽ� (Properties)
        public int ID { get; set; }
        public int Level 
        {
            get => m_Level;
            set
            {
                m_Level = value;
                m_OnLevelChangedEvents?.Invoke(m_Level);
            }
        }
        public CanonType Type { get; set; }
        public CanonGrade Grade { get; set; }

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