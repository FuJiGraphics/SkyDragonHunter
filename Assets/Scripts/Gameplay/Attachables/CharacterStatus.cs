using UnityEngine;
using SkyDragonHunter.Managers;
using SkyDragonHunter.UI;
using SkyDragonHunter.Structs;
using SkyDragonHunter.Utility;
using System.Text;
using SkyDragonHunter.Interfaces;

namespace SkyDragonHunter.Gameplay
{
    public class CharacterStatus : MonoBehaviour
        , IStateResetHandler
    {
        // 필드 (Fields)
        [SerializeField] private string damage = CommonStats.c_DefaultDamage;
        [SerializeField] private string health = CommonStats.c_DefaultHealth;
        [SerializeField] private string shield = CommonStats.c_DefaultShield;
        [SerializeField] private string armor = CommonStats.c_DefaultArmor;
        [SerializeField] private string resilient = CommonStats.c_DefaultResilient;
        [SerializeField] private float criticalChance = CommonStats.c_DefaultCriticalChance;
        [SerializeField] private float criticalMultiplier = CommonStats.c_DefaultCriticalMultiplier;
        [SerializeField] private float bossDamageMultiplier = CommonStats.c_DefaultBossDamageMultiplier;
        [SerializeField] private float skillEffectMultiplier = CommonStats.c_DefaultSkillEffectMultiplier;

        // 속성 (Properties)
        public BigNum MaxDamage { get => m_CommonStats.MaxDamage; set => m_CommonStats.SetMaxDamage(value); }
        public BigNum MaxHealth { get => m_CommonStats.MaxHealth; set => m_CommonStats.SetMaxHealth(value); }
        public BigNum MaxShield { get => m_CommonStats.MaxShield; set => m_CommonStats.SetMaxShield(value); }
        public BigNum MaxArmor { get => m_CommonStats.MaxArmor; set => m_CommonStats.SetMaxArmor(value); }
        public BigNum MaxResilient { get => m_CommonStats.MaxResilient; set => m_CommonStats.SetMaxResilient(value); }

        public BigNum Damage { get => m_CommonStats.Damage; set => m_CommonStats.SetDamage(value); }
        public BigNum Health { get => m_CommonStats.Health; set => m_CommonStats.SetHealth(value); }
        public BigNum Shield { get => m_CommonStats.Shield; set => m_CommonStats.SetShield(value); }
        public BigNum Armor { get => m_CommonStats.Armor; set => m_CommonStats.SetArmor(value); }
        public BigNum Resilient { get => m_CommonStats.Resilient; set => m_CommonStats.SetResilient(value); }
        public float CriticalChance { get => m_CommonStats.CriticalChance; set => m_CommonStats.SetCriticalChance(value); }
        public float CriticalMultiplier { get => m_CommonStats.CriticalMultiplier; set => m_CommonStats.SetCriticalMultiplier(value); }
        public float BossDamageMultiplier { get => m_CommonStats.BossDamageMultiplier; set => m_CommonStats.SetBossDamageMultiplier(value); }
        public float SkillEffectMultiplier { get => m_CommonStats.SkillEffectMultiplier; set => m_CommonStats.SetSkillEffectMultiplier(value); }

        public bool IsFullHealth => m_CommonStats.IsFullHealth;
        public bool IsFullShield => m_CommonStats.IsFullShield;
        public bool IsFullDamage => m_CommonStats.IsFullDamage;
        public bool IsFullArmor => m_CommonStats.IsFullArmor;
        public bool IsFullResilient => m_CommonStats.IsFullResilient;

        // 외부 종속성 필드 (External dependencies field)
        private CommonStats m_CommonStats = new CommonStats();

        // 이벤트 (Events)
        // 유니티 (MonoBehaviour 기본 메서드)
        private void Awake()
        {
            Init();
        }

        private void Start()
        {
        }

        private void Update()
        {
#if UNITY_EDITOR
            UpdateParams();
#endif
        }

        // Public 메서드
        public void ResetAll() => m_CommonStats.ResetAll();
        public void ResetDamage() => m_CommonStats.ResetDamage();
        public void ResetHealth() => m_CommonStats.ResetHealth();
        public void ResetShield() => m_CommonStats.ResetShield();
        public void ResetArmor() => m_CommonStats.ResetArmor();
        public void ResetResilient() => m_CommonStats.ResetResilient();

        // Private 메서드

#if UNITY_EDITOR
        public void UpdateParams()
        {
            damage = m_CommonStats.Damage.ToString();
            health = m_CommonStats.Health.ToString();
            shield = m_CommonStats.Shield.ToString();
            armor = m_CommonStats.Armor.ToString();
            resilient = m_CommonStats.Resilient.ToString();
            criticalChance = m_CommonStats.CriticalChance;
            criticalMultiplier = m_CommonStats.CriticalMultiplier;
            bossDamageMultiplier = m_CommonStats.BossDamageMultiplier;
            skillEffectMultiplier = m_CommonStats.SkillEffectMultiplier;
        }
#endif

        private void Init()
        {
             m_CommonStats.SetMaxDamage(damage);
            m_CommonStats.SetMaxHealth(health);
            m_CommonStats.SetMaxShield(shield);
            m_CommonStats.SetMaxArmor(armor);
            m_CommonStats.SetMaxResilient(resilient);
            m_CommonStats.SetDamage(damage);
            m_CommonStats.SetHealth(health);
            m_CommonStats.SetShield(shield);
            m_CommonStats.SetArmor(armor);
            m_CommonStats.SetResilient(resilient);
            m_CommonStats.SetCriticalChance(criticalChance);
            m_CommonStats.SetCriticalMultiplier(criticalMultiplier);
            m_CommonStats.SetBossDamageMultiplier(bossDamageMultiplier);
            m_CommonStats.SetSkillEffectMultiplier(skillEffectMultiplier);
            m_CommonStats.ResetAll();
        }

        // Others
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append($"Damage: {Damage} Health: {Health} Shield: {Shield} Armor: {Armor} Resilient: {Resilient}");
            sb.Append($"CriticalChance: {CriticalChance}, CriticalMultiplier: {CriticalMultiplier}");
            sb.Append($"BossDamageMultiplier: {BossDamageMultiplier}, SkillEffectMultiplier: {SkillEffectMultiplier}");
            return sb.ToString();
        }

        public void ResetState()
        {
            ResetAll();
        }

    } // Scope by class CharacterStatus
} // namespace Root