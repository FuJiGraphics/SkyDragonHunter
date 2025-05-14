using SkyDragonHunter.Managers;
using SkyDragonHunter.Structs;
using SkyDragonHunter.Tables;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SkyDragonHunter.UI {

    public struct RewardItem
    {
        public ItemType type;
        public BigNum count;

        public RewardItem(ItemType type, BigNum count)
        {
            this.type = type;
            this.count = count;
        }

        public ItemData GetItem()
            => DataTableMgr.ItemTable.Get(type);
    }

    [System.Serializable]
    public struct UIRewardSlot
    {
        public Image rewardBackgroundImage;
        public Image rewardIcon;
        public TextMeshProUGUI rewardText;

        public void SetDisable() => rewardBackgroundImage.gameObject.SetActive(false);
        public void SetEnable() => rewardBackgroundImage.gameObject.SetActive(true);
    }

    public class UIClearPanel : MonoBehaviour
    {
        // 필드 (Fields)
        [SerializeField] private TextMeshProUGUI m_StageNumberText;
        [SerializeField] private TextMeshProUGUI m_StageNameText;
        [SerializeField] private UIRewardSlot[] m_RewardSlots;

        // 속성 (Properties)
        // 외부 종속성 필드 (External dependencies field)
        // 이벤트 (Events)
        // 유니티 (MonoBehaviour 기본 메서드)
        // Public 메서드
        public void ShowResult(string stageNumber, string stageName, RewardItem[] rewards)
        {
            DisableRewardSlotIcons();

            int rewardCount = Mathf.Min(m_RewardSlots.Length, rewards.Length);
            m_StageNumberText.text = stageNumber;
            m_StageNameText.text = stageName;
            for (int i = 0; i < rewardCount; ++i)
            {
                m_RewardSlots[i].SetEnable();
                m_RewardSlots[i].rewardIcon.sprite = rewards[i].GetItem().Icon;
                m_RewardSlots[i].rewardText.text = rewards[i].count.ToUnit();
            }
        }

        // Private 메서드
        private void DisableRewardSlotIcons()
        {
            for (int i = 0; i < m_RewardSlots.Length; ++i)
            {
                m_RewardSlots[i].SetDisable();
            }
        }
        // Others

    } // Scope by class UIClearPanel
} // namespace Root
