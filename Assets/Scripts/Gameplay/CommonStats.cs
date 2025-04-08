using UnityEngine;
using System;
using SkyDragonHunter.Structs;
using System.Numerics;
using SkyDragonHunter.Managers;

namespace SkyDragonHunter.Gameplay
{
    public class CommonStats
    {
        public const string c_DefaultDamage = "10";               // �⺻ ���ݷ�
        public const string c_DefaultHealth = "100";              // �⺻ ü��
        public const string c_DefaultShield = "50";               // �⺻ ��
        public const string c_DefaultArmor = "5";                 // �⺻ ����
        public const string c_DefaultResilient = "0";             // �⺻ ȸ����
        public const float c_DefaultCriticalChance = 0f;        // �⺻ ġ��Ÿ Ȯ��
        public const float c_DefaultCriticalMultiplier = 2f;    // �⺻ ġ��Ÿ ���� ���� 200%
        public const float c_DefaultBossDamageMultiplier = 1f;  // �⺻ ���� ���� ���� 100%
        public const float c_DefaultSkillEffectMultiplier = 1f; // �⺻ ��ų ���� ���� 100%

        public AlphaUnit MaxDamage { get; private set; }    // �ִ� ���ݷ�
        public AlphaUnit Damage { get; private set; }       // ���� ���ݷ�

        public AlphaUnit MaxHealth { get; private set; }    // �ִ� HP
        public AlphaUnit Health { get; private set; }       // ���� HP

        public AlphaUnit MaxShield { get; private set; }    // �ִ� ��
        public AlphaUnit Shield { get; private set; }       // ���� ��

        public AlphaUnit MaxArmor { get; private set; }     // �ִ� ����
        public AlphaUnit Armor { get; private set; }        // ���� ����

        public AlphaUnit MaxResilient  { get; private set; }  // �ִ� ȸ����
        public AlphaUnit Resilient  { get; private set; }     // ���� ȸ����

        public float CriticalChance { get; private set; }        // ġ��Ÿ Ȯ��
        public float CriticalMultiplier { get; private set; }    // ġ��Ÿ ����
        public float BossDamageMultiplier { get; private set; }  // ���� ���ط� ����
        public float SkillEffectMultiplier { get; private set; } // ��ų ȿ�� ����

        public bool IsFullDamage => Damage.Equals(MaxDamage);
        public bool IsFullHealth => Health.Equals(MaxHealth);
        public bool IsFullShield => Shield.Equals(MaxShield);
        public bool IsFullArmor => Armor.Equals(MaxArmor);
        public bool IsFullResilient => Resilient.Equals(MaxResilient );

        public CommonStats()
        {
            MaxDamage = Damage = BigInteger.Parse(c_DefaultDamage);
            MaxHealth = Health = BigInteger.Parse(c_DefaultHealth);
            MaxShield = Shield = BigInteger.Parse(c_DefaultShield);
            MaxArmor = Armor = BigInteger.Parse(c_DefaultArmor);
            MaxResilient = Resilient = BigInteger.Parse(c_DefaultResilient);
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

        public void SetMaxDamage(BigInteger value)
        {
            MaxDamage = Math2DHelper.Max(0, value);
            Damage = Math2DHelper.Min(Damage.Value, MaxDamage.Value);
        }

        public void SetMaxHealth(BigInteger value)
        {
            MaxHealth = Math2DHelper.Max(0, value);
            Health = Math2DHelper.Min(Health.Value, MaxHealth.Value);
        }

        public void SetMaxShield(BigInteger value)
        {
            MaxShield = Math2DHelper.Max(0, value);
            Shield = Math2DHelper.Min(Shield.Value, MaxShield.Value);
        }

        public void SetMaxArmor(BigInteger value)
        {
            MaxArmor = Math2DHelper.Max(0, value);
            Armor = Math2DHelper.Min(Armor.Value, MaxArmor.Value);
        }

        public void SetMaxResilient(BigInteger value)
        {
            MaxResilient = Math2DHelper.Max(0, value);
            Resilient = Math2DHelper.Min(Resilient.Value, MaxResilient.Value);
        }

        public void SetMaxDamage(string value)
            => SetMaxDamage(BigInteger.Parse(value));
        public void SetMaxHealth(string value)
            => SetMaxHealth(BigInteger.Parse(value));
        public void SetMaxShield(string value)
            => SetMaxShield(BigInteger.Parse(value));
        public void SetMaxArmor(string value)
            => SetMaxArmor(BigInteger.Parse(value));
        public void SetMaxResilient(string value)
            => SetMaxResilient(BigInteger.Parse(value));

        public void SetDamage(BigInteger value)
            => Damage = Math2DHelper.Clamp(value, 0, MaxDamage.Value);
        public void SetHealth(BigInteger value)
            => Health = Math2DHelper.Clamp(value, 0, MaxHealth.Value);
        public void SetShield(BigInteger value)
            => Shield = Math2DHelper.Clamp(value, 0, MaxShield.Value);
        public void SetArmor(BigInteger value)
            => Armor = Math2DHelper.Clamp(value, 0, MaxArmor.Value);
        public void SetResilient(BigInteger value)
            => Resilient = Math2DHelper.Clamp(value, 0, MaxResilient.Value);

        public void SetDamage(string value)
            => Damage = Math2DHelper.Clamp(BigInteger.Parse(value), 0, MaxDamage.Value);
        public void SetHealth(string value)
            => Health = Math2DHelper.Clamp(BigInteger.Parse(value), 0, MaxHealth.Value);
        public void SetShield(string value)
            => Shield = Math2DHelper.Clamp(BigInteger.Parse(value), 0, MaxShield.Value);
        public void SetArmor(string value)
            => Armor = Math2DHelper.Clamp(BigInteger.Parse(value), 0, MaxArmor.Value);
        public void SetResilient(string value)
            => Resilient = Math2DHelper.Clamp(BigInteger.Parse(value), 0, MaxResilient.Value);

        // 0.0 ~ 1.0 ������ ��
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