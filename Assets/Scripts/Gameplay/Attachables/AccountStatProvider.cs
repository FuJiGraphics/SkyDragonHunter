using SkyDragonHunter.Managers;
using SkyDragonHunter.Structs;
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
            CommonStats artifactStats = AccountMgr.ArtifactStats;
            CommonStats canonHoldStats = AccountMgr.GetCanonHoldStats();
            CommonStats repairHoldStats = AccountMgr.GetRepairHoldStats();
            CommonStats socketStats = AccountMgr.GetSocketStat();

            double prevHealthWeight = (double)m_Stats.Health / (double)m_Stats.MaxHealth;

            var mergeDamage = (accStats.MaxDamage + artifactStats.MaxDamage + socketStats.MaxDamage + AccountMgr.DefaultGrowthStats.MaxDamage);
            var mergeHealth = (accStats.MaxHealth + artifactStats.MaxHealth + socketStats.MaxHealth + AccountMgr.DefaultGrowthStats.MaxHealth);
            var mergeArmor = (accStats.MaxArmor + artifactStats.MaxArmor + socketStats.MaxArmor + AccountMgr.DefaultGrowthStats.MaxArmor);
            var mergeRes = accStats.MaxResilient + artifactStats.MaxResilient + socketStats.MaxResilient + AccountMgr.DefaultGrowthStats.MaxResilient;
            var mergeCriMul = (accStats.CriticalMultiplier + artifactStats.CriticalMultiplier + socketStats.CriticalMultiplier + AccountMgr.DefaultGrowthStats.CriticalMultiplier);
            var mergeBossDamMul = (accStats.BossDamageMultiplier + artifactStats.BossDamageMultiplier + socketStats.BossDamageMultiplier);
            var mergeSkillMul = (accStats.SkillEffectMultiplier + artifactStats.SkillEffectMultiplier + socketStats.SkillEffectMultiplier);

            // 곱연산
            m_Stats.MaxDamage = m_FirstStats.MaxDamage * mergeDamage;
            m_Stats.MaxHealth = m_FirstStats.MaxHealth * mergeHealth;
            m_Stats.MaxArmor = m_FirstStats.MaxArmor * mergeArmor;
            m_Stats.MaxResilient = m_FirstStats.MaxResilient * mergeRes;

            // 합연산
            m_Stats.CriticalChance = m_FirstStats.CriticalChance + accStats.CriticalChance + artifactStats.CriticalChance;
            m_Stats.CriticalMultiplier = m_FirstStats.CriticalMultiplier + mergeCriMul;
            m_Stats.BossDamageMultiplier = m_FirstStats.BossDamageMultiplier + mergeBossDamMul;
            m_Stats.SkillEffectMultiplier = m_FirstStats.SkillEffectMultiplier + mergeSkillMul;

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
            m_Stats.Health *= prevHealthWeight;
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

            m_FirstStats = new CommonStats();
            m_FirstStats.SetMaxDamage(m_Stats.MaxDamage);
            m_FirstStats.SetMaxHealth(m_Stats.MaxHealth);
            m_FirstStats.SetMaxShield(m_Stats.MaxShield);
            m_FirstStats.SetMaxArmor(m_Stats.MaxArmor);
            m_FirstStats.SetMaxResilient(m_Stats.MaxResilient);
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
            if (TryGetComponent<RepairExecutor>(out var repairExecutor))
            {
                repairExecutor.onEquipEvents += OnEquipRepairEvents;
                repairExecutor.onUnequipEvents += OnUnequipRepairEvents;
            }

            m_Stats.ResetAll();

            AccountMgr.AddLevelUpEvent(MergedAccountStatsForCharacter);

            MergedAccountStatsForCharacter();
        }

        // Others

    } // Scope by class CrystalStatProvider
} // namespace SkyDragonHunter