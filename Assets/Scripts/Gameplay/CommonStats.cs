using UnityEngine;
using System;
using SkyDragonHunter.Structs;
using System.Numerics;
using SkyDragonHunter.Managers;

namespace SkyDragonHunter.Gameplay
{
    public class CommonStats
    {
        public const string c_DefaultDamage = "10";               // 기본 공격력
        public const string c_DefaultHealth = "100";              // 기본 체력
        public const string c_DefaultShield = "50";               // 기본 방어막
        public const string c_DefaultArmor = "5";                 // 기본 방어력
        public const string c_DefaultResilient = "0";             // 기본 회복력
        public const float c_DefaultCriticalChance = 0f;        // 기본 치명타 확률
        public const float c_DefaultCriticalMultiplier = 2f;    // 기본 치명타 피해 배율 200%
        public const float c_DefaultBossDamageMultiplier = 1f;  // 기본 보스 피해 배율 100%
        public const float c_DefaultSkillEffectMultiplier = 1f; // 기본 스킬 피해 배율 100%

        public BigNum MaxDamage { get; private set; }    // 최대 공격력
        public BigNum Damage { get; private set; }       // 현재 공격력

        public BigNum MaxHealth { get; private set; }    // 최대 HP
        public BigNum Health { get; private set; }       // 현재 HP

        public BigNum MaxShield { get; private set; }    // 최대 방어막
        public BigNum Shield { get; private set; }       // 현재 방어막

        public BigNum MaxArmor { get; private set; }     // 최대 방어력
        public BigNum Armor { get; private set; }        // 현재 방어력

        public BigNum MaxResilient  { get; private set; }  // 최대 회복력
        public BigNum Resilient  { get; private set; }     // 현재 회복력

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
            MaxDamage = Damage = new BigNum(c_DefaultDamage);
            MaxHealth = Health = new BigNum(c_DefaultHealth);
            MaxShield = Shield = new BigNum(c_DefaultShield);
            MaxArmor = Armor = new BigNum(c_DefaultArmor);
            MaxResilient = Resilient = new BigNum(c_DefaultResilient);
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

        public void ResetAllZero()
        {
            MaxDamage = Damage = 0;
            MaxHealth = Health = 0;
            MaxShield = Shield = 0;
            MaxArmor = Armor = 0;
            MaxResilient = Resilient = 0;
            CriticalChance = 0f;
            CriticalMultiplier = 0f;
            BossDamageMultiplier = 0f;
            SkillEffectMultiplier = 0f;
        }

        public void SetMaxDamage(BigNum value)
        {
            MaxDamage = Math2DHelper.Max(0, value);
        }

        public void SetMaxHealth(BigNum value)
        {
            MaxHealth = Math2DHelper.Max(0, value);
        }

        public void SetMaxShield(BigNum value)
        {
            MaxShield = Math2DHelper.Max(0, value);
        }

        public void SetMaxArmor(BigNum value)
        {
            MaxArmor = Math2DHelper.Max(0, value);
        }

        public void SetMaxResilient(BigNum value)
        {
            MaxResilient = Math2DHelper.Max(0, value);
        }

        public void SetMaxDamage(string value)
            => SetMaxDamage(new BigNum(value));
        public void SetMaxHealth(string value)
            => SetMaxHealth(new BigNum(value));
        public void SetMaxShield(string value)
            => SetMaxShield(new BigNum(value));
        public void SetMaxArmor(string value)
            => SetMaxArmor(new BigNum(value));
        public void SetMaxResilient(string value)
            => SetMaxResilient(new BigNum(value));

        public void SetDamage(BigNum value)
            => Damage = Math2DHelper.Clamp(value, 0, MaxDamage);
        public void SetHealth(BigNum value)
            => Health = Math2DHelper.Clamp(value, 0, MaxHealth);
        public void SetShield(BigNum value)
            => Shield = Math2DHelper.Clamp(value, 0, MaxShield);
        public void SetArmor(BigNum value)
            => Armor = Math2DHelper.Clamp(value, 0, MaxArmor);
        public void SetResilient(BigNum value)
            => Resilient = Math2DHelper.Clamp(value, 0, MaxResilient);

        public void SetDamage(string value)
            => Damage = Math2DHelper.Clamp(new BigNum(value), 0, MaxDamage);
        public void SetHealth(string value)
            => Health = Math2DHelper.Clamp(new BigNum(value), 0, MaxHealth);
        public void SetShield(string value)
            => Shield = Math2DHelper.Clamp(new BigNum(value), 0, MaxShield);
        public void SetArmor(string value)
            => Armor = Math2DHelper.Clamp(new BigNum(value), 0, MaxArmor);
        public void SetResilient(string value)
            => Resilient = Math2DHelper.Clamp(new BigNum(value), 0, MaxResilient);

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