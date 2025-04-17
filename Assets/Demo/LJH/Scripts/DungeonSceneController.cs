using SkyDragonHunter.Entities;
using SkyDragonHunter.Interfaces;
using SkyDragonHunter.Managers;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace SkyDragonHunter {

    public class DungeonSceneController : MonoBehaviour
    {
        // Fields
        private DungeonUIMgr m_UIMgr;

        private DungeonType m_DungeonType;
        private int m_StageIndex;

        private MonsterPrefabLoader m_MonsterPrefabLoader;
        [SerializeField] private Transform m_SpawnPosition;

        private NewBossControllerBT m_cachedBoss;
        private bool m_Cleared;

        // Unity Methods
        private void Awake()
        {
            m_Cleared = false;
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

        private void Update()
        {
            switch (m_DungeonType)
            {
                case DungeonType.Type1:
                    UpdateDungeonType1();
                    break;
                case DungeonType.Type2:
                    UpdateDungeonType2();
                    break;
                case DungeonType.Type3:
                    UpdateDungeonType3();
                    break;
            }

            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                TestKillBoss();
            }
        }

        // Public Methods
        

        // Private Methods
        public void TestKillBoss()
        {
            if(m_cachedBoss == null)
            {
                Debug.LogError($"Boss Null");
                return;
            }

            var destructables = m_cachedBoss.GetComponents<IDestructible>();
            foreach (var destructable in destructables)
            {
                destructable.OnDestruction(gameObject);
            }
        }

        private void UpdateDungeonType1()
        {
            if (m_Cleared)
                return;

            if (m_cachedBoss == null)
            {
                Debug.LogError($"Boss Cannot be null in dungeon type 1");
                return;
            }
            
        }
        private void UpdateDungeonType2()
        {

        }
        private void UpdateDungeonType3()
        {

        }

        private void Init()
        {
            m_UIMgr = GameMgr.FindObject("DungeonUIManager").GetComponent<DungeonUIMgr>();
            m_MonsterPrefabLoader = GameMgr.FindObject("MonsterPrefabLoader").GetComponent<MonsterPrefabLoader>();
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
            var boss = Instantiate(m_MonsterPrefabLoader.GetMonsterAnimController(300004), m_SpawnPosition.position, Quaternion.identity);
            m_cachedBoss = boss.GetComponent<NewBossControllerBT>();
            var destructableEvent = m_cachedBoss.AddComponent<DestructableEvent>();
            if(destructableEvent.destructEvent == null)
            {
                destructableEvent.destructEvent = new UnityEngine.Events.UnityEvent();
            }
            destructableEvent.destructEvent.AddListener(OnStageClear);

            
        }

        private void SetStageType2()
        {

        }

        private void SetStageType3()
        {

        }

        private void OnStageClear() 
        {
            m_UIMgr.EnableClearedPanel(true);
            m_Cleared = true;
        }

    } // Scope by class DungeonProgressController

} // namespace Root