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
        // �ʵ� (Fields)
        public static int CurrentLevel { get; set; }
        public static CommonStats AccountStats { get; set; }
        public static CrystalData Crystal { get; set; }
        public static event Action onLevelUpEvents;

        // �Ӽ� (Properties)
        // �ܺ� ���Ӽ� �ʵ� (External dependencies field)
        // �̺�Ʈ (Events)
        // Public �޼���
        public static void Init()
        {
            Debug.Log("AccountMgr Init");
            AccountStats = new CommonStats();
            var crystalData = DataTableManager.CrystalLevelTable.First;
            InitAccountData(crystalData);
        }

        public static void Release()
        {
            Debug.Log($"[AccountMgr] Account Stats ���� ��");
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

            // ũ����Ż ��� ����
            var crystalData = DataTableManager.CrystalLevelTable.Get(Crystal.nextLevelId);
            InitAccountData(crystalData);

            // �̺�Ʈ ȣ��
            onLevelUpEvents?.Invoke();
        }

        // Private �޼���
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

            // ���� ���Ȱ� ����
            CurrentLevel = Crystal.level;
            AccountStats.SetMaxDamage(AccountStats.MaxDamage.Value + Crystal.atkUp);
            AccountStats.ResetDamage();
            AccountStats.SetMaxHealth(AccountStats.MaxHealth.Value + Crystal.hpUp);
            AccountStats.ResetHealth();
        }
        // Others

    } // Scope by class GameMgr
} // namespace Root
