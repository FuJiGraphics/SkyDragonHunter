using SkyDragonHunter.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SkyDragonHunter {

    public class DungeonSceneController : MonoBehaviour
    {
        // Fields
        private DungeonUIMgr m_UIMgr;
        private DungeonType m_DungeonType;
        private int m_StageIndex;

        // Unity Methods
        private void Awake()
        {
            if (DungeonMgr.TryGetStageData(out DungeonType dungeonType, out int stageIndex))
            {
                m_DungeonType = dungeonType;
                m_StageIndex = stageIndex;
            }
            else
            {
                Debug.LogError($"Failed to syncronize dungeon information");
            }
        }

        private void Start()
        {
            Init();            
        }

        // Public Methods


        // Private Methods
        private void Init()
        {
            m_UIMgr = GameMgr.FindObject("DungeonUIManager").GetComponent<DungeonUIMgr>();
            SetStage();
        }

        private void SetStage()
        {
            switch (m_DungeonType)
            {
                case DungeonType.Type1:
                    SetStageType1();
                    break;
                case DungeonType.Type2:
                    SetStageType2();
                    break;
                case DungeonType.Type3:
                    SetStageType3();
                    break;
            }
        }

        private void SetStageType1()
        {

        }

        private void SetStageType2()
        {

        }

        private void SetStageType3()
        {

        }

    } // Scope by class DungeonProgressController

} // namespace Root