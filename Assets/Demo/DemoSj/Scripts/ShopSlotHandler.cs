using SkyDragonHunter.test;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace SkyDragonHunter
{
    [System.Serializable]
    public class ShopSlotState
    {
        public ItemStatus item;
        public int currentCount;

        public ShopSlotState(ItemStatus item)
        {
            this.item = item;
            this.currentCount = item.maxCount;
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
        private int currentPrice; // 할인 적용된 실제 가격

        [Header("통화 아이콘 스프라이트")]
        [SerializeField] private Sprite diamondSprite;
        [SerializeField] private Sprite goldSprite;
        // 슬롯별 상태 객체
        // 속성 (Properties)
        // 외부 종속성 필드 (External dependencies field)
        // 이벤트 (Events)
        // 유니티 (MonoBehaviour 기본 메서드)
        // Public 메서드
        public void UpdateDiscountedPrice(float discountRate)
        {
            currentPrice = Mathf.FloorToInt(slotState.item.price * (1f - discountRate));
            priceText.text = currentPrice.ToString("N0");
        }

        // 외부에서 슬롯 초기화 시 상태 객체를 전달
        public void Initialize(ShopSlotState state, FavorailityMgr manager, ShopType type)
        {
            slotState = state;
            shopType = type;
            favorabilityMgr = manager;

            // 할인율 계산
            float discountRate = favorabilityMgr.GetDiscountRate();
            currentPrice = Mathf.FloorToInt(state.item.price * (1f - discountRate)); // 할인된 가격 저장

            var item = state.item;

            nameText.text = item.itemName;
            itemImage.sprite = item.itemImage;
            priceText.text = currentPrice.ToString("N0");

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
                if (favorabilityMgr.testGold < currentPrice)
                {
                    Debug.Log("골드 부족으로 구매 불가");
                    return;
                }
                favorabilityMgr.testGold -= currentPrice;
            }
            else if (shopType == ShopType.Diamond)
            {
                if (favorabilityMgr.testDiamond < currentPrice)
                {
                    Debug.Log("다이아 부족으로 구매 불가");
                    return;
                }
                favorabilityMgr.testDiamond -= currentPrice;
            }


            if (slotState.currentCount <= 0)
            {
                Debug.Log($"[{slotState.item.itemName}] 구매 불가: 남은 수량 없음");
                return;
            }

            // 할인 가격 기준 재화 차감
            //if (!currencySystem.TryConsume(currentPrice)) return;

            slotState.currentCount--;     // 상태에 직접 반영
            Debug.Log($"[{slotState.item.itemName}] 구매 완료!");
            UpdateLimitText();

            favorabilityMgr.GainExpFromCurrencyPurchase(currentPrice, shopType);
        }
    }
    // Others

} // Scope by class ShopSlotHandler

// namespace Root