using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SkyDragonHunter.SaveLoad 
{
    public class SavedArtifact
    {
        int instanceID;
        int artifactID;
    }

    public class SavedArtifactsData
    {
        public Dictionary<int, SavedArtifact> artifactDict;

        public void InitData()
        {
            artifactDict = new Dictionary<int, SavedArtifact>();


        }

        public void UpdateData()
        {

        }
        public void ApplySavedData()
        {

        }
    } // Scope by class SavedArtifactsData
} // namespace Root