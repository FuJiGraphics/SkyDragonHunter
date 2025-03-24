using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace SkyDragonHunter {
    public class UIDamageMeter : MonoBehaviour
    {
        // 필드 (Fields)
        [SerializeField] private string m_Text;
        [SerializeField] private TextMeshProUGUI m_TextComponent;
        [SerializeField] private Vector3 m_Offset;

        private Vector3 m_LastPosition;
        private bool m_IsFirstUpdate = false;

        // 속성 (Properties)
        // 외부 종속성 필드 (External dependencies field)
        // 이벤트 (Events)
        // 유니티 (MonoBehaviour 기본 메서드)
        private void Awake()
        {
            m_TextComponent = GetComponentInChildren<TextMeshProUGUI>();
            m_TextComponent.text = m_Text;
        }

        private void Update()
        {
            if (!m_IsFirstUpdate)
            {
                m_IsFirstUpdate = true;
                m_LastPosition = transform.position;
            }
            transform.position = m_LastPosition + m_Offset;
        }

        public void SetText(string text)
            => m_TextComponent.text = text;

        public void SetColor(Color color)
            => m_TextComponent.color = color;

        // Public 메서드
        public void OnAnimationEnd()
        {
            GameObject.Destroy(gameObject);
        }

        // Private 메서드
        // Others

    } // Scope by class UIDamageMeter
} // namespace SkyDragonHunter