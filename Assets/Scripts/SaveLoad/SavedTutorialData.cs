using CsvHelper.TypeConversion;
using Newtonsoft.Json;
using SkyDragonHunter.Managers;
using SkyDragonHunter.Structs;
using SkyDragonHunter.Tables;
using System;
using System.Collections.Generic;

namespace SkyDragonHunter.SaveLoad
{
    public class SavedTutorialData
    {
        public bool tutorialCleared;

        public void InitData()
        {
            tutorialCleared = false;
        }

        public void UpdateSavedData()
        {
            var tuto = GameMgr.FindObject<TutorialMgr>("TutorialMgr");
            if (tuto != null)
            {
                tutorialCleared = GameMgr.FindObject<TutorialMgr>("TutorialMgr").TutorialEnd;
            }
        }

        public void ApplySavedData()
        {
            var tuto = GameMgr.FindObject<TutorialMgr>("TutorialMgr");
            if (tuto != null)
            {
                GameMgr.FindObject<TutorialMgr>("TutorialMgr").TutorialEnd = tutorialCleared;
            }
        }

    } // Scope by class SavedItemData

} // namespace Root