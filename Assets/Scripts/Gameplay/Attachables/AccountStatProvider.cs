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
        private CommonStats m_SocketStats;
        private bool m_IsInitialized = false;

        // 이벤트 (Events)
        // 유니티 (MonoBehaviour 기본 메서드)
        // Public 메서드
        public void MergedAccountStatsForCharacter()
        {
            CommonStats accStats = AccountMgr.AccountStats;
            CommonStats canonHoldStats = GetCanonHoldStats();
            m_SocketStats = GetCalculateSocketStats();

            AlphaUnit prevLostHealth = m_Stats.MaxHealth - m_Stats.Health;

            var mergeDamage = (accStats.MaxDamage.Value + m_SocketStats.MaxDamage.Value + AccountMgr.DefaultGrowthStats.MaxDamage);
            var mergeHealth = (accStats.MaxHealth.Value + m_SocketStats.MaxHealth.Value + AccountMgr.DefaultGrowthStats.MaxHealth);
            var mergeArmor = (accStats.MaxArmor.Value + m_SocketStats.MaxArmor.Value + AccountMgr.DefaultGrowthStats.MaxArmor);
            var mergeRes = accStats.MaxResilient.Value + m_SocketStats.MaxResilient.Value + AccountMgr.DefaultGrowthStats.MaxResilient;
            var mergeCriMul = (accStats.CriticalMultiplier + m_SocketStats.CriticalMultiplier + AccountMgr.DefaultGrowthStats.CriticalMultiplier);
            var mergeBossDamMul = (accStats.BossDamageMultiplier + m_SocketStats.BossDamageMultiplier);
            var mergeSkillMul = (accStats.SkillEffectMultiplier + m_SocketStats.SkillEffectMultiplier);

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
                    m_Stats.MaxDamage = m_Stats.MaxDamage + BigInteger.Parse(m_CurrentEpCanonInstance.CanonData.canEqATK);
                    m_Stats.MaxArmor = m_Stats.MaxArmor + BigInteger.Parse(m_CurrentEpCanonInstance.CanonData.canEqDEF);
                }
            }

            m_Stats.ResetDamage();
            m_Stats.ResetHealth();
            m_Stats.ResetArmor();
            m_Stats.ResetResilient();
            m_Stats.Health -= prevLostHealth;
        }

        public void RegisterEquipCanon(CanonBase canonInstance)
        {
            m_CurrentEpCanonInstance = canonInstance;
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
                canonExecutor.onEquipEvents += RegisterEquipCanon;
            }

            m_Stats.ResetAll();

            AccountMgr.onLevelUpEvents += MergedAccountStatsForCharacter;

            MergedAccountStatsForCharacter();
        }

        private CommonStats GetCanonHoldStats()
        {
            var canons = AccountMgr.Canons;
            CommonStats stats = new CommonStats();
            for (int i = 0; i < canons.Length; ++i)
            {
                if (canons[i].TryGetComponent<CanonBase>(out var canonBase))
                {
                    var data = canonBase.CanonData;
                    if (i == 0)
                    {
                        stats.SetMaxDamage(data.canHoldATK);
                        stats.SetMaxArmor(data.canHoldDEF);
                    }
                    else
                    {
                        stats.SetMaxDamage(stats.MaxDamage.Value + BigInteger.Parse(data.canHoldATK));
                        stats.SetMaxArmor(stats.MaxArmor.Value + BigInteger.Parse(data.canHoldDEF));
                    }
                }
            }
            return stats;
        }

        private CommonStats GetCalculateSocketStats()
        {
            CommonStats result = new CommonStats();
            result.ResetAllZero();

            var socketMap = AccountMgr.SocketMap;
            foreach (var socketList in socketMap)
            {
                foreach (var socket in socketList.Value)
                {
                    switch (socket.Type)
                    {
                        case MasterySockeyType.Damage:
                            result.SetMaxDamage(socket.Stat);
                            break;
                        case MasterySockeyType.Health:
                            result.SetMaxHealth(socket.Stat);
                            break;
                        case MasterySockeyType.Armor:
                            result.SetMaxArmor(socket.Stat);
                            break;
                        case MasterySockeyType.Resilient:
                            result.SetMaxResilient(socket.Stat);
                            break;
                        case MasterySockeyType.CriticalMultiplier:
                            result.SetCriticalMultiplier((float)socket.Multiplier);
                            break;
                        case MasterySockeyType.BossDamageMultiplier:
                            result.SetBossDamageMultiplier((float)socket.Multiplier);
                            break;
                        case MasterySockeyType.SkillEffectMultiplier:
                            result.SetSkillEffectMultiplier((float)socket.Multiplier);
                            break;
                    }
                }
            }

            return result;
        }

        // Others

    } // Scope by class CrystalStatProvider
} // namespace SkyDragonHunter