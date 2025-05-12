
using SkyDragonHunter.Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SkyDragonHunter
{
    public class AttackTargetSelector : MonoBehaviour
        , IAttackTargetProvider
    {
        // 필드 (Fields)
        [Tooltip("공격 대상에 대한 태그를 스킬이 설정할 지 여부")]
        [SerializeField] private bool m_IsSelfTargeting = false;
        [SerializeField] private string[] allowedTargetTags;

        private Dictionary<string, bool> m_AllowedTargetMaps;

        // 속성 (Properties)
        public string[] AllowedTargetTags => allowedTargetTags;
        public bool IsAllowedTarget(string tag) => m_AllowedTargetMaps.ContainsKey(tag);
        public bool IsSelfTargeting => m_IsSelfTargeting;

        // 외부 종속성 필드 (External dependencies field)
        // 이벤트 (Events)
        // 유니티 (MonoBehaviour 기본 메서드)
        private void Awake()
        {
            Init();
        }

        // Public 메서드
        public void SetAllowedTarget(string[] targets)
        {
            allowedTargetTags = targets;
            Init();
        }

        // Private 메서드
        private void Init()
        {
            m_AllowedTargetMaps = new Dictionary<string, bool>();
            foreach (var target in allowedTargetTags)
            {
                m_AllowedTargetMaps.Add(target, true);
            }
        }

        // Others

    } // Scope by class AttackTargetSelector
} // namespace Root
