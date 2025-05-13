using SkyDragonHunter.Managers;
using SkyDragonHunter.Structs;
using SkyDragonHunter.Tables;
using SkyDragonHunter.test;
using SkyDragonHunter.UI;
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

        [Header("공용 잠금 확인 패널")] // (수정됨) 공용 패널 참조 필드 추가
        [SerializeField] private RerollShopLockConfirmPanel lockConfirmPanel; // (수정됨)

        private RerollShopController shopController; // (추가)
        private bool isLocked = false;
        private bool isPurchased = false;
        private ItemSlotData itemData;
        private BigNum currentPrice;

        // Public 메서드
        // 외부에서 할인율 변경 시 가격 반영
        public void UpdateDiscountedPrice(float discountRate)
        {
            currentPrice = itemData.Price * (1f - discountRate);
            priceText.text = currentPrice.ToUnit();
        }

        public void InitializeWithState(RerollShopSlotState state, RerollShopController controller,
            FavorailityMgr favorMgr, FavorabilityUIController favorUIController, RerollShopLockConfirmPanel confirmPanel)
        {
            Initialize(state.item, controller, favorMgr, favorUIController, confirmPanel, state.isLocked, state.isPurchased);
        }


        public RerollShopSlotState GetSlotState()
        {
            return new RerollShopSlotState(itemData, isLocked, isPurchased);
        }

        // 슬롯 초기화
        public void Initialize(ItemSlotData item, RerollShopController controller, FavorailityMgr favorMgr,
            FavorabilityUIController favorUIController, RerollShopLockConfirmPanel confirmPanel,
            bool isLocked = false, bool isPurchased = false) // (수정됨)
        {
            shopController = controller;
            favorabilityMgr = favorMgr;
            favorabilityUIController = favorUIController;
            itemData = item;
            lockConfirmPanel = confirmPanel;

            this.isLocked = isLocked;
            this.isPurchased = isPurchased;

            float discountRate = favorabilityMgr.GetDiscountRate();
            currentPrice = item.Price * (1f - discountRate);

            var tableData = item.GetData();
            itemNameText.text = tableData.Name;
            itemImage.sprite = tableData.Icon;
            currencyImage.sprite = item.CurrencyIcon;
            currencyType = item.currencyType;
            priceText.text = currentPrice.ToString();

            lockImage.sprite = isLocked ? lockedSprite : unlockedSprite;
            lockImage.gameObject.SetActive(true);

            lockConfirmPanel.gameObject.SetActive(false);

            lockButton.interactable = true;
            buyButton.interactable = true;

            lockButton.onClick.RemoveAllListeners();
            lockButton.onClick.AddListener(OnClickLockButton); // (수정됨) 확인창 호출로 변경

            buyButton.onClick.RemoveAllListeners();
            buyButton.onClick.AddListener(TryBuy);
        }

        // 구매 처리
        public void TryBuy()
        {
            if (itemData.currencyType == CurrencyType.Coin)
            {
                if (AccountMgr.Coin < itemData.Price)
                {
                    var diff = currentPrice - AccountMgr.Coin;
                    DrawableMgr.Dialog("알림", $"골드가 부족합니다. = {new BigNum(diff).ToUnit()}");
                    return;
                }
                AccountMgr.Coin -= itemData.Price;
            }
            else if (itemData.currencyType == CurrencyType.Diamond)
            {
                if (AccountMgr.Diamond < itemData.Price)
                {
                    var diff = currentPrice - AccountMgr.Diamond;
                    DrawableMgr.Dialog("알림", $"다이아 부족합니다. = {new BigNum(diff).ToString()}");
                    return;
                }
                AccountMgr.Diamond -= itemData.Price;
            }

            if (isPurchased) return;

            isPurchased = true;
            isLocked = false;

            lockImage.sprite = unlockedSprite;
            buyButton.interactable = false;
            lockButton.interactable = false;

            favorabilityMgr.GainExpFromRerollPurchase();
            favorabilityUIController.UpdateUI();
            shopController?.SaveAllSlotStates(); // (추가)
        }

        // (수정됨) 확인 패널에서 호출되어 실제 잠금 처리 수행
        public void ConfirmLockChange(bool unlock)
        {
            isLocked = !unlock;
            lockImage.sprite = isLocked ? lockedSprite : unlockedSprite;
            lockButton.interactable = !isLocked;
        }

        // 잠금/해제 동작 (확인 시 실행됨) (수정됨)
        public void ToggleLock()
        {
            if (isPurchased) return;

            isLocked = !isLocked;
            lockImage.sprite = isLocked ? lockedSprite : unlockedSprite;

            shopController?.SaveAllSlotStates(); // (추가)
        }

        // private 메서드
        // 잠금 버튼 클릭 시 호출됨 (수정됨)
        private void OnClickLockButton()
        {
            if (isPurchased) return;

            // 잠금비용 처리 코드
            //int lockCost = 100;
            //if (!isLocked && favorabilityMgr.testGold < lockCost)
            //{
            //    Debug.Log("잠금에 필요한 골드가 부족합니다.");
            //    return;
            //}

            string message = isLocked ? "정말 잠금을 해제하시겠습니까?" : "정말 잠그시겠습니까?";
            Vector3 worldPos = lockButton.transform.position;

            lockConfirmPanel.Open(() => ToggleLock(), worldPos, message, isLocked);
        }

        public bool IsLocked() => isLocked;
        public bool IsPurchased() => isPurchased;
        public ItemSlotData GetItem() => itemData;

    } // Scope by class RerollShopSlotHandler

} // namespace Root
