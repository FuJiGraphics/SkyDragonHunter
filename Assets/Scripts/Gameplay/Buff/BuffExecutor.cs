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
        [SerializeField] private Transform m_BuffEffectAnchor;
        [SerializeField] private CharacterStatus m_Status;

        private Dictionary<BuffType, Dictionary<BuffStatType, Coroutine>> m_CommandList; 
        private Dictionary<BuffStatType, BigNum> m_OriginalStatCache = new();
        private List<BuffStatType> m_CachingApplyTypes = new();

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

        private void OnDisable()
        {
            StopAll();
        }

        private void OnDestroy()
        {
            StopAll();
        }

        // Public 메서드
        public void Execute(BuffData target)
        {
            if (m_CommandList == null || !m_CommandList.ContainsKey(target.BuffApply))
                return;

            if (m_CommandList[target.BuffApply].ContainsKey(target.BuffStatType))
                return;

            Coroutine coroutine = StartCoroutine(CoBuff(target));
            if (coroutine != null)
            {
                m_CommandList[target.BuffApply].Add(target.BuffStatType, coroutine);
            }
            else
            {
                Debug.LogError("[BuffExecutor]: 코루틴이 비정상적으로 종료 되었습니다.");
                return;
            }
        }

        public void Stop(BuffData target)
        {
            if (m_CommandList == null || !m_CommandList.ContainsKey(target.BuffApply))
                return;

            if (m_CommandList[target.BuffApply].ContainsKey(target.BuffStatType))
            {
                StopCoroutine(m_CommandList[target.BuffApply][target.BuffStatType]);
                m_CommandList[target.BuffApply].Remove(target.BuffStatType);
            }
        }

        public void StopAll()
        {
            foreach (var buffType in m_CachingApplyTypes)
            {
                Revert(buffType);
            }
            foreach (var command in m_CommandList)
            {
                foreach (var coroutine in command.Value)
                {
                    if (coroutine.Value != null)
                    {
                        StopCoroutine(coroutine.Value);
                    }
                }
                command.Value.Clear();
            }
            m_CachingApplyTypes.Clear();
        }

        public bool HasBuff(BuffData target)
        {
            if (m_CommandList != null && m_CommandList.Count <= 0)
                return false;

            bool result = false;
            if (m_CommandList[target.BuffApply].ContainsKey(target.BuffStatType))
            {
                result = true;
            }
            return result;
        }

        public bool HasBuff(BuffData[] targets)
        {
            if (m_CommandList != null && m_CommandList.Count <= 0)
                return false;

            bool result = false;
            foreach (var target in targets)
            {
                if (m_CommandList[target.BuffApply].ContainsKey(target.BuffStatType))
                {
                    result = true;
                }
            }
            return result;
        }

        // Private 메서드
        private void RemoveBuff(BuffData target)
        {
            if (m_CommandList != null && m_CommandList.Count <= 0)
                return;

            m_CommandList[target.BuffApply].Remove(target.BuffStatType);
        }

        private void Apply(BuffData target)
        {
            var statType = target.BuffStatType;
            if (!m_OriginalStatCache.ContainsKey(statType))
            {
                if (target.BuffStatType == BuffStatType.Shield)
                {
                    m_OriginalStatCache[statType] = 0;
                }
                else
                {
                    m_OriginalStatCache[statType] = GetStatValue(statType);
                }
            }

            BigNum modified;
            if (target.BuffStatType == BuffStatType.Shield)
            {
                modified = m_OriginalStatCache[BuffStatType.Health] * target.EffectiveMultiplier;
            }
            else
            {
                modified = m_OriginalStatCache[statType] * target.EffectiveMultiplier;
            }
            SetStatValue(statType, modified);
        }

        private void Revert(BuffStatType type)
        {
            var statType = type;
            if (m_OriginalStatCache.TryGetValue(statType, out BigNum originalValue))
            {
                SetStatValue(statType, originalValue);
                m_OriginalStatCache.Remove(statType); // 1회성이라면 제거
            }
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
                BuffStatType.Shield => m_Status.Shield,
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
                    double prevHealthWeight = (double)m_Status.Health / (double)m_Status.MaxHealth;
                    m_Status.MaxHealth = value;
                    m_Status.Health = value;
                    m_Status.Health *= prevHealthWeight;
                    break;
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
                case BuffStatType.Shield:
                    m_Status.MaxShield = value;
                    m_Status.ResetShield();
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
            m_CachingApplyTypes.Add(target.BuffStatType);
            while (Time.time <= endTime)
            {
                yield return null;
                if (!HasBuff(target))
                    break;
            }
            Revert(target.BuffStatType);
            RemoveBuff(target);
        }

    } // Scope by class BuffExecutor
} // namespace SkyDragonHunter.Gameplay