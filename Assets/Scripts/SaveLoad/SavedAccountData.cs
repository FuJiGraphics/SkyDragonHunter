using SkyDragonHunter.Managers;
using SkyDragonHunter.Test;
using System;
using UnityEngine;

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
            lastOnlineTime = DateTime.UtcNow;
        }

        public void ApplySavedData()
        {
            AccountMgr.Nickname = userNickName;
            TempUserData.s_CrystalLevelID = crystalLevel;            
        }

    } // Scope by class SavedAccountData

} // namespace Root