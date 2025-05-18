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
        public float summonExp;

        public float bgmVol;
        public float sfxVol;
        public bool isDisplayDmg;
        public int autoPowerSavingMins = 0;

        public void InitData()
        {
            isGuest = true;
            guestId = 0;
            userId = "DefaultID";
            userNickName = "DefaultUserName";
            accountCreatedTime = DateTime.UtcNow;
            crystalLevel = 1;
            summonExp = 0;
            bgmVol = 0.5f;
            sfxVol = 0.5f;
        }

        public void UpdateSavedData()
        {
            userNickName = AccountMgr.Nickname;
            crystalLevel = AccountMgr.Crystal.CurrentLevel;
            crystalLevelId = AccountMgr.Crystal.CurrLevelId;
            lastOnlineTime = DateTime.UtcNow;
            var summonUi = GameMgr.FindObject<UISummonPanel>("UISummonPanel");
            if (summonUi != null)
            {
                summonExp = summonUi.Exp;
            }
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
            var summonUi = GameMgr.FindObject<UISummonPanel>("UISummonPanel");
            if (summonUi != null)
            {
                summonUi.Exp = summonExp;
            }
        }

    } // Scope by class SavedAccountData

} // namespace Root