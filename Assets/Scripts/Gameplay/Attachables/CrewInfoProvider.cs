using SkyDragonHunter.Entities;
using SkyDragonHunter.Interfaces;
using SkyDragonHunter.Managers;
using SkyDragonHunter.Scriptables;
using SkyDragonHunter.UI;
using UnityEngine;

namespace SkyDragonHunter.Gameplay {

    public class CrewInfoProvider : MonoBehaviour
        , ICrewInfoProvider
    {
        // �ʵ� (Fields)
        [SerializeField] private string m_CrewTitle;
        [SerializeField] private string m_CrewName;
        [SerializeField] private Sprite m_CrewIcon;
        [SerializeField] private Sprite m_CrewPreview;

        private int m_QuantityCount = 0;
        private bool m_EquipState = false;

        // �Ӽ� (Properties)
        // �ܺ� ���Ӽ� �ʵ� (External dependencies field)
        private CharacterStatus m_Status;
        private NewCrewControllerBT m_Controller;

        // �̺�Ʈ (Events)
        // ����Ƽ (MonoBehaviour �⺻ �޼���)
        public void Awake()
        {
            m_Status = GetComponent<CharacterStatus>();
            m_Controller = GetComponent<NewCrewControllerBT>();
        }

        // Public �޼���
        public void IncreaseQuantityCount()
            => m_QuantityCount++;

        public void SetEquipState(bool isEquip)
            => m_EquipState = isEquip;

        public string Title => gameObject.name;
        public string Name => "Lv: " + SaveLoadMgr.GameData.savedCrewData.GetCrewLevel(m_Controller.ID).ToString();
        public string Damage => m_Status.Damage.ToUnit();
        public string Health => m_Status.Health.ToUnit();
        public string Defense => m_Status.Armor.ToUnit();
        public Sprite Preview => m_CrewPreview;
        public Sprite Icon => m_CrewIcon;
        public bool IsMounted => m_Controller.IsMounted;
        public int QuantityCount => m_QuantityCount;
        public bool IsEquip => m_EquipState;
        public SkillDefinition Skill => m_Controller.skillExecutor != null 
            ? m_Controller.skillExecutor.SkillData : null;

        // Private �޼���
        // Others

    } // Scope by class CrewDestructable

} // namespace Root