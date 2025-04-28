using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
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

        public int atkLevel;
        public int hpLevel;
        public int defLevel;
        public int regLevel;
        public int critRateLevel;
        public int critDmgLevel;

        public DateTime lastOnlineTime;

        public void InitData()
        {
            isGuest = true;
            guestId = 0;
            userId = "DefaultID";
            userNickName = "DefaultUserName";
            accountCreatedTime = DateTime.UtcNow;

            crystalLevel = 1;

            atkLevel = 0;
            hpLevel = 0;
            defLevel = 0;
            regLevel = 0;
            critRateLevel = 0;
            critDmgLevel = 0;
        }
        public void UpdateData()
        {

        }
    } // Scope by class SavedAccountData

} // namespace Root