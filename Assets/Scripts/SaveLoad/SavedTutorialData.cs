using CsvHelper.TypeConversion;
using Newtonsoft.Json;
using SkyDragonHunter.Managers;
using SkyDragonHunter.Structs;
using SkyDragonHunter.Tables;
using System;
using System.Collections.Generic;

namespace SkyDragonHunter.SaveLoad
{
    public class SavedTutorial
    {
        public bool oneClear;
        public bool isStartTutorial;
        public bool tutorialEnd;
    }

    public class SavedTutorialData
    {
        public SavedTutorial savedTutorial;

        public void InitData()
        {
            savedTutorial = new SavedTutorial();
            savedTutorial.oneClear = false;
            savedTutorial.isStartTutorial = true;
            savedTutorial.tutorialEnd = false;
        }

        public void UpdateSavedData()
        {
            savedTutorial.oneClear = TutorialEndValue.OneClear;
            savedTutorial.isStartTutorial = TutorialEndValue.IsStartTutorial;
            savedTutorial.tutorialEnd = TutorialEndValue.TutorialEnd;
        }

        public void ApplySavedData()
        {
            TutorialEndValue.OneClear = savedTutorial.oneClear;
            TutorialEndValue.IsStartTutorial = savedTutorial.isStartTutorial;
            TutorialEndValue.TutorialEnd = savedTutorial.tutorialEnd;
        }

    } // Scope by class SavedItemData

} // namespace Root