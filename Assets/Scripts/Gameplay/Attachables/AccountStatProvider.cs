using SkyDragonHunter.Gameplay;
using SkyDragonHunter.Managers;
using SkyDragonHunter.Structs;
using SkyDragonHunter.UI;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

namespace SkyDragonHunter.Gameplay {

    public class AccountStatProvider : MonoBehaviour
    {
        // 필드 (Fields)
        // 속성 (Properties)

        // TODO: 현재까지는 UI 표시 테스트용
        public CommonStats FirstStat => m_FirstStats;

        // 외부 종속성 필드 (External dependencies field)
        private CommonStats m_FirstStats;
        private CharacterStatus m_Stats;
        private CanonExecutor m_CanonExecutor;
        private CanonBase m_CurrentEpCanonInstance;
        private bool m_IsInitialized = false;

        // 이벤트 (Events)
        // 유니티 (MonoBehaviour 기본 메서드)
        // Public 메서드
        public void MergedAccountStatsForCharacter()
        {
            CommonStats accStats = AccountMgr.AccountStats;
            CommonStats canonHoldStats = AccountMgr.GetCanonHoldStats();
            CommonStats socketStats = AccountMgr.GetSocketStat();

            AlphaUnit prevLostHealth = m_Stats.MaxHealth - m_Stats.Health;

            var mergeDamage = (accStats.MaxDamage.Value + socketStats.MaxDamage.Value + AccountMgr.DefaultGrowthStats.MaxDamage);
            var mergeHealth = (accStats.MaxHealth.Value + socketStats.MaxHealth.Value + AccountMgr.DefaultGrowthStats.MaxHealth);
            var mergeArmor = (accStats.MaxArmor.Value + socketStats.MaxArmor.Value + AccountMgr.DefaultGrowthStats.MaxArmor);
            var mergeRes = accStats.MaxResilient.Value + socketStats.MaxResilient.Value + AccountMgr.DefaultGrowthStats.MaxResilient;
            var mergeCriMul = (accStats.CriticalMultiplier + socketStats.CriticalMultiplier + AccountMgr.DefaultGrowthStats.CriticalMultiplier);
            var mergeBossDamMul = (accStats.BossDamageMultiplier + socketStats.BossDamageMultiplier);
            var mergeSkillMul = (accStats.SkillEffectMultiplier + socketStats.SkillEffectMultiplier);

            // 곱연산
            m_Stats.MaxDamage = m_FirstStats.MaxDamage.Value * mergeDamage;
            m_Stats.MaxHealth = m_FirstStats.MaxHealth.Value * mergeHealth;
            m_Stats.MaxArmor = m_FirstStats.MaxArmor.Value * mergeArmor;

            // 기본 회복력 1
            if (mergeRes > 0)
            {
                m_Stats.MaxResilient = m_FirstStats.MaxResilient.Value * mergeRes;
            }

            // 합연산
            m_Stats.CriticalChance = m_FirstStats.CriticalChance + accStats.CriticalChance;
            m_Stats.CriticalMultiplier = m_FirstStats.CriticalMultiplier + mergeCriMul;
            m_Stats.BossDamageMultiplier = m_FirstStats.BossDamageMultiplier + mergeBossDamMul;
            m_Stats.SkillEffectMultiplier = m_FirstStats.SkillEffectMultiplier + mergeSkillMul;

            if (gameObject.name == "Airship")
            {
                // 캐논 보유 스탯 적용
                m_Stats.MaxDamage = m_Stats.MaxDamage + canonHoldStats.MaxDamage;
                m_Stats.MaxArmor = m_Stats.MaxArmor + canonHoldStats.MaxArmor;

                // 캐논 장착 스탯 적용
                if (m_CurrentEpCanonInstance != null)
                {
                    var canonData = m_CurrentEpCanonInstance.CanonData;
                    BigInteger newEpATK = BigInteger.Parse(canonData.canEqATK) + (BigInteger.Parse(canonData.canEqATKup) * AccountMgr.EquipCannonDummy.Level);
                    BigInteger newEpDEF = BigInteger.Parse(canonData.canEqDEF) + (BigInteger.Parse(canonData.canEqDEFup) * AccountMgr.EquipCannonDummy.Level);
                    m_Stats.MaxDamage = m_Stats.MaxDamage + newEpATK;
                    m_Stats.MaxArmor = m_Stats.MaxArmor + newEpDEF;
                }
            }

            m_Stats.ResetDamage();
            m_Stats.ResetHealth();
            m_Stats.ResetArmor();
            m_Stats.ResetResilient();
            m_Stats.Health -= prevLostHealth;
        }

        public void OnEquipCannonEvents(CanonDummy canonDummy)
        {
            AccountMgr.EquipCannonDummy = canonDummy;
            m_CurrentEpCanonInstance = canonDummy.GetCanonInstance()?.GetComponent<CanonBase>();
            MergedAccountStatsForCharacter();
        }

        public void OnUnequipCannonEvents()
        {
            AccountMgr.EquipCannonDummy = null;
            m_CurrentEpCanonInstance = null;
            MergedAccountStatsForCharacter();
        }

        // Private 메서드
        public void Init()
        {
            if (m_IsInitialized)
                return;

            m_IsInitialized = true;

            m_Stats = GetComponent<CharacterStatus>();
            if (m_Stats == null)
            {
                Debug.Log("Account Stats과 병합할 수 없습니다. CharacterStatus가 null입니다.");
            }

            m_FirstStats = new CommonStats();
            m_FirstStats.SetMaxDamage(m_Stats.MaxDamage.Value);
            m_FirstStats.SetMaxHealth(m_Stats.MaxHealth.Value);
            m_FirstStats.SetMaxShield(m_Stats.MaxShield.Value);
            m_FirstStats.SetMaxArmor(m_Stats.MaxArmor.Value);
            m_FirstStats.SetMaxResilient(m_Stats.MaxResilient.Value);
            m_FirstStats.SetCriticalChance(m_Stats.CriticalChance);
            m_FirstStats.SetCriticalMultiplier(m_Stats.CriticalMultiplier);
            m_FirstStats.SetBossDamageMultiplier(m_Stats.BossDamageMultiplier);
            m_FirstStats.SetSkillEffectMultiplier(m_Stats.SkillEffectMultiplier);

            if (TryGetComponent<CanonExecutor>(out var canonExecutor))
            {
                m_CanonExecutor = canonExecutor;
                canonExecutor.onEquipEvents += OnEquipCannonEvents;
                canonExecutor.onUnequipEvents += OnUnequipCannonEvents;
            }

            m_Stats.ResetAll();

            AccountMgr.onLevelUpEvents += MergedAccountStatsForCharacter;

            MergedAccountStatsForCharacter();
        }

        // Others

    } // Scope by class CrystalStatProvider
} // namespace SkyDragonHunter