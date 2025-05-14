using SkyDragonHunter.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SkyDragonHunter.SaveLoad 
{
    public class SavedStageData
    {
        public int currentStage;
        public int currentZone;

        public int clearedStage;
        public int clearedZone;

        public void InitData()
        {
            currentStage = 1;
            currentZone = 1;

            clearedStage = 1;
            clearedZone = 0;
        }
        public void UpdateSavedData()
        {
            var waveControllerGo = GameMgr.FindObject("WaveController");
            if(waveControllerGo == null)
            {
                // Debug.LogError($"Update Stage Data Failed, WaveController GO Null");
                return;
            }
            if(!waveControllerGo.TryGetComponent<TestWaveController>(out var waveController))
            {
                // Debug.LogError($"Update Stage Data Failed, WaveController Null");
                return;
            }
            currentStage = waveController.CurrentTriedMissionLevel;
            currentZone = waveController.CurrentTriedZonelLevel;
            GetClearedLevels(waveController.LastTriedMissionLevel, waveController.LastTriedZoneLevel, out int newClearedStage, out int newClearedZone);
            clearedStage = newClearedStage;
            clearedZone = newClearedZone;
        }

        public void ApplySavedData()
        {
            var waveControllerGo = GameMgr.FindObject("WaveController");
            if (waveControllerGo == null)
            {
                // Debug.LogError($"Apply Stage Data Failed, WaveController GO Null");
                return;
            }

           
            if (waveControllerGo.TryGetComponent<TestWaveController>(out var waveController))
            {
                AccountMgr.CurrentStageLevel = currentStage;
                AccountMgr.CurrentStageZoneLevel = currentZone;
                waveController.CurrentTriedMissionLevel = currentStage;
                waveController.CurrentTriedZonelLevel = currentZone;
                GetLastTriedLevels(clearedStage, clearedZone, out int newTriedMission, out int newTriedZone);
                waveController.LastTriedMissionLevel = newTriedMission;
                waveController.LastTriedZoneLevel = newTriedZone;
                //waveController.Init();
            }
        }

        public int GetTriedMission()
        {
            GetLastTriedLevels(clearedStage, clearedZone, out int newTriedMission, out int newTriedZone);
            return newTriedMission;
        }

        public int GetTriedZone()
        {
            GetLastTriedLevels(clearedStage, clearedZone, out int newTriedMission, out int newTriedZone);
            return newTriedZone;
        }        

        private bool GetClearedLevels(int triedMission, int triedZone, out int clearedMission, out int clearedZone)
        {
            if(triedZone == 1)
            {
                if(triedMission <= 1)
                {
                    clearedMission = 1;
                    clearedZone = 0;
                    return true;
                }
                else
                {
                    clearedMission = triedMission - 1;
                    clearedZone = 20;
                    return true;
                }
            }
            else
            {
                clearedMission = triedMission;
                clearedZone = triedZone - 1;
                return true;
            }
        }

        private bool GetLastTriedLevels(int clearedMission, int clearedZone, out int triedMission, out int triedZone)
        {
            if(clearedZone == 20)
            {
                if(clearedMission >= 10)
                {
                    triedMission = 10;
                    triedZone = 10;
                    return true;
                }
                else
                {
                    triedMission = clearedMission + 1;
                    triedZone = 1;
                    return true;
                }
            }
            else
            {
                triedMission = clearedMission;
                triedZone = clearedZone + 1;
                return true;
            }
        }
    } // Scope by class SavedStageData

} // namespace Root