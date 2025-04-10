using SkyDragonHunter.Managers;
using SkyDragonHunter.Tables;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using TMPro;
using UnityEngine;

namespace SkyDragonHunter.Gameplay {

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

    public class UIMasterySocket : MonoBehaviour
    {
        // 필드 (Fields)
        [SerializeField] private Sprite m_Icon;
        [SerializeField] private int m_MaxSlotCount; // TODO: 임시

        private int currentCount = 1;

        // 속성 (Properties)
        public int ID { get; private set; }
        public MasterySockeyType Type { get; private set; } = MasterySockeyType.Damage;
        public BigInteger Stat { get; private set; }
        public int NextID { get; private set; }
        public string SlotCountString { get; private set; }
        public Sprite Icon => m_Icon;

        // 외부 종속성 필드 (External dependencies field)
        // 이벤트 (Events)
        // 유니티 (MonoBehaviour 기본 메서드)
        private void Start()
        {
            SlotCountString = currentCount.ToString() + "/" + m_MaxSlotCount.ToString();
        }

        // Public 메서드
        public void SetSocketData(int id)
        {
            var newData = DataTableMgr.MasterySocketTable.Get(id);
            ID = newData.ID;
            Type = (MasterySockeyType)newData.StatType;
            Stat = BigInteger.Parse(newData.Stat);
            NextID = newData.NextSocketID;
            SlotCountString = currentCount.ToString() + "/" + m_MaxSlotCount.ToString();
        }

        public bool LevelUp()
        {
            bool result = false;
            if (NextID != -1)
            {
                var newData = DataTableMgr.MasterySocketTable.Get(NextID);
                ID = newData.ID;
                Type = (MasterySockeyType)newData.StatType;
                Stat = BigInteger.Parse(newData.Stat);
                NextID = newData.NextSocketID;
                result = true;
                currentCount++;
                SlotCountString = currentCount.ToString() + "/" + m_MaxSlotCount.ToString();
            }
            return result;
        }

        // Private 메서드
        // Others

    } // Scope by class MasterySocket
} // namespace Root