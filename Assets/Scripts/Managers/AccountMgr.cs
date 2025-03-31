using SkyDragonHunter.Gameplay;
using SkyDragonHunter.Structs;
using SkyDragonHunter.Tables;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace SkyDragonHunter.Managers
{
    public static class AccountMgr
    {
        // 필드 (Fields)
        public static int CurrentLevel { get; set; }
        public static CommonStats AccountStats { get; set; }
        public static CrystalData Crystal { get; set; }
        public static event Action onLevelUpEvents;

        // 속성 (Properties)
        // 외부 종속성 필드 (External dependencies field)
        // 이벤트 (Events)
        // Public 메서드
        public static void Init()
        {
            Debug.Log("AccountMgr Init");
            AccountStats = new CommonStats();
            var crystalData = DataTableManager.CrystalLevelTable.First;
            InitAccountData(crystalData);
        }

        public static void Release()
        {
            Debug.Log($"[AccountMgr] Account Stats 정리 중");
            CurrentLevel = 0;
            AccountStats = null;
            Crystal = new CrystalData();
        }

        public static void LevelUp()
        {
            if (Crystal.nextLevelId == 0)
            {
                Debug.Log("Max Level!!");
                return;
            }

            // 크리스탈 등급 증가
            var crystalData = DataTableManager.CrystalLevelTable.Get(Crystal.nextLevelId);
            InitAccountData(crystalData);

            // 이벤트 호출
            onLevelUpEvents?.Invoke();
        }

        // Private 메서드
        private static void InitAccountData(CrystalLevelData data)
        {
            Crystal = new CrystalData
            {
                level = data.Level,
                needExp = data.NeedEXP,
                atkUp = data.AtkUP,
                hpUp = data.HPUP,
                nextLevelId = data.NextLvID
            };

            // 계정 스탯과 통합
            CurrentLevel = Crystal.level;
            AccountStats.SetMaxDamage(AccountStats.MaxDamage.Value + Crystal.atkUp);
            AccountStats.ResetDamage();
            AccountStats.SetMaxHealth(AccountStats.MaxHealth.Value + Crystal.hpUp);
            AccountStats.ResetHealth();
        }
        // Others

    } // Scope by class GameMgr
} // namespace Root
