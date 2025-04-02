using SkyDragonHunter.Gameplay;
using SkyDragonHunter.Managers;
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

        // 이벤트 (Events)
        // 유니티 (MonoBehaviour 기본 메서드)
        private void Awake()
        {
            Init();
            MergedAccountStatsForCharacter();
            AccountMgr.onLevelUpEvents += MergedAccountStatsForCharacter;
        }

        [ContextMenu("테스트용 레벨 업")]
        private void LevelUp()
        {
            AccountMgr.LevelUp();
        }

        // Public 메서드
        public void MergedAccountStatsForCharacter()
        {
            if (m_Stats == null)
            {
                Debug.Log("Account Stats과 병합할 수 없습니다. CharacterStatus가 null입니다.");
            }

            CommonStats accStats = AccountMgr.AccountStats;

            // 곱연산
            m_Stats.MaxDamage = m_FirstStats.MaxDamage.Value * accStats.MaxDamage.Value;
            m_Stats.MaxHealth = m_FirstStats.MaxHealth.Value * accStats.MaxHealth.Value;
            m_Stats.MaxArmor = m_FirstStats.MaxArmor.Value * accStats.MaxArmor.Value;
            m_Stats.MaxResilient = m_FirstStats.MaxResilient.Value * accStats.MaxResilient.Value;

            // 합연산
            m_Stats.CriticalChance = m_FirstStats.CriticalChance + accStats.CriticalChance;
            m_Stats.CriticalMultiplier = m_FirstStats.CriticalMultiplier + accStats.CriticalMultiplier;
            m_Stats.BossDamageMultiplier = m_FirstStats.BossDamageMultiplier + accStats.BossDamageMultiplier;
            m_Stats.SkillEffectMultiplier = m_FirstStats.SkillEffectMultiplier + accStats.SkillEffectMultiplier;
            
            m_Stats.ResetAll();
        }

        // Private 메서드
        private void Init()
        {
            m_Stats = GetComponent<CharacterStatus>();
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
        }

        // Others

    } // Scope by class CrystalStatProvider
} // namespace SkyDragonHunter