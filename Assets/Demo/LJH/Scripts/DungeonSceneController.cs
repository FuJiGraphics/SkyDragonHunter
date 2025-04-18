using SkyDragonHunter.Entities;
using SkyDragonHunter.Interfaces;
using SkyDragonHunter.Managers;
using Unity.VisualScripting;
using UnityEngine;

namespace SkyDragonHunter
{

    public class DungeonSceneController : MonoBehaviour
    {
        // Fields
        private DungeonUIMgr m_UIMgr;

        private DungeonType m_DungeonType;
        private int m_StageIndex;

        private MonsterPrefabLoader m_MonsterPrefabLoader;
        [SerializeField] private Transform m_SpawnPosition;

        private NewBossControllerBT m_CachedBoss;
        private bool m_Cleared;
        private bool m_Failed;

        private float m_InitialTimeLimit;
        private float m_RemainingTime;

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
            if (m_CachedBoss == null)
            {
                Debug.LogError($"Boss Null");
                return;
            }

            var destructables = m_CachedBoss.GetComponents<IDestructible>();
            foreach (var destructable in destructables)
            {
                destructable.OnDestruction(gameObject);
            }
        }

        private void UpdateDungeonType1()
        {
            if (m_Cleared || m_Failed)
                return;

            if (m_CachedBoss == null)
            {
                Debug.LogError($"Boss Cannot be null in dungeon type 1");
                return;
            }
            m_RemainingTime -= Time.deltaTime;
            if (m_RemainingTime < 0f)
            {
                m_RemainingTime = 0f;
                m_Failed = true;
                m_UIMgr.OnDungeonClearFailed();
            }

            m_UIMgr.InfoPanel.SetDungeonProgress(m_CachedBoss.HP, m_CachedBoss.MaxHP);
            m_UIMgr.InfoPanel.SetDungeonTimer(m_RemainingTime, m_InitialTimeLimit);

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
            m_CachedBoss = boss.GetComponent<NewBossControllerBT>();
            m_InitialTimeLimit = 40f;
            m_RemainingTime = m_InitialTimeLimit;
            var destructableEvent = m_CachedBoss.AddComponent<DestructableEvent>();
            if (destructableEvent.destructEvent == null)
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