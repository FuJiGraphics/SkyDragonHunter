using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SkyDragonHunter 
{
    /// <summary>
    /// 친밀도 시스템 전반을 관리하는 클래스
    /// - 레벨/경험치 계산
    /// - 하루 최대 15 경험치 제한
    /// - 일반 구매: 가격의 5% 경험치 적립
    /// - 리롤 구매: 고정 5 경험치 적립
    /// </summary>
    public class FavorailityMgr : MonoBehaviour
    {
        [Serializable]
        public class FavorabilityData
        {
            public int level = 1;            // 현재 친밀도 레벨
            public int currentExp = 0;       // 현재 누적된 총 경험치
        
            public int ExpToNextLevel => level * 300;
        }
        // 필드 (Fields)
        public int testGold = 100000;      // 임시 테스트용 골드
        public int testDiamond = 10000;    // 임시 테스트용 다이아

        private int goldDailyExp = 0;
        private int diamondDailyExp = 0;
        // 최대치 상수
        private const int GoldMaxDaily = 15;
        private const int DiamondMaxDaily = 15;

        private DateTime lastResetDate;
        // 속성 (Properties)
        // 외부 종속성 필드 (External dependencies field)
        // 이벤트 (Events)
        // 유니티 (MonoBehaviour 기본 메서드)
        [Header("친밀도 데이터")]
        [SerializeField] private FavorabilityData data = new();

        private void Awake()
        {
            ResetCurrencyDailyIfNewDay();
        }

        // Public 메서드

        // ===== 외부에서 접근 가능한 API =====
        public int GetLevel() => data.level;
        public int GetCurrentExp() => data.currentExp;
        public int GetExpToNext() => data.ExpToNextLevel;
        /// <summary>
        /// 리롤 상점에서 아이템 구매 시 호출 (5 경험치 고정 추가)
        /// </summary>
        public void GainExpFromRerollPurchase()
        {
            data.currentExp += 50;
            CheckLevelUp();
        }

        /// <summary>
        /// 일반 상점(골드/다이아) 구매 시 가격의 5%만큼 경험치 적립
        /// </summary>
        public void GainExpFromCurrencyPurchase(int price, ShopType shopType)
        {
            ResetCurrencyDailyIfNewDay(); // 매번 진입 시 리셋 체크

            int exp = Mathf.FloorToInt(price * 0.05f);
            if (exp <= 0) return;

            switch (shopType)
            {
                case ShopType.Gold:
                    if (goldDailyExp >= GoldMaxDaily) return;
                    int goldGain = Mathf.Min(exp, GoldMaxDaily - goldDailyExp);
                    data.currentExp += goldGain;
                    goldDailyExp += goldGain;
                    break;

                case ShopType.Diamond:
                    if (diamondDailyExp >= DiamondMaxDaily) return;
                    int diaGain = Mathf.Min(exp, DiamondMaxDaily - diamondDailyExp);
                    data.currentExp += diaGain;
                    diamondDailyExp += diaGain;
                    break;
            }

            CheckLevelUp();
        }

        /// <summary>
        /// 현재 친밀도 레벨 기준 할인율(0.0 ~ 1.0) 반환
        /// 예: 레벨 1 → 0%, 레벨 2 → 5%, 레벨 3 → 10% ...
        /// </summary>
        public float GetDiscountRate()
        {
            // 예시: 레벨 * 0.05f, 최대 30% 제한
            return Mathf.Clamp01((data.level - 1) * 0.02f);
        }

        // Private 메서드
        // 레벨업 조건 확인 및 레벨 증가 처리
        private void CheckLevelUp()
        {
            // 현재 경험치가 다음 레벨까지 필요한 경험치 이상이면
            while (data.currentExp >= data.ExpToNextLevel)
            {
                data.currentExp -= data.ExpToNextLevel;     // 초과 경험치는 이월
                data.level++;                               // 레벨 증가
            }

            // 여기서 슬롯 가격 갱신
            UpdateAllSlotDiscountPrices();
        }

        // 골드/다이아 제한 리셋
        private void ResetCurrencyDailyIfNewDay()
        {
            if (lastResetDate.Date != DateTime.Now.Date)
            {
                goldDailyExp = 0;
                diamondDailyExp = 0;
                lastResetDate = DateTime.Now;
            }
        }

        private void UpdateAllSlotDiscountPrices()
        {
            float newRate = GetDiscountRate();

            // 일반 상점 슬롯
            foreach (var slot in FindObjectsOfType<ShopSlotHandler>())
            {
                slot.UpdateDiscountedPrice(newRate);
            }

            // 리롤 상점 슬롯
            foreach (var slot in FindObjectsOfType<RerollShopSlotHandler>())
            {
                slot.UpdateDiscountedPrice(newRate);
            }
        }

        // Others

    } // Scope by class FavorailityMgr

} // namespace Root