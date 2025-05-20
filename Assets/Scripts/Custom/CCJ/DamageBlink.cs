using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SkyDragonHunter.Gameplay
{
    public class DamageBlink : MonoBehaviour
    {
        // 필드 (Fields)
        [SerializeField] private float m_BlinkDuration = 0.1f;
        [SerializeField] private int m_BlinkCount = 3;

        private Renderer[] m_Renderers;
        private List<Color[]> m_OriginalColors;

        private Coroutine m_Coroutine = null;

        // 속성 (Properties)
        // 외부 종속성 필드 (External dependencies field)
        // 이벤트 (Events)
        // 유니티 (MonoBehaviour 기본 메서드)
        private void Awake()
        {
            // 자신과 모든 자식의 Renderer를 가져옴 (MeshRenderer, SpriteRenderer 포함)
            m_Renderers = GetComponentsInChildren<Renderer>();
            m_OriginalColors = new List<Color[]>();

            // 머티리얼 색상을 저장
            foreach (var r in m_Renderers)
            {
                var colors = new Color[r.materials.Length];
                for (int i = 0; i < r.materials.Length; i++)
                {
                    colors[i] = r.materials[i].color;
                }
                m_OriginalColors.Add(colors);
            }
        }

        private void OnDisable()
        {
            m_Coroutine = null;
            StopAllCoroutines();
        }

        // Public 메서드
        public void OnHit()
        {
            if (m_Coroutine != null)
                StopCoroutine(m_Coroutine);

            m_Coroutine = StartCoroutine(BlinkCoroutine());
        }

        // Private 메서드
        private IEnumerator BlinkCoroutine()
        {
            for (int i = 0; i < m_BlinkCount; i++)
            {
                SetRenderersRed(true);
                yield return new WaitForSeconds(m_BlinkDuration);
                SetRenderersRed(false);
                yield return new WaitForSeconds(m_BlinkDuration);
            }
            m_Coroutine = null;
        }

        private void SetRenderersRed(bool red)
        {
            for (int i = 0; i < m_Renderers.Length; i++)
            {
                var renderer = m_Renderers[i];
                for (int j = 0; j < renderer.materials.Length; j++)
                {
                    renderer.materials[j].color = red ? Color.red : m_OriginalColors[i][j];
                }
            }
        }

        // Others

    } // Scope by class DamageBlink
} // namespace Root
