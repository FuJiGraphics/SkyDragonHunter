using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SkyDragonHunter.UI {

    public class UIClickOnDestroy : MonoBehaviour
    {
        // 필드 (Fields)
        [SerializeField] private Button[] m_Buttons;

        [SerializeField] private bool m_IsAnimEnd = false;

        // 속성 (Properties)
        public bool IsAnimEnd => m_IsAnimEnd;

        // 외부 종속성 필드 (External dependencies field)
        // 이벤트 (Events)
        // 유니티 (MonoBehaviour 기본 메서드)
        private void Start()
        {
            if (m_Buttons != null)
            {
                foreach (var button in m_Buttons)
                {
                    button.onClick.AddListener(() => 
                    { 
                        if (IsAnimEnd) 
                        { 
                            Destroy(gameObject); 
                        } 
                    });
                }
            }
        }

        // Public 메서드
        public void OnAnimationEnd()
            => m_IsAnimEnd = true;

        // Private 메서드
        // Others

    } // Scope by class UIClickOnDestroy
} // namespace SkyDragonHunter