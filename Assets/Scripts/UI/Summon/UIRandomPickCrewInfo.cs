using SkyDragonHunter.Managers;
using SkyDragonHunter.test;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SkyDragonHunter.UI
{
    public enum PickCrewInfoType
    {
        Crew,
        Cannon,
        Repairer,
        Spoils,
    }

    public class UIRandomPickCrewInfo : MonoBehaviour
    {
        // 필드 (Fields)
        [SerializeField] private Image m_Icon;
        [SerializeField] private TextMeshProUGUI m_Count;
        [SerializeField] private Button m_FreePickButton;
        [SerializeField] private Button m_OnePickButton;
        [SerializeField] private Button m_TenPickButton;

        [SerializeField] private Image m_OnePickIcon;
        [SerializeField] private Image m_TenPickIcon;

        // 속성 (Properties)
        public PickCrewInfoType InfoType { get; private set; } = PickCrewInfoType.Crew;

        // 외부 종속성 필드 (External dependencies field)
        // 이벤트 (Events)
        // 유니티 (MonoBehaviour 기본 메서드)
        // Public 메서드
        public void SetType(PickCrewInfoType type)
        {
            InfoType = type;
            switch (InfoType)
            {
                case PickCrewInfoType.Crew:
                    SetCrewMode();
                    break;
                case PickCrewInfoType.Cannon:
                    SetCannonMode();
                    break;
                case PickCrewInfoType.Repairer:
                    SetRepairerMode();
                    break;
                case PickCrewInfoType.Spoils:
                    SetSpoilsMode();
                    break;
            };
        }

        // Private 메서드
        private void SetCrewMode()
        {
            m_Icon.sprite = DataTableMgr.ItemTable.Get(Tables.ItemType.CrewTicket).Icon;
            m_Count.text = AccountMgr.ItemCount(Tables.ItemType.CrewTicket).ToString();
            m_OnePickIcon.sprite = m_Icon.sprite;
            m_TenPickIcon.sprite = m_Icon.sprite;

            var testRandomPick = GameMgr.FindObject<TestRandomPick>("TestRandomPick");
            gameObject.GetComponent<Button>().onClick.RemoveAllListeners();
            m_FreePickButton.onClick.RemoveAllListeners();
            m_FreePickButton.onClick.AddListener(testRandomPick.RandomFreePick);
            m_OnePickButton.onClick.RemoveAllListeners();
            m_OnePickButton.onClick.AddListener(testRandomPick.RandomPick);
            m_TenPickButton.onClick.RemoveAllListeners();
            m_TenPickButton.onClick.AddListener(testRandomPick.RandomTenPick);
        }

        private void SetCannonMode()
        {
            m_Icon.sprite = DataTableMgr.ItemTable.Get(Tables.ItemType.CanonTicket).Icon;
            m_Count.text = AccountMgr.ItemCount(Tables.ItemType.CanonTicket).ToString();
            m_OnePickIcon.sprite = m_Icon.sprite;
            m_TenPickIcon.sprite = m_Icon.sprite;

            var testRandomPick = GameMgr.FindObject<TestRandomPick>("TestRandomPick");
            gameObject.GetComponent<Button>().onClick.RemoveAllListeners();
            m_FreePickButton.onClick.RemoveAllListeners();
            m_FreePickButton.onClick.AddListener(testRandomPick.RandomFreePickCannon);
            m_OnePickButton.onClick.RemoveAllListeners();
            m_OnePickButton.onClick.AddListener(testRandomPick.RandomPickCannon);
            m_TenPickButton.onClick.RemoveAllListeners();
            m_TenPickButton.onClick.AddListener(testRandomPick.RandomTenPickCannon);
        }

        private void SetRepairerMode()
        {
            m_Icon.sprite = DataTableMgr.ItemTable.Get(Tables.ItemType.RepairTicket).Icon;
            m_Count.text = AccountMgr.ItemCount(Tables.ItemType.RepairTicket).ToString();
            m_OnePickIcon.sprite = m_Icon.sprite;
            m_TenPickIcon.sprite = m_Icon.sprite;

            var testRandomPick = GameMgr.FindObject<TestRandomPick>("TestRandomPick");
            gameObject.GetComponent<Button>().onClick.RemoveAllListeners();
            m_FreePickButton.onClick.RemoveAllListeners();
            m_FreePickButton.onClick.AddListener(testRandomPick.RandomFreePickRepairer);
            m_OnePickButton.onClick.RemoveAllListeners();
            m_OnePickButton.onClick.AddListener(testRandomPick.RandomPickRepairer);
            m_TenPickButton.onClick.RemoveAllListeners();
            m_TenPickButton.onClick.AddListener(testRandomPick.RandomTenPickRepairer);
        }

        private void SetSpoilsMode()
        {
            m_Icon.sprite = DataTableMgr.ItemTable.Get(Tables.ItemType.Spoils).Icon;
            m_Count.text = AccountMgr.ItemCount(Tables.ItemType.Spoils).ToString();
            m_OnePickIcon.sprite = m_Icon.sprite;
            m_TenPickIcon.sprite = m_Icon.sprite;

            var testRandomPick = GameMgr.FindObject<TestRandomPick>("TestRandomPick");
            gameObject.GetComponent<Button>().onClick.RemoveAllListeners();
            m_FreePickButton.onClick.RemoveAllListeners();
            m_FreePickButton.onClick.AddListener(testRandomPick.RandomFreePickSpoils);
            m_OnePickButton.onClick.RemoveAllListeners();
            m_OnePickButton.onClick.AddListener(testRandomPick.RandomPickSpoils);
            m_TenPickButton.onClick.RemoveAllListeners();
            m_TenPickButton.onClick.AddListener(testRandomPick.RandomTenPickSpoils);
        }
        // Others

    } // Scope by class UIRandomPickCrewInfo
} // namespace Root
