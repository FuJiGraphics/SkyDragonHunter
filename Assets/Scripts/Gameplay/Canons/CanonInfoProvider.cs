using SkyDragonHunter.Interfaces;
using SkyDragonHunter.Scriptables;
using UnityEngine;

namespace SkyDragonHunter.Gameplay {
    public class CanonInfoProvider : MonoBehaviour
        , ICanonInfoProvider
    {
        // 필드 (Fields)
        [SerializeField] private Sprite canonIcon;

        [Tooltip("게임 오브젝트에 CanonBase가 바인딩 되어있어야 함")]
        [SerializeField] private CanonBase m_CanonBase;
        private CanonDefinition m_CanonData;

        // 속성 (Properties)
        public string Name => m_CanonData.canName;
        public Sprite Icon => canonIcon;

        // 외부 종속성 필드 (External dependencies field)
        // 이벤트 (Events)
        // 유니티 (MonoBehaviour 기본 메서드)
        private void Awake()
        {
            if (m_CanonBase == null)
            {
                m_CanonBase = GetComponent<CanonBase>();
                m_CanonData = m_CanonBase.CanonData;
            }
        }

        // Public 메서드
        // Private 메서드
        // Others

    } // Scope by class CanonInfoProvider
} // namespace SkyDragonHunter.Gameplay