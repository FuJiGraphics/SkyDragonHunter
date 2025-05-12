using SkyDragonHunter.Interfaces;
using SkyDragonHunter.Managers;
using SkyDragonHunter.Scriptables;
using System.Collections.Generic;
using UnityEngine;

namespace SkyDragonHunter.Gameplay {

    public class SkillOnHitEnterEffect : MonoBehaviour
        , ISkillEffectLifecycleHandler
    {
        // 필드 (Fields)
        [SerializeField] private string m_EffectName;
        [SerializeField] private float m_EffectDuration = 1f;
        [SerializeField] private Vector2 m_Position = Vector2.zero;
        [SerializeField] private Vector2 m_Scale = Vector2.one;

        private SkillBase m_SkillBase;

        // 속성 (Properties)
        // 외부 종속성 필드 (External dependencies field)
        // 이벤트 (Events)
        // 유니티 (MonoBehaviour 기본 메서드)
        private void Awake()
        {
            m_SkillBase = GetComponent<SkillBase>();
        }

        // Public 메서드
        public void OnCastEffect(GameObject caster)
        {
        }

        public void OnHitEnterEffect(GameObject caster, GameObject receiver)
        {
            EnterHitEffectToLocalPosition();
        }

        public void OnHitStayEffect(GameObject caster, GameObject receiver) { }

        // Private 메서드
        private void EnterHitEffectToLocalPosition()
        {
            string effectName = "";
            if (!string.IsNullOrEmpty(m_EffectName))
                effectName = m_EffectName;
            else
                effectName = m_SkillBase.SkillData.skillEffect;

            if (!string.IsNullOrEmpty(effectName))
            {
                Vector2 newPos = transform.position;
                newPos += m_Position;
                var effectInstance = EffectMgr.Play(effectName, newPos, m_Scale, m_EffectDuration);
            }
        }

        // Others

    } // Scope by class SkillOnCastEffect
} // namespace SkyDragonHunter