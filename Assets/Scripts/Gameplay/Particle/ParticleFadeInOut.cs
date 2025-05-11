using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SkyDragonHunter.Gameplay {

    public class ParticleFadeInOut : MonoBehaviour
    {
        // 필드 (Fields)
        [SerializeField] private ParticleSystem m_Particle;
        [SerializeField] private SpriteRenderer m_Target;
        [SerializeField] private float m_FadeDuration = 0.5f;
        [Tooltip("어두워지는 정도 조절용")]
        [SerializeField, Range(0f, 1f)] private float m_MaxAlphaOnFadeOut = 0f;

        // 속성 (Properties)
        // 외부 종속성 필드 (External dependencies field)
        // 이벤트 (Events)
        // 유니티 (MonoBehaviour 기본 메서드)
        private void Start()
        {
            StartCoroutine(HandleFadeSequence());
        }

        // Public 메서드
        // Private 메서드
        private IEnumerator HandleFadeSequence()
        {
            yield return StartCoroutine(FadeInSpriteRenderer());

            float particleDuration = m_Particle.main.duration;
            float waitTime = particleDuration - m_FadeDuration;
            yield return new WaitForSeconds(waitTime);

            yield return StartCoroutine(FadeOutSpriteRenderer());
        }

        private IEnumerator FadeOutSpriteRenderer()
        {
            float elapsed = 0f;
            Color color = m_Target.color;
            color.a = m_MaxAlphaOnFadeOut;
            m_Target.color = color;

            while (elapsed < m_FadeDuration)
            {
                elapsed += Time.deltaTime;
                float t = elapsed / m_FadeDuration;
                color.a = Mathf.Lerp(m_MaxAlphaOnFadeOut, 0f, t);
                m_Target.color = color;
                yield return null;
            }

            color.a = 0f;
            m_Target.color = color;
        }

        private IEnumerator FadeInSpriteRenderer()
        {
            float elapsed = 0f;
            Color color = m_Target.color;
            color.a = 0f;
            m_Target.color = color;

            while (elapsed < m_FadeDuration)
            {
                elapsed += Time.deltaTime;
                float t = elapsed / m_FadeDuration;
                color.a = Mathf.Lerp(0f, m_MaxAlphaOnFadeOut, t);
                m_Target.color = color;
                yield return null;
            }

            color.a = m_MaxAlphaOnFadeOut;
            m_Target.color = color;
        }

        // Others

    } // Scope by class ParticleFadeInOut
} // namespace SkyDragonHunter