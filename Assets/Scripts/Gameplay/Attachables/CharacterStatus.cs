using UnityEngine;
using SkyDragonHunter.Managers;
using SkyDragonHunter.UI;
using SkyDragonHunter.Structs;
using SkyDragonHunter.Utility;
using System.Text;
using SkyDragonHunter.Interfaces;
using System;
using Unity.VisualScripting;

namespace SkyDragonHunter.Gameplay
{
    public enum StatusChangedEventType
    {
        MaxDamage,
        Damage,
        MaxHealth,
        Health,
        MaxShield,
        Shield,
        MaxArmor,
        Armor,
        MaxResilient,
        Resilient,
        All,
    }

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
        public BigNum MaxDamage
        {
            get => m_CommonStats.MaxDamage;
            set
            {
                if (!m_CommonStats.MaxDamage.Equals(value))
                {
                    m_CommonStats.SetMaxDamage(value);
                    m_MaxDamageChangedEvent?.Invoke(value);
                }
            }
        }

        public BigNum Damage
        {
            get => m_CommonStats.Damage;
            set
            {
                if (!m_CommonStats.Damage.Equals(value))
                {
                    m_CommonStats.SetDamage(value);
                    m_DamageChangedEvent?.Invoke(value);
                }
            }
        }

        public BigNum MaxHealth
        {
            get => m_CommonStats.MaxHealth;
            set
            {
                if (!m_CommonStats.MaxHealth.Equals(value))
                {
                    m_CommonStats.SetMaxHealth(value);
                    m_MaxHealthChangedEvent?.Invoke(value);
                }
            }
        }

        public BigNum Health
        {
            get => m_CommonStats.Health;
            set
            {
                if (!m_CommonStats.Health.Equals(value))
                {
                    m_CommonStats.SetHealth(value);
                    m_HealthChangedEvent?.Invoke(value);
                }
            }
        }

        public BigNum MaxShield
        {
            get => m_CommonStats.MaxShield;
            set
            {
                if (!m_CommonStats.MaxShield.Equals(value))
                {
                    m_CommonStats.SetMaxShield(value);
                    m_MaxShieldChangedEvent?.Invoke(value);
                }
            }
        }

        public BigNum Shield
        {
            get => m_CommonStats.Shield;
            set
            {
                if (!m_CommonStats.Shield.Equals(value))
                {
                    m_CommonStats.SetShield(value);
                    m_ShieldChangedEvent?.Invoke(value);
                }
            }
        }

        public BigNum MaxArmor
        {
            get => m_CommonStats.MaxArmor;
            set
            {
                if (!m_CommonStats.MaxArmor.Equals(value))
                {
                    m_CommonStats.SetMaxArmor(value);
                    m_MaxArmorChangedEvent?.Invoke(value);
                }
            }
        }

        public BigNum Armor
        {
            get => m_CommonStats.Armor;
            set
            {
                if (!m_CommonStats.Armor.Equals(value))
                {
                    m_CommonStats.SetArmor(value);
                    m_ArmorChangedEvent?.Invoke(value);
                }
            }
        }

        public BigNum MaxResilient
        {
            get => m_CommonStats.MaxResilient;
            set
            {
                if (!m_CommonStats.MaxResilient.Equals(value))
                {
                    m_CommonStats.SetMaxResilient(value);
                    m_MaxResilientChangedEvent?.Invoke(value);
                }
            }
        }

        public BigNum Resilient
        {
            get => m_CommonStats.Resilient;
            set
            {
                if (!m_CommonStats.Resilient.Equals(value))
                {
                    m_CommonStats.SetResilient(value);
                    m_ResilientChangedEvent?.Invoke(value);
                }
            }
        }

        public float CriticalChance
        {
            get => m_CommonStats.CriticalChance;
            set => m_CommonStats.SetCriticalChance(value);
        }

