using SkyDragonHunter.Managers;
using SkyDragonHunter.Tables;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using TMPro;
using UnityEngine;

namespace SkyDragonHunter.UI {

    public enum MasterySockeyType
    {
        Damage = 0, 
        Health = 1, 
        Armor = 2, 
        Resilient = 3,
        CriticalMultiplier = 5,
        BossDamageMultiplier = 6,
        SkillEffectMultiplier = 7,
    }

    public class UIMasterySocket
    {
        // �ʵ� (Fields)
        private int m_MaxLevelCount = 0;
        private int m_CurrentCount = 0;

        // �Ӽ� (Properties)
        public int ID { get; private set; }
        public MasterySockeyType Type { get; private set; } = MasterySockeyType.Damage;
        public BigInteger Stat { get; private set; }
        public int NextID { get; private set; }
        public string SlotCountString { get; private set; }
        public bool IsActive { get; private set; } = false;
        public bool IsMaxLevel { get; private set; } = false;

        // �ܺ� ���Ӽ� �ʵ� (External dependencies field)
        // �̺�Ʈ (Events)
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
                return true;
            }

            bool result = false;
            if (NextID != -1)
            {
                var newData = DataTableMgr.MasterySocketTable.Get(NextID);
                ID = newData.ID;
                Type = (MasterySockeyType)newData.StatType;
                Stat = BigInteger.Parse(newData.Stat);
                Stat = (BigInteger)((double)Stat * newData.Multiplier);
                NextID = newData.NextSocketID;
                result = true;
                m_CurrentCount++;
                ResetSlotCountString();
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
            Type = (MasterySockeyType)newData.StatType;
            Stat = BigInteger.Parse(newData.Stat);
            NextID = newData.NextSocketID;
            m_MaxLevelCount = CalculateNextLevelCount();
            m_CurrentCount = 0;
            ResetSlotCountString();
        }

        private int CalculateNextLevelCount()
        {
            int resultCount = 1;

            int safeMaxLoopCount = 100;
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