using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SkyDragonHunter.UI {

    public class UISpeechBubble : MonoBehaviour
    {
        // 필드 (Fields)
        [SerializeField] private float lifeTime;
        [SerializeField] private TextMeshProUGUI m_Text;

        // 속성 (Properties)
        // 외부 종속성 필드 (External dependencies field)
        // 이벤트 (Events)
        // 유니티 (MonoBehaviour 기본 메서드)

        private void Awake()
        {
            m_Text = GetComponent<TextMeshProUGUI>();
            Destroy(gameObject, lifeTime);
            var position = transform.position;
            position.y = 5f;
            transform.position = position;
            transform.rotation = Camera.main.transform.rotation;
        }

        private void Update()
        {
            transform.rotation = Camera.main.transform.rotation;
        }

        // Public 메서드
        public void SetText(string text)
            => m_Text.text = text;

        // Private 메서드
        // Others
    
    } // Scope by class UISpeechBubble

} // namespace Root