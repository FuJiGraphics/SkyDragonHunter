using UnityEngine;
using System;
using SkyDragonHunter.Structs;

namespace SkyDragonHunter.Gameplay
{
    public class CommonStats
    {
        public const float c_DefaultDamage = 10f;               // 기본 공격력
        public const float c_DefaultHealth = 100f;              // 기본 체력
        public const float c_DefaultShield = 50f;               // 기본 방어막
        public const float c_DefaultArmor = 5f;                 // 기본 방어력
        public const float c_DefaultResilient = 0f;             // 기본 회복력
        public const float c_DefaultCriticalChance = 0f;        // 기본 치명타 확률
        public const float c_DefaultCriticalMultiplier = 2f;    // 기본 치명타 피해 배율 200%
        public const float c_DefaultBossDamageMultiplier = 1f;  // 기본 보스 피해 배율 100%
        public const float c_DefaultSkillEffectMultiplier = 1f; // 기본 스킬 피해 배율 100%

        public AlphaUnit MaxDamage { get; private set; }    // 최대 공격력
        public AlphaUnit Damage { get; private set; }       // 현재 공격력

        public AlphaUnit MaxHealth { get; private set; }    // 최대 HP
        public AlphaUnit Health { get; private set; }       // 현재 HP

        public AlphaUnit MaxShield { get; private set; }    // 최대 방어막
        public AlphaUnit Shield { get; private set; }       // 현재 방어막

        public AlphaUnit MaxArmor { get; private set; }     // 최대 방어력
        public AlphaUnit Armor { get; private set; }        // 현재 방어력

        public AlphaUnit MaxResilient  { get; private set; }  // 최대 회복력
        public AlphaUnit Resilient  { get; private set; }     // 현재 회복력

        public float CriticalChance { get; private set; }        // 치명타 확률
        public float CriticalMultiplier { get; private set; }    // 치명타 배율
        public float BossDamageMultiplier { get; private set; }  // 보스 피해량 배율
        public float SkillEffectMultiplier { get; private set; } // 스킬 효과 증가

        public bool IsFullDamage => Damage.Equals(MaxDamage);
        public bool IsFullHealth => Health.Equals(MaxHealth);
        public bool IsFullShield => Shield.Equals(MaxShield);
        public bool IsFullArmor => Armor.Equals(MaxArmor);
        public bool IsFullResilient => Resilient.Equals(MaxResilient );

        public CommonStats()
        {
            MaxDamage = Damage = c_DefaultDamage;
            MaxHealth = Health = c_DefaultHealth;
            MaxShield = Shield = c_DefaultShield;
            MaxArmor = Armor = c_DefaultArmor;
            MaxResilient = Resilient = c_DefaultResilient;
            CriticalChance = c_DefaultCriticalChance;
            CriticalMultiplier = c_DefaultCriticalMultiplier;
            BossDamageMultiplier = c_DefaultBossDamageMultiplier;
            SkillEffectMultiplier = c_DefaultSkillEffectMultiplier;
        }

        public void ResetDamage() => Damage = MaxDamage;
        public void ResetHealth() => Health = MaxHealth;
        public void ResetShield() => Shield = MaxShield;
        public void ResetArmor() => Armor = MaxArmor;
        public void ResetResilient() => Resilient = MaxResilient;
        public void ResetAll()
        {
            ResetDamage();
            ResetHealth();
            ResetShield();
            ResetArmor();
            ResetResilient();
        }

        public void SetMaxDamage(double value)
            => MaxDamage = Math.Max(0.0, value);
        public void SetMaxHealth(double value)
            => MaxHealth = Math.Max(0.0, value);
        public void SetMaxShield(double value)
            => MaxShield = Math.Max(0.0, value);
        public void SetMaxArmor(double value)
            => MaxArmor = Math.Max(0.0, value);
        public void SetMaxResilient(double value)
            => MaxResilient = Math.Max(0.0, value);

        public void SetDamage(double value)
            => Damage = Math.Clamp(value, 0.0, MaxDamage.Value);
        public void SetHealth(double value)
            => Health = Math.Clamp(value, 0.0, MaxHealth.Value);
        public void SetShield(double value)
            => Shield = Math.Clamp(value, 0.0, MaxShield.Value);
        public void SetArmor(double value)
            => Armor = Math.Clamp(value, 0.0, MaxArmor.Value);
        public void SetResilient(double value)
            => Resilient  = Math.Clamp(value, 0.0, MaxResilient.Value);

        // 0.0 ~ 1.0 사이의 값
        public void SetCriticalChance(float percent)
            => CriticalChance = Mathf.Clamp01(percent);
        public void SetCriticalMultiplier(float multiplier)
            => CriticalMultiplier = Math.Max(0f, multiplier);
        public void SetBossDamageMultiplier(float multiplier)
            => BossDamageMultiplier = Math.Max(0f, multiplier);
        public void SetSkillEffectMultiplier(float multiplier)
            => SkillEffectMultiplier = Math.Max(0f, multiplier);

        public override string ToString()
        {
            return $"HP: {Health}/{MaxHealth}, DMG: {Damage}, Crit: {CriticalChance * 100f:F1}%";
        }

    }; // struct CommonStats

} // namespace SkyDragonHunter.Structs