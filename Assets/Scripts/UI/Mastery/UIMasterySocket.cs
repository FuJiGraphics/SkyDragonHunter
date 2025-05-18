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

    // 소켓은 생성된 후 절대 게임 도중 메모리에서 제외되면 안됩니다.
    // AccountMgr에서 소켓의 정보를 참조하여 업데이트 되기 때문
    public class UIMasterySocket
    {
        // 필드 (Fields)
        private int m_MaxLevelCount = 0;
        private int m_CurrentCount = 0;

        private string m_IconName;

        // 속성 (Properties)
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

        // 외부 종속성 필드 (External dependencies field)
        // 이벤트 (Events)
        public event Action onLevelupEvents;

        // 유니티 (MonoBehaviour 기본 메서드)

        // Public 메서드
        public UIMasterySocket(int socketId)
        {
            if (socketId < 0)
            {
                Debug.LogError($"[UIMasterySocket]: 잘못된 소켓 ID 입니다. {socketId}");
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
                SaveLoadMgr.CallSaveGameData();
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

            SaveLoadMgr.CallSaveGameData();
            return result;
        }

        // Private 메서드
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
                    Debug.LogError("[UIMasterySocket]: 루프 제한 초과");
            }
            return resultCount;
        }

        private void ResetSlotCountString()
            => SlotCountString = m_CurrentCount.ToString() + "/" + m_MaxLevelCount.ToString();
        
        // Others

    } // Scope by class MasterySocket
} // namespace Root