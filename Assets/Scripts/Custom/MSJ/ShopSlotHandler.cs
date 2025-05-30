using SkyDragonHunter.Managers;
using SkyDragonHunter.Structs;
using SkyDragonHunter.test;
using SkyDragonHunter.UI;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace SkyDragonHunter
{
    [System.Serializable]
    public class ShopSlotState
    {
        public ItemSlotData item;
        public int currentCount;

        public ShopSlotState(ItemSlotData item)
        {
            this.item = item;
            this.currentCount = item.currCount;
        }
    }

    public class ShopSlotHandler : MonoBehaviour
    {
        // 필드 (Fields)
        [Header("UI 연결 요소")]
        public TextMeshProUGUI nameText;              // 아이템 이름 텍스트
        public Image itemImage;                       // 아이템 이미지
        public TextMeshProUGUI purchaseLimitText;     // 구매 제한 표시 텍스트
        public Image currencyImage;                   // 재화 아이콘
        public TextMeshProUGUI priceText;             // 가격 텍스트
        public Button buyButton;                      // 구매 버튼
        public FavorailityMgr favorabilityMgr;         // 상점 친밀도 가지고있는 객체

        private ShopSlotState slotState;
        private ShopType shopType;
        private BigNum currentPrice; // 할인 적용된 실제 가격

        [Header("통화 아이콘 스프라이트")]
        [SerializeField] private Sprite diamondSprite;
        [SerializeField] private Sprite goldSprite;
        // 슬롯별 상태 객체
        // 속성 (Properties)
        // 외부 종속성 필드 (External dependencies field)
        // 이벤트 (Events)
        // 유니티 (MonoBehaviour 기본 메서드)
        private void OnEnable()
        {
            buyButton.onClick.RemoveAllListeners();
            buyButton.onClick.AddListener(TryBuy);
        }

        // Public 메서드

        public void UpdateDiscountedPrice(float discountRate)
        {
            currentPrice = slotState.item.price * (1f - discountRate);
            priceText.text = currentPrice.ToUnit();
        }

        // 외부에서 슬롯 초기화 시 상태 객체를 전달
        public void Initialize(ShopSlotState state, FavorailityMgr manager, ShopType type)
        {
            slotState = state;
            shopType = type;
            favorabilityMgr = manager;

            // 할인율 계산
            float discountRate = favorabilityMgr.GetDiscountRate();
            currentPrice = slotState.item.price * (1f - discountRate); // 할인된 가격 저장

            var item = state.item.GetData();

            nameText.text = item.Name;
            itemImage.sprite = item.Icon;
            priceText.text = currentPrice.ToUnit();

            // 통화 스프라이트는 상점 타입에 따라 고정
            currencyImage.sprite = (shopType == ShopType.Diamond) ? diamondSprite : goldSprite;

            UpdateLimitText();

            buyButton.onClick.RemoveAllListeners();
            buyButton.onClick.AddListener(TryBuy);
        }
        // Private 메서드
        // UI 텍스트에 구매 가능 수 갱신
        private void UpdateLimitText()
        {
            purchaseLimitText.text = $"{slotState.currentCount} / {slotState.item.maxCount}";
            buyButton.interactable = slotState.currentCount > 0;
        }

        // 실제 구매 로직
        private void TryBuy()
        {
            if (shopType == ShopType.Gold)
            {
                if (AccountMgr.Coin < currentPrice)
                {
                    var diff = currentPrice - AccountMgr.Coin;
                    DrawableMgr.Dialog("알림", $"골드가 부족합니다. = {new BigNum(diff).ToUnit()}");
                    return;
                }
                AccountMgr.Coin -= currentPrice;
            }
            else if (shopType == ShopType.Diamond)
            {
                if (AccountMgr.Diamond < currentPrice)
                {
                    var diff = currentPrice - AccountMgr.Diamond;
                    DrawableMgr.Dialog("알림", $"다이아 부족합니다. = {new BigNum(diff).ToString()}");
                    return;
                }
                AccountMgr.Diamond -= currentPrice;
            }


            var itemData = slotState.item.GetData();

            if (slotState.item.maxCount != 0 && slotState.currentCount <= 0)
            {
                DrawableMgr.Dialog("Alert", $"[{itemData.Name}] 구매 불가: 남은 수량 없음");
                return;
            }

            if (slotState.item.maxCount != 0)
            {
                slotState.currentCount--;     // 상태에 직접 반영
            }
            // 할인 가격 기준 재화 차감
            //if (!currencySystem.TryConsume(currentPrice)) return;

            DrawableMgr.Dialog("Alert", $"[{itemData.Name}] 구매 완료!");
            UpdateLimitText();

            favorabilityMgr.GainExpFromCurrencyPurchase(currentPrice, shopType);

            BigNum currentItemCount = AccountMgr.ItemCount(itemData.Type) + 1;
            AccountMgr.SetItemCount(itemData.Type, currentItemCount);
        }
    }
    // Others

} // Scope by class ShopSlotHandler

// namespace Root