using SkyDragonHunter.Entities;
using SkyDragonHunter.Interfaces;
using SkyDragonHunter.Managers;
using SkyDragonHunter.UI;
using UnityEngine;

namespace SkyDragonHunter.Gameplay {

    public class CrewInfoProvider : MonoBehaviour
        , ICrewInfoProvider
    {
        // �ʵ� (Fields)
        [SerializeField] private string CrewName;
        [SerializeField] private Sprite CrewIcon;
        [SerializeField] private Sprite CrewPreview;

        // �Ӽ� (Properties)
        // �ܺ� ���Ӽ� �ʵ� (External dependencies field)
        private CharacterStatus m_Status;
        private CrewControllerBT m_Controller;

        // �̺�Ʈ (Events)
        // ����Ƽ (MonoBehaviour �⺻ �޼���)
        public void Awake()
        {
            m_Status = GetComponent<CharacterStatus>();
            m_Controller = GetComponent<CrewControllerBT>();
        }

        public void Start()
        {
            // Crew ����â�� �ڽ��� ���
            GameObject findPanelGo = GameMgr.FindObject("UICrewInfoPanel");
            if (findPanelGo.TryGetComponent<UICrewInfoPanel>(out var crewInfoPanel))
            {
                crewInfoPanel.AddCrewNode(gameObject);
            }
            else
            {
                Debug.LogWarning("[CrewInfoProvider]: Crew Info Panel Node ��� ����");
            }
        }

        // Public �޼���
        public string Name => CrewName;
        public string Damage => m_Status.Damage.ToString();
        public string Health => m_Status.Health.ToString();
        public string Defense => m_Status.Armor.ToString();
        public Sprite Preview => CrewPreview;
        public Sprite Icon => CrewIcon;
        public bool IsMounted => m_Controller.isMounted;

        // Private �޼���
        // Others

    } // Scope by class CrewDestructable

} // namespace Root