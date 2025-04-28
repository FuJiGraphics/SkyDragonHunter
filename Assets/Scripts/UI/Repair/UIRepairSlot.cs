using SkyDragonHunter.Gameplay;
using UnityEngine;
using UnityEngine.UI;

namespace SkyDragonHunter.UI {

    public class UIRepairSlot : MonoBehaviour
    {
        // �ʵ� (Fields)
        [SerializeField] private Image m_RepairIcon;
        [SerializeField] private Sprite m_RepairSlotIcon;
        [SerializeField] private Sprite m_RepairDownIcon;
        [SerializeField] private Image m_RepairLockIcon;

        // �Ӽ� (Properties)
        public Image RepairIcon => m_RepairIcon;
        public RepairDummy RepairDummy { get; set; }

        // �ܺ� ���Ӽ� �ʵ� (External dependencies field)
        // �̺�Ʈ (Events)
        // ����Ƽ (MonoBehaviour �⺻ �޼���)
        private void OnEnable()
        {
            UIUpdateUnlockState();
        }

        private void Update()
        {
            UIUpdateUnlockState();
        }

        // Public �޼���
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
        // Private �޼���
        // Others

    } // Scope by class UIRepairSlot
} // namespace SkyDragonHunter