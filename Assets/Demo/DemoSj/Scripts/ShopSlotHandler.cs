using SkyDragonHunter.test;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SkyDragonHunter
{

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

        private ItemStatus itemData;     // 원본 아이템 데이터
        private int currentCount;        // 슬롯 개별 구매 가능 수
        // 속성 (Properties)
        // 외부 종속성 필드 (External dependencies field)
        // 이벤트 (Events)
        // 유니티 (MonoBehaviour 기본 메서드)
        private void Start()
        {

        }

        private void Update()
        {

        }

        // Public 메서드
        // 외부에서 슬롯 초기화 시 호출
        public void Initialize(ItemStatus item)
        {
            itemData = item;
            currentCount = item.MaxCount; // 처음엔 최대 구매 가능 수 만큼 가능

            nameText.text = item.itemName;
            itemImage.sprite = item.itemImage;
            priceText.text = item.price.ToString("N0");
            currencyImage.sprite = item.currencyTypeIcon;

            UpdateLimitText();

            buyButton.onClick.RemoveAllListeners();
            buyButton.onClick.AddListener(TryBuy); // 버튼 클릭 시 구매 시도
        }
        // Private 메서드
        // UI 텍스트에 구매 가능 수 갱신
        private void UpdateLimitText()
        {
            Debug.Log($"[슬롯 초기화] {itemData.itemName} 구매 제한 표시: {purchaseLimitText != null}, 값: {purchaseLimitText.text}");
            purchaseLimitText.text = $"{currentCount} / {itemData.MaxCount}";
        }

        // 실제 구매 로직
        private void TryBuy()
        {
            // 남은 수량 확인
            if (currentCount <= 0)
            {
                Debug.Log("구매 불가: 남은 수량 없음");
                return;
            }

            // 구매 처리 로직 (나중에 재화 차감 등 연결 가능)
            Debug.Log($"구매 완료: {itemData.itemName}");

            currentCount--;      // 남은 수량 감소
            UpdateLimitText();   // UI 갱신
        }
    }
    // Others

} // Scope by class ShopSlotHandler

// namespace Root