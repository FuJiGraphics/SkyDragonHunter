using SkyDragonHunter.Entities;
using SkyDragonHunter.Interfaces;
using SkyDragonHunter.Managers;
using SkyDragonHunter.UI;
using UnityEngine;

namespace SkyDragonHunter.Gameplay {

    public class CrewInfoProvider : MonoBehaviour
        , ICrewInfoProvider
    {
        // 필드 (Fields)
        [SerializeField] private string CrewName;
        [SerializeField] private Sprite CrewIcon;
        [SerializeField] private Sprite CrewPreview;

        // 속성 (Properties)
        // 외부 종속성 필드 (External dependencies field)
        private CharacterStatus m_Status;
        private CrewControllerBT m_Controller;

        // 이벤트 (Events)
        // 유니티 (MonoBehaviour 기본 메서드)
        public void Awake()
        {
            m_Status = GetComponent<CharacterStatus>();
            m_Controller = GetComponent<CrewControllerBT>();
        }

        public void Start()
        {
            // Crew 정보창에 자신을 등록
            GameObject findPanelGo = GameMgr.FindObject("UICrewInfoPanel");
            if (findPanelGo.TryGetComponent<UICrewInfoPanel>(out var crewInfoPanel))
            {
                crewInfoPanel.AddCrewNode(gameObject);
            }
            else
            {
                Debug.LogWarning("[CrewInfoProvider]: Crew Info Panel Node 등록 실패");
            }
        }

        // Public 메서드
        public string Name => CrewName;
        public string Damage => m_Status.Damage.ToString();
        public string Health => m_Status.Health.ToString();
        public string Defense => m_Status.Armor.ToString();
        public Sprite Preview => CrewPreview;
        public Sprite Icon => CrewIcon;
        public bool IsMounted => m_Controller.isMounted;

        // Private 메서드
        // Others

    } // Scope by class CrewDestructable

} // namespace Root