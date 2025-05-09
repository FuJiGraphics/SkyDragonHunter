using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SkyDragonHunter {

    public class SavedAirshipData
    {
        // equipped info
        public int[] mountedCrewIDs;
        public int equippedCannonID;
        public int equippedRepairerID;
        public int equippedArtifactInstanceID; // Hash ID

        public void InitData()
        {
            mountedCrewIDs = new int[4];
            equippedCannonID = 0;
            equippedRepairerID = 0;
            equippedArtifactInstanceID = 0;
        }
        public void UpdateData()
        {

        }
        public void ApplySavedData()
        {

        }
    } // Scope by class SavedAirshipData

} // namespace Root