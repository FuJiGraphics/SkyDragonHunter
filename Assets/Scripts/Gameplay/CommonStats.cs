using UnityEngine;
using System;
using SkyDragonHunter.Structs;

namespace SkyDragonHunter.Gameplay
{
    public class CommonStats
    {
        public const float c_DefaultDamage = 10f;               // �⺻ ���ݷ�
        public const float c_DefaultHealth = 100f;              // �⺻ ü��
        public const float c_DefaultShield = 50f;               // �⺻ ��
        public const float c_DefaultArmor = 5f;                 // �⺻ ����
        public const float c_DefaultResilient = 0f;             // �⺻ ȸ����
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