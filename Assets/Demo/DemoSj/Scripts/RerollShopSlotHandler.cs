using SkyDragonHunter.test;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SkyDragonHunter
{
    public class RerollShopSlotHandler : MonoBehaviour
    {
        // 필드 (Fields)
        [Header("UI 연결")]
        [SerializeField] private FavorabilityUIController favorabilityUIController;       
        [SerializeField] private FavorailityMgr favorabilityMgr;       
        [SerializeField] private TextMeshProUGUI itemNameText;       // 아이템 이름 텍스트
        [SerializeField] private Image itemImage;                    // 아이템 이미지
        [SerializeField] private Image currencyImage;                // 재화 아이콘 (골드/다이아 등)
        [SerializeField] private CurrencyType currencyType;                // 재화 아이콘 (골드/다이아 등)
        [SerializeField] private TextMeshProUGUI priceText;          // 가격 텍스트
        [SerializeField] private Button buyButton;                   // 구매 버튼 ← 추가

        [Header("잠금 기능")]
        [SerializeField] private Button lockButton;                  // 자물쇠 버튼
        [SerializeField] private Image lockImage;                    // 잠금 상태 표시 이미지
        [SerializeField] private Sprite lockedSprite;                // 잠겼을 때 스프라이트
        [SerializeField] private Sprite unlockedSprite;              // 풀렸을 때 스프라이트

        private bool isLocked = false;               // 현재 슬롯 잠금 여부
        private bool isPurchased = false;            // 해당 슬롯에서 구매가 발생했는지 여부
        private ItemStatus itemData;                 // 슬롯에 표시 중인 아이템 데이터
        private int currentPrice; // 할인 적용된 가격 (내부 저장)

        // Public 메서드

        public void UpdateDiscountedPrice(float discountRate)
        {
            currentPrice = Mathf.FloorToInt(itemData.price * (1f - discountRate));
            priceText.text = currentPrice.ToString("N0");
        }

        // 슬롯에 아이템 적용 (외부에서 호출)
        public void Initialize(ItemStatus item, FavorailityMgr favorMgr, FavorabilityUIController favorUIController)
        {
            favorabilityMgr = favorMgr;
            favorabilityUIController = favorUIController;
            itemData = item;
            isLocked = false;         // 초기화 시 잠금 해제
            isPurchased = false;      // 초기화 시 미구매 상태

            // 친밀도 할인율 계산
            float discountRate = favorabilityMgr.GetDiscountRate(); // 예: 0.15f → 15% 할인
            currentPrice = Mathf.FloorToInt(item.price * (1f - discountRate));

            // UI 적용
            itemNameText.text = item.itemName;
            itemImage.sprite = item.itemImage;
            currencyImage.sprite = item.currencyTypeIcon;
            currencyType = item.currencyType;
            priceText.text = currentPrice.ToString();

            // 버튼 및 잠금 이미지 초기화
            lockImage.sprite = unlockedSprite;
            lockImage.gameObject.SetActive(true);

            lockButton.interactable = true;
            buyButton.interactable = true;

            // 잠금 버튼 리스너 연결
            lockButton.onClick.RemoveAllListeners();
            lockButton.onClick.AddListener(ToggleLock);

            // 구매 버튼 리스너 연결 ← 추가
            buyButton.onClick.RemoveAllListeners();
            buyButton.onClick.AddListener(TryBuy);

            buyButton.interactable = true; // 초기화 시 버튼 활성화
        }

        // 실제 구매 처리 함수 (버튼에서 호출됨)
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

            if (isPurchased)
                return;

            // 재화 차감 처리 (할인 적용된 가격 사용)
            //if (!currencySystem.TryConsume(currentPrice)) return;

            // 구매 처리
            isPurchased = true;    // 구매됨 표시
            isLocked = false;      // 잠금 자동 해제

            lockImage.sprite = unlockedSprite;       // 자물쇠 이미지 업데이트
            buyButton.interactable = false;          // 더 이상 구매 불가
            lockButton.interactable = false;         // 자물쇠 버튼도 비활성화

            favorabilityMgr.GainExpFromRerollPurchase();
            favorabilityUIController.UpdateUI();
        }

        // 잠금 상태 토글 (버튼 클릭 시 호출)
        private void ToggleLock()
        {
            if (isPurchased)
                return; // 이미 구매된 경우 토글 불가

            isLocked = !isLocked;
            lockImage.sprite = isLocked ? lockedSprite : unlockedSprite;
        }

        // 외부에서 잠금 상태 확인
        public bool IsLocked()
        {
            return isLocked;
        }

        // 외부에서 현재 아이템 데이터 확인
        public ItemStatus GetItem()
        {
            return itemData;
        }

        // 외부에서 구매 여부 확인
        public bool IsPurchased()
        {
            return isPurchased;
        }

    } // Scope by class RerollShopSlotHandler

} // namespace Root
