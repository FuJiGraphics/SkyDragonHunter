
using UnityEngine;
using UnityEngine.EventSystems;

namespace SkyDragonHunter.UI {

    public class UIClickEffect : MonoBehaviour
    {
        // 필드 (Fields)
        public GameObject m_EffectPrefab;

        // 속성 (Properties)
        // 외부 종속성 필드 (External dependencies field)
        // 이벤트 (Events)
        // 유니티 (MonoBehaviour 기본 메서드)
        private void Update()
        {
            if (Input.touchCount > 0 && Time.timeScale > 0f)
            {
                Touch touch = Input.GetTouch(0);
                if (touch.phase == TouchPhase.Began)
                {
                    Touch(ref touch);
                }
            }
        }

        // Public 메서드
        public void Touch(ref Touch touch)
        {
            Vector3 pos = Camera.main.ScreenToWorldPoint(touch.position);
            pos.z = 1f;
            GameObject effect = Instantiate(m_EffectPrefab, pos, Quaternion.identity);
            Destroy(effect, 1f);
        }

        // Private 메서드
        // Others

    } // Scope by class UIClickEffect
} // namespace SkyDragonHunter