using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SkyDragonHunter.SaveLoad 
{
    public class SavedArtifact
    {
        int eigenID;
        int artifactID;
    }

    public class SavedArtifactsData : MonoBehaviour
    {
        public Dictionary<int, SavedArtifact> artifactDict;

        public void InitData()
        {
            artifactDict = new Dictionary<int, SavedArtifact>();


        }

        public void UpdateData()
        {

        }
    } // Scope by class SavedArtifactsData
} // namespace Root