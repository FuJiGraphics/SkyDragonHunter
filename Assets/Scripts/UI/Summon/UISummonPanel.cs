using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SkyDragonHunter.UI
{
    public class UISummonPanel : MonoBehaviour
    {
        // 필드 (Fields)
        [SerializeField] private Slider[] m_SummonExpSliders;

        private float m_CurrentExp = 0f;

        // 속성 (Properties)
        public float Exp
        {
            get => m_CurrentExp;
            set
            {
                m_CurrentExp = value;
                foreach (var slider in m_SummonExpSliders)
                {
                    slider.value = Exp;
                }
            }
        }

        public void ResetExp()
            => Exp = 0;

        public bool IsDone => m_CurrentExp >= 1f;

        // 외부 종속성 필드 (External dependencies field)
        // 이벤트 (Events)
        // 유니티 (MonoBehaviour 기본 메서드)
        // Public 메서드
        // Private 메서드
        // Others

    } // Scope by class UISummonPanel
} // namespace Root
