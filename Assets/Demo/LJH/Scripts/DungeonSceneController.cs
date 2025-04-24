using SkyDragonHunter.Entities;
using SkyDragonHunter.Interfaces;
using SkyDragonHunter.Managers;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace SkyDragonHunter.UI {

    public class DungeonSceneController : MonoBehaviour
    {
        // Fields
        private DungeonUIMgr m_UIMgr;

        private DungeonType m_DungeonType;
        private int m_StageIndex;

        private MonsterPrefabLoader m_MonsterPrefabLoader;
        [SerializeField] private Transform m_BossSpawnPosition;
        [SerializeField] private Transform m_SandbagSpawnPosition;
        [SerializeField] private Transform[] m_MonsterSpawnPositions;

        [SerializeField] private NewBossControllerBT m_SandbagPrefab;

        private NewBossControllerBT m_CachedBoss;
        private NewBossControllerBT m_CachedSandbag;
        [SerializeField] private List<NewMonsterControllerBT> m_MonsterList;
        private bool m_Cleared;
        private bool m_Failed;

        private float m_InitialTimeLimit;
        private float m_RemainingTime;
        private int m_StatMultiplier;

        private const float c_SpawnDelay = 0.2f;
        private float m_SpawnTimer;

        private int m_Dungeon2KillGoal;
        private int m_Dungeon2KillCount;

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
            if (m_Cleared || m_Failed)
                return;

            m_RemainingTime -= Time.deltaTime;
            m_UIMgr.InfoPanel.SetDungeonTimer(m_RemainingTime, m_InitialTimeLimit);

            if (m_RemainingTime < 0f)
            {
                m_RemainingTime = 0f;
                m_Failed = true;
                m_UIMgr.OnDungeonClearFailed();
            }

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
            m_RemainingTime -= Time.deltaTime;
            var destructables = m_CachedBoss.GetComponents<IDestructible>();
            foreach (var destructable in destructables)
            {
                destructable.OnDestruction(gameObject);
            }
        }

        private void SpawnDungeonType2Monster(int count = 1)
        {
            for(int i = 0; i < count; ++i)
            {
                var randIndex = 100000;
                var randFactor1 = Random.Range(0, 4) * 10;
                var randFactor2 = Random.Range(1, 7);
                randIndex += randFactor1 + randFactor2;

                var randPosIndex = Random.Range(0, m_MonsterSpawnPositions.Length);

                var monster = Instantiate(m_MonsterPrefabLoader.GetMonsterAnimController(randIndex), m_MonsterSpawnPositions[randPosIndex].position, Quaternion.identity);
                var monsterBT = monster.GetComponent<NewMonsterControllerBT>();
                var health = 100;
                var attack = 1;
                health *= m_StatMultiplier;
                attack *= m_StatMultiplier;
                monsterBT.MaxHP = health;
                Debug.Log($"max HP set to {monsterBT.MaxHP}");
                monsterBT.Damage = attack;

                m_MonsterList.Add(monsterBT);
                var captured = monsterBT;
                var destructableEvent = monsterBT.AddComponent<DestructableEvent>();
                if (destructableEvent.destructEvent == null)
                {
                    destructableEvent.destructEvent = new UnityEngine.Events.UnityEvent();
                }
                destructableEvent.destructEvent.AddListener(() => OnDungeonType2MonsterDie(captured));
            }
            m_SpawnTimer = c_SpawnDelay;
        }

        private void UpdateDungeonType1()
        {           
            if (m_CachedBoss == null)
            {
                Debug.LogError($"Boss Cannot be null in dungeon type 1");
                return;
            }
            
            m_UIMgr.InfoPanel.SetDungeonProgress(m_CachedBoss.HP, m_CachedBoss.MaxHP);
        }
        private void UpdateDungeonType2()
        {
            if (m_SpawnTimer > 0f)
                m_SpawnTimer -= Time.deltaTime;

            if(m_MonsterList.Count < 10 && !(m_SpawnTimer > 0))
                SpawnDungeonType2Monster(5);

        }
        private void UpdateDungeonType3()
        {
            if (m_CachedSandbag == null)
            {
                Debug.LogError($"Sandbag Cannot be null in dungeon type 3");
                return;
            }

            m_UIMgr.InfoPanel.SetDungeonProgress(m_CachedSandbag.HP, m_CachedSandbag.MaxHP);
        }

        private void Init()
        {
            m_UIMgr = GameMgr.FindObject("DungeonUIManager").GetComponent<DungeonUIMgr>();
            m_MonsterPrefabLoader = GameMgr.FindObject("MonsterPrefabLoader").GetComponent<MonsterPrefabLoader>();
            m_MonsterList = new List<NewMonsterControllerBT>();
            SetStage();
        }

        private void SetStage()
        {
            SetAirshipInDungeonDestructableEvent();
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

        private void SetAirshipInDungeonDestructableEvent()
        {
            var airshipGo = GameMgr.FindObject("Airship");
            var airshipTempDestructable = airshipGo.GetComponent<AirshipTempDestructible>();
            if(airshipTempDestructable != null)
            {                
                Destroy(airshipTempDestructable);
            }

            var destructableEvent = airshipGo.AddComponent<DestructableEvent>();
            if (destructableEvent == null)
            {
                Debug.LogError($"Error occured when attaching destructableEvent on Airship");
                return;
            }
            destructableEvent.destructEvent = new UnityEngine.Events.UnityEvent();
            destructableEvent.destructEvent.AddListener(() =>
            {
                m_Failed = true;
                m_UIMgr.OnDungeonClearFailed();
            });
            
        }

        private void SetStageType1()
        {
            var boss = Instantiate(m_MonsterPrefabLoader.GetMonsterAnimController(300004), m_BossSpawnPosition.position, Quaternion.identity);
            m_CachedBoss = boss.GetComponent<NewBossControllerBT>();
            var health = m_CachedBoss.MaxHP;
            health = 300000;
            for(int i = 1; i < m_StageIndex; ++i)
            {
                health *= 12;
            }
            m_CachedBoss.MaxHP = health;

            m_InitialTimeLimit = 40f;
            m_RemainingTime = m_InitialTimeLimit;

            var destructableEvent = m_CachedBoss.AddComponent<DestructableEvent>();
            if (destructableEvent.destructEvent == null)
            {
                destructableEvent.destructEvent = new UnityEngine.Events.UnityEvent();
            }
            destructableEvent.destructEvent.AddListener(OnStageClear);
        }

        private void OnEvent(int a)
        {

        }

        private void SetStageType2()
        {
            m_StatMultiplier = 1;
            for(int i = 1; i < m_StageIndex; ++i)
            {
                m_StatMultiplier *= 25;
            }
            m_InitialTimeLimit = 40f;
            m_RemainingTime = m_InitialTimeLimit;
            m_SpawnTimer = 0.2f;

            m_Dungeon2KillGoal = 40;
            m_Dungeon2KillCount = 0;

            m_UIMgr.InfoPanel.SetDungeonProgress(m_Dungeon2KillCount, m_Dungeon2KillGoal);
        }

        private void SetStageType3()
        {
            m_CachedSandbag = Instantiate(m_SandbagPrefab, m_SandbagSpawnPosition.position, Quaternion.identity);
            
            var health = m_CachedSandbag.MaxHP;
            health = 1000000;
            for (int i = 1; i < m_StageIndex; ++i)
            {
                health *= 15;
            }
            m_CachedSandbag.MaxHP = health;            

            m_InitialTimeLimit = 40f;
            m_RemainingTime = m_InitialTimeLimit;

            var destructableEvent = m_CachedSandbag.AddComponent<DestructableEvent>();
            if (destructableEvent.destructEvent == null)
            {
                destructableEvent.destructEvent = new UnityEngine.Events.UnityEvent();
            }
            destructableEvent.destructEvent.AddListener(OnStageClear);
        }

        private void OnStageClear()
        {
            if (m_DungeonType == DungeonType.Type1)
                m_UIMgr.InfoPanel.SetDungeonProgress(0, m_CachedBoss.MaxHP);
            if (m_DungeonType == DungeonType.Type3)
                m_UIMgr.InfoPanel.SetDungeonProgress(0, m_CachedSandbag.MaxHP);
            AccountMgr.Ticket -= 1;
            m_UIMgr.EnableClearedPanel(true);
            DungeonMgr.OnStageClear();
            
            m_Cleared = true;
        }

        private void OnDungeonType2MonsterDie(NewMonsterControllerBT monster)
        {
            if(m_MonsterList.Contains(monster))
            {
                m_MonsterList.Remove(monster);
            }
            m_Dungeon2KillCount = Mathf.Clamp(m_Dungeon2KillCount + 1, 0, m_Dungeon2KillGoal);
            m_UIMgr.InfoPanel.SetDungeonProgress(m_Dungeon2KillCount, m_Dungeon2KillGoal);
            if (m_Dungeon2KillCount >= m_Dungeon2KillGoal && !(m_Failed || m_Cleared))
            {
                OnStageClear();
            }
        }
    } // Scope by class DungeonProgressController

} // namespace Root