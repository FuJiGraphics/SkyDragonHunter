using SkyDragonHunter.Managers;
using SkyDragonHunter.Structs;
using SkyDragonHunter.Tables;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

namespace SkyDragonHunter.UI {

    public enum MasterySocketType
    {
        Damage = 0, 
        Health = 1, 
        Armor = 2, 
        Resilient = 3,
        CriticalMultiplier = 4,
        BossDamageMultiplier = 5,
        SkillEffectMultiplier = 6,
    }

    // ������ ������ �� ���� ���� ���� �޸𸮿��� ���ܵǸ� �ȵ˴ϴ�.
    // AccountMgr���� ������ ������ �����Ͽ� ������Ʈ �Ǳ� ����
    public class UIMasterySocket
    {
        // �ʵ� (Fields)
        private int m_MaxLevelCount = 0;
        private int m_CurrentCount = 0;

        private string m_IconName;

        // �Ӽ� (Properties)
        public int ID { get; private set; }
        public Sprite Icon => ResourcesMgr.Load<Sprite>(m_IconName);
        public string SocketName { get; private set; }
        public string Description { get; private set; }
        public MasterySocketType Type { get; private set; } = MasterySocketType.Damage;
        public BigNum Stat { get; private set; }
        public double Multiplier { get; private set; }
        public int NextID { get; private set; }
        public string SlotCountString { get; private set; }
        public bool IsActive { get; private set; } = false;
        public bool IsMaxLevel { get; private set; } = false;
        public UIMasterySocket NextLevelSocket => (NextID == -1) ? null : new UIMasterySocket(NextID);
        public int CurrentLevel => m_CurrentCount;

        // �ܺ� ���Ӽ� �ʵ� (External dependencies field)
        // �̺�Ʈ (Events)
        public event Action onLevelupEvents;

        // ����Ƽ (MonoBehaviour �⺻ �޼���)

        // Public �޼���
        public UIMasterySocket(int socketId)
        {
            if (socketId < 0)
            {
                Debug.LogError($"[UIMasterySocket]: �߸��� ���� ID �Դϴ�. {socketId}");
                return;
            }

            InitSocket(socketId);
            m_CurrentCount = 0;
        }

        public bool LevelUp()
        {
            if (m_CurrentCount == 0)
            {
                m_CurrentCount++;
                IsActive = true;
                ResetSlotCountString();
                if (m_CurrentCount >= m_MaxLevelCount)
                    IsMaxLevel = true;
                AccountMgr.RegisterMasterySocket(this);
                onLevelupEvents?.Invoke();
                return true;
            }

            bool result = false;
            if (NextID != -1)
            {
                var newData = DataTableMgr.MasterySocketTable.Get(NextID);
                ID = newData.ID;
                SocketName = newData.SocketName;
                Description = newData.Description;
                Type = (MasterySocketType)newData.StatType;
                Stat = new BigNum(newData.Stat);
                Multiplier = newData.Multiplier;
                NextID = newData.NextSocketID;
                m_IconName = newData.SocketIconName;
                result = true;
                m_CurrentCount++;
                ResetSlotCountString();
                onLevelupEvents?.Invoke();
            }
            if (m_CurrentCount >= m_MaxLevelCount)
                IsMaxLevel = true;
            return result;
        }

        // Private �޼���
        private void InitSocket(int id)
        {
            var newData = DataTableMgr.MasterySocketTable.Get(id);
            ID = newData.ID;
            SocketName = newData.SocketName;
            Description = newData.Description;
            Type = (MasterySocketType)newData.StatType;
            Stat = new BigNum(newData.Stat);
            NextID = newData.NextSocketID;
            m_IconName = newData.SocketIconName;
            m_MaxLevelCount = CalculateNextLevelCount();
            m_CurrentCount = 0;
            ResetSlotCountString();
        }

        private int CalculateNextLevelCount()
        {
            int resultCount = 1;

            int safeMaxLoopCount = 100000;
            int next = NextID;
            for (int i = 0; i < safeMaxLoopCount; ++i)
            {
                if (next != -1)
                {
                    resultCount++;
                    var newData = DataTableMgr.MasterySocketTable.Get(next);
                    next = newData.NextSocketID;
                }
                else
                    break;

                if (i + 1 == safeMaxLoopCount)
                    Debug.LogError("[UIMasterySocket]: ���� ���� �ʰ�");
            }
            return resultCount;
        }

        private void ResetSlotCountString()
            => SlotCountString = m_CurrentCount.ToString() + "/" + m_MaxLevelCount.ToString();
        
        // Others

    } // Scope by class MasterySocket
} // namespace Root