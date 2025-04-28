using SkyDragonHunter.Gameplay;
using UnityEngine;
using UnityEngine.UI;

namespace SkyDragonHunter.UI {

    public class UIRepairSlot : MonoBehaviour
    {
        // 필드 (Fields)
        [SerializeField] private Image m_RepairIcon;
        [SerializeField] private Sprite m_RepairSlotIcon;
        [SerializeField] private Sprite m_RepairDownIcon;
        [SerializeField] private Image m_RepairLockIcon;

        // 속성 (Properties)
        public Image RepairIcon => m_RepairIcon;
        public RepairDummy RepairDummy { get; set; }

        // 외부 종속성 필드 (External dependencies field)
        // 이벤트 (Events)
        // 유니티 (MonoBehaviour 기본 메서드)
        private void OnEnable()
        {
            UIUpdateUnlockState();
        }

        private void Update()
        {
            UIUpdateUnlockState();
        }

        // Public 메서드
        public void ShowDownIconImage()
        {
            GetComponent<Image>().sprite = m_RepairDownIcon;
        }

        public void ShowIdleIconImage()
        {
            GetComponent<Image>().sprite = m_RepairSlotIcon;
        }

        public void UIUpdateUnlockState()
        {
            if (RepairDummy != null && RepairDummy.IsUnlock)
            {
                m_RepairLockIcon.gameObject.SetActive(false);
                RepairIcon.color = Color.white;
                GetComponent<Image>().color = Color.white;
            }
            else
            {
                m_RepairLockIcon.gameObject.SetActive(true);
                RepairIcon.color = Color.gray;
                GetComponent<Image>().color = Color.gray;
            }
        }
        // Private 메서드
        // Others

    } // Scope by class UIRepairSlot
} // namespace SkyDragonHunter