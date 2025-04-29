using SkyDragonHunter.Interfaces;
using UnityEngine;

namespace SkyDragonHunter.Gameplay {

    public class CanonInfoProvider : MonoBehaviour
        , ICanonInfoProvider
    {
        // 필드 (Fields)
        [SerializeField] private Sprite m_CanonIcon;
        [SerializeField] private UnityEngine.Color m_IconColor = Color.white;

        [Tooltip("게임 오브젝트에 CanonBase가 바인딩 되어있어야 함")]
        [SerializeField] private CanonBase m_CanonBase;

        // 속성 (Properties)
        public string Name => m_CanonBase.CanonData.canName;
        public Sprite Icon => m_CanonIcon;
        public Color Color => m_IconColor;

        // 외부 종속성 필드 (External dependencies field)
        // 이벤트 (Events)
        // 유니티 (MonoBehaviour 기본 메서드)
        private void Awake()
        {
            if (m_CanonBase == null)
            {
                m_CanonBase = GetComponent<CanonBase>();
            }
        }

        // Public 메서드
        // Private 메서드
        // Others

    } // Scope by class CanonInfoProvider
} // namespace SkyDragonHunter.Gameplay