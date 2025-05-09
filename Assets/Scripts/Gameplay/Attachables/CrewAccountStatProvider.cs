using SkyDragonHunter.Entities;
using SkyDragonHunter.Managers;
using SkyDragonHunter.Structs;
using SkyDragonHunter.Temp;
using UnityEngine;

namespace SkyDragonHunter.Gameplay {

    public class CrewAccountStatProvider : MonoBehaviour
    {
        private NewCrewControllerBT crewBT;
        private TempCrewLevelExpData cachedCrewData;

        private BigNum damageMultipler;
        private BigNum healthMultipler;
        private BigNum armorMultipler;
        private BigNum resMultiplier;
        private BigNum bossDamageMultiplier;
        private BigNum skillEffectMultiplier;
        private float critDamageMultiplier;

        private float additionalCritRate;

        // Unity Methods
        private void Awake()
        {
            crewBT = GetComponent<NewCrewControllerBT>();
            if( crewBT == null )
            {
                Debug.LogError($"CrewBT cannot be null");
            }
            if (!TempCrewLevelExpContainer.TryGetTempCrewData(crewBT.ID, out var crewData))
            {
                Debug.LogError($"crewData cannot be null [ GO: {gameObject.name} / ID: {crewBT.ID}]");
            }
            else
            {
                cachedCrewData = crewData;
            }
        }

        private void Start()
        {
            AccountMgr.AddLevelUpEvent(ApplyNewStatus);
            cachedCrewData.RegisterLeveledUpEvent(ApplyNewStatus);
        }

        private void OnDestroy()
        {
            AccountMgr.RemoveLevelUpEvent(ApplyNewStatus);
            cachedCrewData.ClearLeveledUpEvent();
        }

        // Public Methods
        public void ApplyNewStatus()
        {
            if (crewBT == null || cachedCrewData == null)
            {
                Debug.LogError($"Cannot apply new status, CrewBT or CrewData null");
                return;
            }
            UpdateStatMultiplier();
            var basicStat = cachedCrewData.BasicStat;
            crewBT.crewStatus.status.MaxDamage = basicStat.MaxDamage * damageMultipler;
            crewBT.crewStatus.status.MaxHealth = basicStat.MaxHealth * healthMultipler;
            crewBT.crewStatus.status.MaxArmor = basicStat.MaxArmor * armorMultipler;
            crewBT.crewStatus.status.MaxResilient = basicStat.MaxResilient * resMultiplier;
            crewBT.crewStatus.status.ResetAll();
        }

        // Private Methods
        private void UpdateStatMultiplier()
        {
            CommonStats accStats = AccountMgr.AccountStats;
            CommonStats artifactStats = AccountMgr.ArtifactStats;
            CommonStats canonHoldStats = AccountMgr.GetCanonHoldStats();
            CommonStats repairHoldStats = AccountMgr.GetRepairHoldStats();
            CommonStats socketStats = AccountMgr.GetSocketStat();

            damageMultipler = (accStats.MaxDamage + artifactStats.MaxDamage + socketStats.MaxDamage + AccountMgr.DefaultGrowthStats.MaxDamage);
            healthMultipler = (accStats.MaxHealth + artifactStats.MaxHealth + socketStats.MaxHealth + AccountMgr.DefaultGrowthStats.MaxHealth);
            armorMultipler = (accStats.MaxArmor + artifactStats.MaxArmor + socketStats.MaxArmor + AccountMgr.DefaultGrowthStats.MaxArmor);
            resMultiplier = accStats.MaxResilient + artifactStats.MaxResilient + socketStats.MaxResilient + AccountMgr.DefaultGrowthStats.MaxResilient;
            critDamageMultiplier = (accStats.CriticalMultiplier + artifactStats.CriticalMultiplier + socketStats.CriticalMultiplier + AccountMgr.DefaultGrowthStats.CriticalMultiplier);
            bossDamageMultiplier = (accStats.BossDamageMultiplier + artifactStats.BossDamageMultiplier + socketStats.BossDamageMultiplier);
            skillEffectMultiplier = (accStats.SkillEffectMultiplier + artifactStats.SkillEffectMultiplier + socketStats.SkillEffectMultiplier);
            additionalCritRate = accStats.CriticalChance + artifactStats.CriticalChance;
        }
    } // Scope by class CrewAccountStatProvider
} // namespace Root