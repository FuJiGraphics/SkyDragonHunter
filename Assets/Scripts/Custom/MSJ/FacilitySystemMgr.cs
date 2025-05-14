using SkyDragonHunter.test;
using SkyDragonHunter.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SkyDragonHunter.UI {

    public class FacilitySystemMgr : MonoBehaviour
    {
        [Serializable]
        public class FacilityData
        {
            public FacilityType type;                 // 시설 종류 (금/다이아/나무 등)
            public int level = 1;                     // 시설 레벨
            public int currentExp = 0;                // 현재 누적 경험치
            public int expToLevelUp = 100;            // 레벨업 필요 경험치
            public int itemCount = 0;                 // 현재 생성된 아이템 수
            public float timer = 0f;                  // 아이템 생성용 타이머 누적값
            public float generateInterval = 10f;      // 아이템 생성 주기 (초)
            public float levelUpCooldown = 0f; // 남은 레벨업 대기 시간
            public float levelUpInterval => level * 10f; // 레벨업에 필요한 시간
            public bool isInLevelUpCooldown => levelUpCooldown > 0f;

            public ItemSlotData itemToGenerate;         // 해당 슬롯에서 생성할 아이템

            // 현재 레벨에 따른 최대 보유량 (ex: Lv1 → 20, Lv2 → 40 ...)
            public int maxCount => level * 20;

            // 현재 레벨에 따른 생성 단위량 (ex: Lv1 → 5, Lv2 → 10 ...)
            public int perGenerate => level * 5;

            // TODO: LJH
            public DateTime upgradeStartedTime = DateTime.MinValue;
            public DateTime lastAccquiredTime = DateTime.MinValue;

            
            // ~TODO
        }
        // 필드 (Fields)
        // [유니티 인스펙터에서 할당] 각 시설 슬롯의 상태 리스트
        [Header("슬롯별 상태 관리")]
        public List<FacilityData> facilityList = new();

        // 속성 (Properties)
        // 외부 종속성 필드 (External dependencies field)
        // 이벤트 (Events)
        // 유니티 (MonoBehaviour 기본 메서드)
        private void Update()
        {
            UpdateTimers(Time.deltaTime);
        }

        // Public 메서드
        /// <summary>
        /// 시설 유형에 따른 상태 객체 반환
        /// </summary>
        public FacilityData GetFacility(FacilityType type)
        {
            return facilityList.Find(f => f.type == type);
        }

        /// <summary>
        /// 외부에서 호출하여 각 시설의 타이머를 업데이트하고 조건 만족 시 아이템 생성
        /// </summary>
        public void UpdateTimers(float deltaTime)
        {
            foreach (var data in facilityList)
            {
                if (data.levelUpCooldown > 0f)
                {
                    data.levelUpCooldown -= deltaTime;
                    if (data.levelUpCooldown < 0f) data.levelUpCooldown = 0f;
                    continue; // 레벨업 대기 중에는 생산 금지
                }
                // 최대 개수 도달 시 생성 중단
                if (data.itemCount >= data.maxCount)
                    continue;

                data.timer += deltaTime;

                // 생성 간격 이상 시간이 지나면 아이템 생성
                if (data.timer >= data.generateInterval)
                {
                    // perGenerate 만큼 생성하되, maxCount를 초과하지 않게 제한
                    data.itemCount = Mathf.Min(data.itemCount + data.perGenerate, data.maxCount);

                    // 생성 이후 타이머 리셋
                    data.timer = 0f;
                }
            }
        }
        // Private 메서드
        /// <summary>
        /// 시설 슬롯별로 생성할 아이템 지정하는 초기화 함수 (외부에서 호출 필요)
        /// </summary>
        // Others

    } // Scope by class FacilitySystemMgr

} // namespace Root