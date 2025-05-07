using SkyDragonHunter.Gameplay;
using SkyDragonHunter.Managers;
using SkyDragonHunter.Structs;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SkyDragonHunter {

    public class CrewAccountStatInfoHandler : MonoBehaviour
    {
        // 필드 (Fields)
        // 속성 (Properties)

        // TODO: 현재까지는 UI 표시 테스트용
        public CommonStats BaseStat => m_BaseStats;

        // 외부 종속성 필드 (External dependencies field)
        private CommonStats m_BaseStats;
        private CharacterStatus m_Stats;
        private CanonExecutor m_CanonExecutor;
        private CanonBase m_CurrentEpCanonInstance;
        private RepairDummy m_CurrentEqRepair;
        private bool m_IsInitialized = false;

        // 이벤트 (Events)
        // 유니티 (MonoBehaviour 기본 메서드)
        private void OnDestroy()
        {
            if (m_CanonExecutor != null)
            {
                m_CanonExecutor.onEquipEvents -= OnEquipCannonEvents;
                m_CanonExecutor.onUnequipEvents -= OnUnequipCannonEvents;
            }

            if (TryGetComponent<RepairExecutor>(out var repairExecutor))
            {
                repairExecutor.onEquipEvents -= OnEquipRepairEvents;
                repairExecutor.onUnequipEvents -= OnUnequipRepairEvents;
            }

            AccountMgr.AddLevelUpEvent(MergedAccountStatsForCharacter);
        }

        // Public 메서드
        public void MergedAccountStatsForCharacter()
        {
            CommonStats accStats = AccountMgr.AccountStats;
            CommonStats canonHoldStats = AccountMgr.GetCanonHoldStats();
            CommonStats repairHoldStats = AccountMgr.GetRepairHoldStats();
            CommonStats socketStats = AccountMgr.GetSocketStat();

            BigNum prevLostHealth = m_Stats.MaxHealth - m_Stats.Health;

            var mergeDamage = (accStats.MaxDamage + socketStats.MaxDamage + AccountMgr.DefaultGrowthStats.MaxDamage);
            var mergeHealth = (accStats.MaxHealth + socketStats.MaxHealth + AccountMgr.DefaultGrowthStats.MaxHealth);
            var mergeArmor = (accStats.MaxArmor + socketStats.MaxArmor + AccountMgr.DefaultGrowthStats.MaxArmor);
            var mergeRes = accStats.MaxResilient + socketStats.MaxResilient + AccountMgr.DefaultGrowthStats.MaxResilient;
            var mergeCriMul = (accStats.CriticalMultiplier + socketStats.CriticalMultiplier + AccountMgr.DefaultGrowthStats.CriticalMultiplier);
            var mergeBossDamMul = (accStats.BossDamageMultiplier + socketStats.BossDamageMultiplier);
            var mergeSkillMul = (accStats.SkillEffectMultiplier + socketStats.SkillEffectMultiplier);

            // 곱연산
            m_Stats.MaxDamage = m_BaseStats.MaxDamage * mergeDamage;
            m_Stats.MaxHealth = m_BaseStats.MaxHealth * mergeHealth;
            m_Stats.MaxArmor = m_BaseStats.MaxArmor * mergeArmor;
            m_Stats.MaxResilient = m_BaseStats.MaxResilient * mergeRes;

            // 합연산
            m_Stats.CriticalChance = m_BaseStats.CriticalChance + accStats.CriticalChance;
            m_Stats.CriticalMultiplier = m_BaseStats.CriticalMultiplier + mergeCriMul;
            m_Stats.BossDamageMultiplier = m_BaseStats.BossDamageMultiplier + mergeBossDamMul;
            m_Stats.SkillEffectMultiplier = m_BaseStats.SkillEffectMultiplier + mergeSkillMul;

            if (gameObject.name == "Airship")
            {
                // 캐논 보유 스탯 적용
                m_Stats.MaxDamage = m_Stats.MaxDamage + canonHoldStats.MaxDamage;
                m_Stats.MaxArmor = m_Stats.MaxArmor + canonHoldStats.MaxArmor;

                // 수리공 보유 스탯 적용
                m_Stats.MaxHealth = m_Stats.MaxHealth + repairHoldStats.MaxHealth;
                m_Stats.MaxResilient = m_Stats.MaxResilient + repairHoldStats.MaxResilient;

                // 캐논 장착 스탯 적용
                if (m_CurrentEpCanonInstance != null)
                {
                    var canonData = m_CurrentEpCanonInstance.CanonData;
                    BigNum newEpATK = new BigNum(canonData.canEqATK) + (new BigNum(canonData.canEqATKup) * AccountMgr.EquipCannonDummy.Level);
                    BigNum newEpDEF = new BigNum(canonData.canEqDEF) + (new BigNum(canonData.canEqDEFup) * AccountMgr.EquipCannonDummy.Level);
                    m_Stats.MaxDamage = (m_Stats.MaxDamage + newEpATK) * canonData.canATKMultiplier;
                    m_Stats.MaxArmor = m_Stats.MaxArmor + newEpDEF;
                }
                if (m_CurrentEqRepair != null)
                {
                    var repairData = m_CurrentEqRepair.GetData();
                    BigNum newEpHP = new BigNum(repairData.RepEqHP) + (new BigNum(repairData.RepEqHPup) * AccountMgr.EquipRepairDummy.Level);
                    BigNum newEpREC = new BigNum(repairData.RepEqREC) + (new BigNum(repairData.RepEqRECup) * AccountMgr.EquipRepairDummy.Level);
                    m_Stats.MaxHealth = (m_Stats.MaxHealth + newEpHP) * repairData.RepHpMultiplier;
                    m_Stats.MaxResilient = (m_Stats.MaxResilient + newEpREC) * repairData.RepRecMultiplier;
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

        public void OnEquipRepairEvents(RepairDummy repairDummy)
        {
            AccountMgr.EquipRepairDummy = repairDummy;
            m_CurrentEqRepair = repairDummy;
            MergedAccountStatsForCharacter();
        }

        public void OnUnequipRepairEvents()
        {
            AccountMgr.EquipRepairDummy = null;
            m_CurrentEqRepair = null;
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

            m_BaseStats = new CommonStats();
            m_BaseStats.SetMaxDamage(m_Stats.MaxDamage);
            m_BaseStats.SetMaxHealth(m_Stats.MaxHealth);
            m_BaseStats.SetMaxShield(m_Stats.MaxShield);
            m_BaseStats.SetMaxArmor(m_Stats.MaxArmor);
            m_BaseStats.SetMaxResilient(m_Stats.MaxResilient);
            m_BaseStats.SetCriticalChance(m_Stats.CriticalChance);
            m_BaseStats.SetCriticalMultiplier(m_Stats.CriticalMultiplier);
            m_BaseStats.SetBossDamageMultiplier(m_Stats.BossDamageMultiplier);
            m_BaseStats.SetSkillEffectMultiplier(m_Stats.SkillEffectMultiplier);

            if (TryGetComponent<CanonExecutor>(out var canonExecutor))
            {
                m_CanonExecutor = canonExecutor;
                canonExecutor.onEquipEvents += OnEquipCannonEvents;
                canonExecutor.onUnequipEvents += OnUnequipCannonEvents;
            }
            if (TryGetComponent<RepairExecutor>(out var repairExecutor))
            {
                repairExecutor.onEquipEvents += OnEquipRepairEvents;
                repairExecutor.onUnequipEvents += OnUnequipRepairEvents;
            }

            m_Stats.ResetAll();

            AccountMgr.AddLevelUpEvent(MergedAccountStatsForCharacter);

            MergedAccountStatsForCharacter();
        }

        public void Init(CharacterStatus newBaseStat)
        {
            if (m_IsInitialized)
                return;

            m_IsInitialized = true;

            m_Stats = GetComponent<CharacterStatus>();
            if (m_Stats == null)
            {
                Debug.Log("Account Stats과 병합할 수 없습니다. CharacterStatus가 null입니다.");
            }

            m_BaseStats = new CommonStats();
            m_BaseStats.SetMaxDamage(newBaseStat.MaxDamage);
            m_BaseStats.SetMaxHealth(newBaseStat.MaxHealth);
            m_BaseStats.SetMaxArmor(newBaseStat.MaxArmor);
            m_BaseStats.SetMaxResilient(newBaseStat.MaxResilient);
            m_BaseStats.SetCriticalChance(newBaseStat.CriticalChance);
            m_BaseStats.SetCriticalMultiplier(newBaseStat.CriticalMultiplier);
            m_BaseStats.SetBossDamageMultiplier(newBaseStat.BossDamageMultiplier);
            m_BaseStats.SetSkillEffectMultiplier(newBaseStat.SkillEffectMultiplier);

            if (TryGetComponent<CanonExecutor>(out var canonExecutor))
            {
                m_CanonExecutor = canonExecutor;
                canonExecutor.onEquipEvents += OnEquipCannonEvents;
                canonExecutor.onUnequipEvents += OnUnequipCannonEvents;
            }
            if (TryGetComponent<RepairExecutor>(out var repairExecutor))
            {
                repairExecutor.onEquipEvents += OnEquipRepairEvents;
                repairExecutor.onUnequipEvents += OnUnequipRepairEvents;
            }

            m_Stats.MaxDamage = m_BaseStats.MaxDamage;
            m_Stats.MaxHealth = m_BaseStats.MaxHealth;
            m_Stats.MaxArmor = m_BaseStats.MaxArmor;
            m_Stats.MaxResilient = m_BaseStats.MaxResilient;
            m_Stats.ResetAll();

            AccountMgr.AddLevelUpEvent(MergedAccountStatsForCharacter);

            MergedAccountStatsForCharacter();
        }

        public void SetBaseStat(CommonStats newBaseStat)
        {
            m_BaseStats = newBaseStat;

            MergedAccountStatsForCharacter();
        }

        public void SetBaseStat(CharacterStatus newBaseStat)
        {
            m_BaseStats = new CommonStats();
            m_BaseStats.SetMaxDamage(newBaseStat.MaxDamage);
            m_BaseStats.SetMaxHealth(newBaseStat.MaxHealth);
            m_BaseStats.SetMaxArmor(newBaseStat.MaxArmor);
            m_BaseStats.SetMaxResilient(newBaseStat.MaxResilient);
            m_BaseStats.SetCriticalChance(newBaseStat.CriticalChance);
            m_BaseStats.SetCriticalMultiplier(newBaseStat.CriticalMultiplier);
            m_BaseStats.SetBossDamageMultiplier(newBaseStat.BossDamageMultiplier);
            m_BaseStats.SetSkillEffectMultiplier(newBaseStat.SkillEffectMultiplier);

            MergedAccountStatsForCharacter();
        }

        // Others


    } // Scope by class AccountStatInfoProvider

} // namespace Root