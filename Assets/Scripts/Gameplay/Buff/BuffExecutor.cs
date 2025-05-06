using SkyDragonHunter.Interfaces;
using SkyDragonHunter.Managers;
using SkyDragonHunter.Structs;
using SkyDragonHunter.Tables;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SkyDragonHunter.Gameplay {

    public class BuffExecutor : MonoBehaviour
        , IDestructible
    {
        // 필드 (Fields)
        [SerializeField] private Transform m_BuffAnchor;
        [SerializeField] private CharacterStatus m_Status;

        private Dictionary<BuffType, Dictionary<BuffStatType, Coroutine>> m_CommandList; 
        private Dictionary<BuffStatType, BigNum> m_OriginalStatCache = new();

        // 속성 (Properties)
        // 외부 종속성 필드 (External dependencies field)
        // 이벤트 (Events)
        // 유니티 (MonoBehaviour 기본 메서드)
        private void Awake()
        {
            if (m_Status == null)
            {
                m_Status = GetComponent<CharacterStatus>();
            }

            if (m_CommandList == null)
            {
                m_CommandList = new()
                {
                    { BuffType.Buff, new() },
                    { BuffType.Debuff, new() }
                };
            }
        }

        private void OnDestroy()
        {
            StopAll();
        }

        // Public 메서드
        public void Execute(BuffData target)
        {
            if (m_CommandList[target.BuffApply].ContainsKey(target.BuffStatType))
                return;

            Coroutine coroutine = StartCoroutine(CoBuff(target));
            m_CommandList[target.BuffApply].Add(target.BuffStatType, coroutine);
        }

        public void Stop(BuffData target)
        {
            if (m_CommandList[target.BuffApply].ContainsKey(target.BuffStatType))
            {
                StopCoroutine(m_CommandList[target.BuffApply][target.BuffStatType]);
                m_CommandList[target.BuffApply].Remove(target.BuffStatType);
            }
        }

        public void StopAll()
        {
            foreach (var command in m_CommandList)
            {
                foreach (var coroutine in command.Value)
                {
                    StopCoroutine(coroutine.Value);
                }
                command.Value.Clear();
            }
            m_CommandList.Clear();
        }

        // Private 메서드
        private bool HasBuff(BuffData target)
        {
            bool result = false;
            if (m_CommandList[target.BuffApply].ContainsKey(target.BuffStatType))
            {
                result = true;
            }
            return result;
        }
        
        private void RemoveBuff(BuffData target)
        {
            m_CommandList[target.BuffApply].Remove(target.BuffStatType);
        }

        private void Apply(BuffData target)
        {
            var statType = target.BuffStatType;
            if (!m_OriginalStatCache.ContainsKey(statType))
            {
                m_OriginalStatCache[statType] = GetStatValue(statType);
            }

            BigNum modified = m_OriginalStatCache[statType] * target.EffectiveMultiplier;
            SetStatValue(statType, modified);

            if (statType == BuffStatType.Damage)
                m_Status.ResetDamage();
        }

        private void Revert(BuffData target)
        {
            var statType = target.BuffStatType;
            if (m_OriginalStatCache.TryGetValue(statType, out BigNum originalValue))
            {
                SetStatValue(statType, originalValue);
                m_OriginalStatCache.Remove(statType); // 1회성이라면 제거
            }

            if (statType == BuffStatType.Damage)
                m_Status.ResetDamage();
        }

        private BigNum GetStatValue(BuffStatType type)
        {
            return type switch
            {
                BuffStatType.Damage => m_Status.MaxDamage,
                BuffStatType.Health => m_Status.MaxHealth,
                BuffStatType.Armor => m_Status.Armor,
                BuffStatType.Resilient => m_Status.Resilient,
                BuffStatType.CriticalChance => m_Status.CriticalChance,
                BuffStatType.CriticalMultiplier => m_Status.CriticalMultiplier,
                BuffStatType.BossMultiplier => m_Status.BossDamageMultiplier,
                BuffStatType.SkillEffectMultiplier => m_Status.SkillEffectMultiplier,
                _ => throw new ArgumentOutOfRangeException(nameof(type))
            };
        }

        private void SetStatValue(BuffStatType type, BigNum value)
        {
            switch (type)
            {
                case BuffStatType.Damage: 
                    m_Status.MaxDamage = value;
                    m_Status.ResetDamage(); break;
                case BuffStatType.Health: 
                    m_Status.MaxHealth = value;
                    m_Status.ResetHealth(); break;
                case BuffStatType.Armor: 
                    m_Status.MaxArmor = value;
                    m_Status.ResetArmor();  break;
                case BuffStatType.Resilient:
                    m_Status.MaxResilient = value;
                    m_Status.ResetResilient(); break;
                case BuffStatType.CriticalChance:
                    m_Status.CriticalChance = (float)value;
                    break;
                case BuffStatType.CriticalMultiplier:
                    m_Status.CriticalMultiplier = (float)value;
                    break;
                case BuffStatType.BossMultiplier:
                    m_Status.BossDamageMultiplier = (float)value;
                    break;
                case BuffStatType.SkillEffectMultiplier:
                    m_Status.SkillEffectMultiplier = (float)value;
                    break;
                //case BuffStatType.AttackSpeed:
                //case BuffStatType.CooldownResilient:
                default: throw new ArgumentOutOfRangeException(nameof(type));
            }
        }

        // Others
        public void OnDestruction(GameObject attacker)
        {
            StopAll();
        }

        private IEnumerator CoBuff(BuffData target)
        {
            float endTime = Time.time + target.BuffDuration;

            Apply(target);
            while (Time.time > endTime)
            {
                yield return null;
                if (!HasBuff(target))
                    break;
            }
            Revert(target);

            if (HasBuff(target))
            {
                RemoveBuff(target);
            }
        }

    } // Scope by class BuffExecutor
} // namespace SkyDragonHunter.Gameplay