using SkyDragonHunter.Managers;
using SkyDragonHunter.Test;
using SkyDragonHunter.UI;
using System;
using UnityEngine;
using static Unity.Burst.Intrinsics.X86.Avx;

namespace SkyDragonHunter.SaveLoad 
{
    public class SavedAccountData
    {
        public bool isGuest;
        public int guestId;
        public string userId;
        public string userNickName;
        public DateTime accountCreatedTime;

        public int crystalLevel;
        public int crystalLevelId;
        public DateTime lastOnlineTime;

        public void InitData()
        {
            isGuest = true;
            guestId = 0;
            userId = "DefaultID";
            userNickName = "DefaultUserName";
            accountCreatedTime = DateTime.UtcNow;
            crystalLevel = 1;
        }

        public void UpdateSavedData()
        {
            userNickName = AccountMgr.Nickname;
            crystalLevel = AccountMgr.Crystal.CurrentLevel;
            crystalLevelId = AccountMgr.Crystal.CurrLevelId;
            lastOnlineTime = DateTime.UtcNow;
        }

        public void ApplySavedData()
        {
            AccountMgr.Nickname = userNickName;
            AccountMgr.LoadLevel(crystalLevelId);
            var inGameMainFramePanelGo = GameMgr.FindObject("InGameMainFramePanel");
            if (inGameMainFramePanelGo != null &&
                inGameMainFramePanelGo.TryGetComponent<UIInGameMainFramePanel>(out var inGameMainFramePanel))
            {
                inGameMainFramePanel.Nickname = userNickName;
                inGameMainFramePanel.Level = AccountMgr.Crystal.CurrentLevel.ToString();
                inGameMainFramePanel.AtkText = AccountMgr.Crystal.IncreaseDamage.ToUnit();
                inGameMainFramePanel.HpText = AccountMgr.Crystal.IncreaseHealth.ToUnit();
            }

        }

    } // Scope by class SavedAccountData

} // namespace Root