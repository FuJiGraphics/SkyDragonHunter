
using UnityEngine;
using UnityEngine.EventSystems;

namespace SkyDragonHunter.UI {

    public class UIClickEffect : MonoBehaviour
        , IPointerClickHandler
    {
        // 필드 (Fields)
        public GameObject m_EffectPrefab;
        public Transform m_EffectParent;

        // 속성 (Properties)
        // 외부 종속성 필드 (External dependencies field)
        // 이벤트 (Events)
        // 유니티 (MonoBehaviour 기본 메서드)
        private void Awake()
        {
            if (m_EffectParent == null)
            {
                m_EffectParent = transform;
            }
        }

        // Public 메서드
        public void OnPointerClick(PointerEventData eventData)
        {
            GameObject effect = Instantiate(m_EffectPrefab, transform.position, Quaternion.identity, m_EffectParent);
            Destroy(effect, 0.5f);
        }

        // Private 메서드
        // Others

    } // Scope by class UIClickEffect
} // namespace SkyDragonHunter