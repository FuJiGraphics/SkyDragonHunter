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
        // 필드 (Fields)
        [SerializeField] private string m_CrewTitle;
        [SerializeField] private string m_CrewName;
        [SerializeField] private Sprite m_CrewIcon;
        [SerializeField] private Sprite m_CrewPreview;

        private int m_QuantityCount = 0;
        private bool m_EquipState = false;

        // 속성 (Properties)
        // 외부 종속성 필드 (External dependencies field)
        private CharacterStatus m_Status;
        private NewCrewControllerBT m_Controller;

        // 이벤트 (Events)
        // 유니티 (MonoBehaviour 기본 메서드)
        public void Awake()
        {
            m_Status = GetComponent<CharacterStatus>();
            m_Controller = GetComponent<NewCrewControllerBT>();
        }

        // Public 메서드
        public void IncreaseQuantityCount()
            => m_QuantityCount++;

        public void SetEquipState(bool isEquip)
            => m_EquipState = isEquip;

        public int ID => m_Controller.ID;
        public string Title => gameObject.name;
        public string Level => "0";
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

        // Private 메서드
        // Others

    } // Scope by class CrewDestructable

} // namespace Root