        public float CriticalMultiplier
        {
            get => m_CommonStats.CriticalMultiplier;
            set => m_CommonStats.SetCriticalMultiplier(value);
        }

        public float BossDamageMultiplier
        {
            get => m_CommonStats.BossDamageMultiplier;
            set => m_CommonStats.SetBossDamageMultiplier(value);
        }

        public float SkillEffectMultiplier
        {
            get => m_CommonStats.SkillEffectMultiplier;
            set => m_CommonStats.SetSkillEffectMultiplier(value);
        }

        public float AttackSpeedOffset { get; set; } = 0f;

        public bool IsFullHealth => m_CommonStats.IsFullHealth;
        public bool IsFullShield => m_CommonStats.IsFullShield;
        public bool IsFullDamage => m_CommonStats.IsFullDamage;
        public bool IsFullArmor => m_CommonStats.IsFullArmor;
        public bool IsFullResilient => m_CommonStats.IsFullResilient;

        // 외부 종속성 필드 (External dependencies field)
        private CommonStats m_CommonStats = new CommonStats();

        // 이벤트 (Events)
        private event Action<BigNum> m_MaxDamageChangedEvent;
        private event Action<BigNum> m_DamageChangedEvent;
        private event Action<BigNum> m_MaxHealthChangedEvent;
        private event Action<BigNum> m_HealthChangedEvent;
        private event Action<BigNum> m_MaxShieldChangedEvent;
        private event Action<BigNum> m_ShieldChangedEvent;
        private event Action<BigNum> m_MaxArmorChangedEvent;
        private event Action<BigNum> m_ArmorChangedEvent;
        private event Action<BigNum> m_MaxResilientChangedEvent;
        private event Action<BigNum> m_ResilientChangedEvent;

        // 유니티 (MonoBehaviour 기본 메서드)
        private void Awake()
        {
            Init();
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

        public void AddChangedEvent(StatusChangedEventType type, Action<BigNum> action)
        {
            switch (type)
            {
                case StatusChangedEventType.MaxDamage: m_MaxDamageChangedEvent += action; break;
                case StatusChangedEventType.Damage: m_DamageChangedEvent += action; break;
                case StatusChangedEventType.MaxHealth: m_MaxHealthChangedEvent += action; break;
                case StatusChangedEventType.Health: m_HealthChangedEvent += action; break;
                case StatusChangedEventType.MaxShield: m_MaxShieldChangedEvent += action; break;
                case StatusChangedEventType.Shield: m_ShieldChangedEvent += action; break;
                case StatusChangedEventType.MaxArmor: m_MaxArmorChangedEvent += action; break;
                case StatusChangedEventType.Armor: m_ArmorChangedEvent += action; break;
                case StatusChangedEventType.MaxResilient: m_MaxResilientChangedEvent += action; break;
                case StatusChangedEventType.Resilient: m_ResilientChangedEvent += action; break;
            }
        }

        public void RemoveChangedEvent(StatusChangedEventType type, Action<BigNum> action)
        {
            switch (type)
            {
                case StatusChangedEventType.MaxDamage: m_MaxDamageChangedEvent -= action; break;
                case StatusChangedEventType.Damage: m_DamageChangedEvent -= action; break;
                case StatusChangedEventType.MaxHealth: m_MaxHealthChangedEvent -= action; break;
                case StatusChangedEventType.Health: m_HealthChangedEvent -= action; break;
                case StatusChangedEventType.MaxShield: m_MaxShieldChangedEvent -= action; break;
                case StatusChangedEventType.Shield: m_ShieldChangedEvent -= action; break;
                case StatusChangedEventType.MaxArmor: m_MaxArmorChangedEvent -= action; break;
                case StatusChangedEventType.Armor: m_ArmorChangedEvent -= action; break;
                case StatusChangedEventType.MaxResilient: m_MaxResilientChangedEvent -= action; break;
                case StatusChangedEventType.Resilient: m_ResilientChangedEvent -= action; break;
            }
        }


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