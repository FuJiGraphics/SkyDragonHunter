using DamageNumbersPro;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace SkyDragonHunter {
    public class UIDamageMeter : MonoBehaviour
    {
        // 필드 (Fields)
        [SerializeField] private DamageNumberMesh m_TextComponent;

        // 속성 (Properties)
        // 외부 종속성 필드 (External dependencies field)
        // 이벤트 (Events)
        // 유니티 (MonoBehaviour 기본 메서드)
        private void Awake()
        {
            m_TextComponent = GetComponentInChildren<DamageNumberMesh>();
        }

        public void SetTopText(string text, Color color)
        {
            m_TextComponent.SetColor(color);
            m_TextComponent.topText = text;
        }

        public void SetText(string text, Color color)
        {
            m_TextComponent.SetColor(color);
            m_TextComponent.leftText = text;
        }

        // Public 메서드
        // Private 메서드
        // Others

    } // Scope by class UIDamageMeter
} // namespace SkyDragonHunter