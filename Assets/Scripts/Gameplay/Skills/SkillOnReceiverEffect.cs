using SkyDragonHunter.Interfaces;
using SkyDragonHunter.Managers;
using SkyDragonHunter.Scriptables;
using System.Collections.Generic;
using UnityEngine;

namespace SkyDragonHunter.Gameplay {

    public class SkillOnReceiverEffect : MonoBehaviour
        , ISkillEffectLifecycleHandler
    {
        // 필드 (Fields)
        [SerializeField] private string m_TargetEffectName;
        [SerializeField] private float m_EffectDuration = 1f;
        [SerializeField] private Vector2 m_Position = Vector2.zero;
        [SerializeField] private Vector2 m_Scale = Vector2.one;

        // 속성 (Properties)
        // 외부 종속성 필드 (External dependencies field)
        // 이벤트 (Events)
        // 유니티 (MonoBehaviour 기본 메서드)
        // Public 메서드
        public void OnCastEffect(GameObject caster)
        {
        }

        public void OnHitEnterEffect(GameObject caster, GameObject receiver) 
        {
            EnterEffectToReceiver(receiver);
        }
        public void OnHitStayEffect(GameObject caster, GameObject receiver) { }

        // Private 메서드
        private void EnterEffectToReceiver(GameObject receiver)
        {
            if (!string.IsNullOrEmpty(m_TargetEffectName))
            {
                var effectInstance = EffectMgr.Play(m_TargetEffectName, receiver, m_Position, m_Scale, m_EffectDuration);
            }
        }

        // Others

    } // Scope by class SkillOnCastEffect
} // namespace SkyDragonHunter