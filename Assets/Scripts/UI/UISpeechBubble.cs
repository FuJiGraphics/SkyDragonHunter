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

        [SerializeField] private Canvas m_Canvas;
        [SerializeField] private Image m_Image;
        [SerializeField] private TextMeshProUGUI m_Text;

        private Transform m_OwnerTransform;
        private Vector3 offset;

        // 속성 (Properties)
        // 외부 종속성 필드 (External dependencies field)
        // 이벤트 (Events)
        // 유니티 (MonoBehaviour 기본 메서드)
        private void Start()
        {
            Destroy(gameObject, lifeTime);
        }

        private void Update()
        {
            var newPos = m_OwnerTransform.position;
            newPos += offset;
            transform.position = newPos;
        }

        // Public 메서드
        public void SetText(string text, Transform owner)
        {
            m_Text.text = text;
            m_OwnerTransform = owner;
            var meshRenderer = m_OwnerTransform.GetComponent<MeshRenderer>();
            var yOffset = 0.5f;
            if (meshRenderer != null)
            {
                yOffset = meshRenderer.bounds.size.y * 0.5f + 0.5f;
            }
            offset = new Vector3(0, yOffset, 0);
        }

        // Private 메서드
        // Others
    
    } // Scope by class UISpeechBubble

} // namespace Root