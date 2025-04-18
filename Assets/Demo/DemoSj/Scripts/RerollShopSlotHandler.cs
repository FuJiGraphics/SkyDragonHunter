using SkyDragonHunter.test;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace SkyDragonHunter
{
    public class RerollShopSlotHandler : MonoBehaviour
    {
        [Header("UI 연결")]
        [SerializeField] private FavorabilityUIController favorabilityUIController;
        [SerializeField] private FavorailityMgr favorabilityMgr;
        [SerializeField] private TextMeshProUGUI itemNameText;
        [SerializeField] private Image itemImage;
        [SerializeField] private Image currencyImage;
        [SerializeField] private CurrencyType currencyType;
        [SerializeField] private TextMeshProUGUI priceText;
        [SerializeField] private Button buyButton;

        [Header("잠금 기능")]
        [SerializeField] private Button lockButton;
        [SerializeField] private Image lockImage;
        [SerializeField] private Sprite lockedSprite;
        [SerializeField] private Sprite unlockedSprite;

        private bool isLocked = false;
        private bool isPurchased = false;
        private ItemStatus itemData;
        private int currentPrice;

        // Public 메서드

        public void UpdateDiscountedPrice(float discountRate)
        {
            currentPrice = Mathf.FloorToInt(itemData.price * (1f - discountRate));
            priceText.text = currentPrice.ToString("N0");
        }

        public void Initialize(ItemStatus item, FavorailityMgr favorMgr, FavorabilityUIController favorUIController)
        {
            favorabilityMgr = favorMgr;
            favorabilityUIController = favorUIController;
            itemData = item;
            isLocked = false;
            isPurchased = false;

            float discountRate = favorabilityMgr.GetDiscountRate();
            currentPrice = Mathf.FloorToInt(item.price * (1f - discountRate));

            itemNameText.text = item.itemName;
            itemImage.sprite = item.itemImage;
            currencyImage.sprite = item.currencyTypeIcon;
            currencyType = item.currencyType;
            priceText.text = currentPrice.ToString();

            lockImage.sprite = unlockedSprite;
            lockImage.gameObject.SetActive(true);

            lockButton.interactable = true;
            buyButton.interactable = true;

            lockButton.onClick.RemoveAllListeners();
            lockButton.onClick.AddListener(LockOnce);

            buyButton.onClick.RemoveAllListeners();
            buyButton.onClick.AddListener(TryBuy);
        }

        public void TryBuy()
        {
            if (itemData.currencyType == CurrencyType.Gold)
            {
                if (favorabilityMgr.testGold < itemData.price) return;
                favorabilityMgr.testGold -= itemData.price;
            }
            else if (itemData.currencyType == CurrencyType.Diamond)
            {
                if (favorabilityMgr.testDiamond < itemData.price) return;
                favorabilityMgr.testDiamond -= itemData.price;
            }

            if (isPurchased) return;

            isPurchased = true;
            isLocked = false;

            lockImage.sprite = unlockedSprite;
            buyButton.interactable = false;
            lockButton.interactable = false;

            favorabilityMgr.GainExpFromRerollPurchase();
            favorabilityUIController.UpdateUI();
        }


        // 한 번만 잠금 가능
        private void LockOnce()
        {
            if (isPurchased || isLocked) return;

            isLocked = true;
            lockImage.sprite = lockedSprite;

            // 이후 클릭 방지 (시각적만 아니라 기능적으로도)
            lockButton.interactable = false;
        }

        public bool IsLocked() => isLocked;
        public bool IsPurchased() => isPurchased;
        public ItemStatus GetItem() => itemData;

    } // Scope by class RerollShopSlotHandler

} // namespace Root
