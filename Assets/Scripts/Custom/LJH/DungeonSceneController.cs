using SkyDragonHunter.Entities;
using SkyDragonHunter.Interfaces;
using SkyDragonHunter.Managers;
using SkyDragonHunter.Structs;
using SkyDragonHunter.Tables;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Unity.VisualScripting;
using UnityEditor.U2D.Aseprite;
using UnityEngine;

namespace SkyDragonHunter.UI {

    public class DungeonSceneController : MonoBehaviour
    {
        // Fields
        private DungeonUIMgr m_UIMgr;

        private DungeonType m_DungeonType;
        private int m_StageIndex;

        private MonsterPrefabLoader m_MonsterPrefabLoader;
        [SerializeField] private Transform[] m_MonsterSpawnPositions;
        [SerializeField] private Transform m_BossSpawnPosition;
        [SerializeField] private Transform m_SandbagSpawnPosition;

        [SerializeField] private NewBossControllerBT m_SandbagPrefab;

        private NewBossControllerBT m_CachedBoss;
        private NewBossControllerBT m_CachedSandbag;
        [SerializeField] private List<NewMonsterControllerBT> m_MonsterList;
        private bool m_Cleared;
        private bool m_Failed;

        private float m_InitialTimeLimit;
        private float m_RemainingTime;
        private BigNum atkMultiplier;
        private BigNum hpMultiplier;

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
                case DungeonType.Wave:
                    UpdateWaveDungeon();
                    break;
                case DungeonType.Boss:
                    UpdateBossDungeon();
                    break;
                case DungeonType.SandBag:
                    UpdateSandbagDungeon();
                    break;
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

        private void SpawnWaveDungeonMonster()
        {
            var dungeonData = DataTableMgr.DungeonTable.GetCurrentDungeonData();
            var waveData = DataTableMgr.WaveTable.Get(dungeonData.MonsterWaveID);

            var waveSpawnCount = waveData.MonsterIDs.Length;
            if (waveSpawnCount != waveData.MonsterCounts.Length)
                Debug.LogError($"Length of Monster ID, Count do not match");

            for (int i = 0; i < waveSpawnCount; ++i)
            {
                for (int j = 0; j < waveData.MonsterCounts[i]; ++j)
                {
                    var randPosIndex = Random.Range(0, m_MonsterSpawnPositions.Length);
                    var spawned = Instantiate(m_MonsterPrefabLoader.GetMonsterAnimController(waveData.MonsterIDs[i]), m_MonsterSpawnPositions[randPosIndex].position, Quaternion.identity);
                    spawned.name = $"{DataTableMgr.MonsterTable.Get(waveData.MonsterIDs[i]).Name}";
                    var bt = spawned.GetComponent<NewMonsterControllerBT>();
                    bt.SetDataFromTable(waveData.MonsterIDs[i], hpMultiplier, atkMultiplier);

                    var destructableEvent = spawned.AddComponent<DestructableEvent>();
                    destructableEvent.destructEvent = new UnityEngine.Events.UnityEvent();

                    var captured = bt;
                    destructableEvent.destructEvent.AddListener(() => OnDungeonType2MonsterDie(captured));

                    m_MonsterList.Add(bt);
                }
            }

            //for (int i = 0; i < count; ++i)
            //{
            //    var randIndex = 2000000;
            //    var randFactor1 = Random.Range(1, 5) * 100;
            //    var randFactor2 = Random.Range(1, 6);
            //    randIndex += randFactor1 + randFactor2;

            //    var randPosIndex = Random.Range(0, m_MonsterSpawnPositions.Length);

            //    var monster = Instantiate(m_MonsterPrefabLoader.GetMonsterAnimController(randIndex), m_MonsterSpawnPositions[randPosIndex].position, Quaternion.identity);
            //    var monsterBT = monster.GetComponent<NewMonsterControllerBT>();
            //    var health = 100;
            //    var attack = 1;
            //    health *= atkMultiplier;
            //    attack *= atkMultiplier;
            //    monsterBT.MaxHP = health;
            //    Debug.Log($"max HP set to {monsterBT.MaxHP}");
            //    monsterBT.Damage = attack;

            //    // TODO: LJH value allocated are temp vals;
            //    var monsterData = DataTableMgr.MonsterTable.Get(randIndex);

            //    monsterBT.monsterStatus.speed = monsterData.Speed;
            //    monsterBT.monsterStatus.chaseSpeed = monsterData.ChaseSpeed;
            //    monsterBT.monsterStatus.attackInterval = monsterData.AttackInterval;
            //    monsterBT.monsterStatus.attackRange = monsterData.AttackRange;
            //    monsterBT.monsterStatus.aggroRange = monsterData.AggroRange;
            //    // ~TODO
                

            //    m_MonsterList.Add(monsterBT);
            //    var captured = monsterBT;
            //    var destructableEvent = monsterBT.AddComponent<DestructableEvent>();
            //    if (destructableEvent.destructEvent == null)
            //    {
            //        destructableEvent.destructEvent = new UnityEngine.Events.UnityEvent();
            //    }
            //    destructableEvent.destructEvent.AddListener(() => OnDungeonType2MonsterDie(captured));
            //}
            //m_SpawnTimer = c_SpawnDelay;
        }

        private void UpdateBossDungeon()
        {           
            if (m_CachedBoss == null)
            {
                Debug.LogError($"Boss Cannot be null in dungeon type 1");
                return;
            }
            
            m_UIMgr.InfoPanel.SetDungeonProgress(m_CachedBoss.HP, m_CachedBoss.MaxHP);
        }
        private void UpdateWaveDungeon()
        {
            if (m_SpawnTimer > 0f)
                m_SpawnTimer -= Time.deltaTime;

            if(m_MonsterList.Count < 2 && !(m_SpawnTimer > 0))
                SpawnWaveDungeonMonster();

        }
        private void UpdateSandbagDungeon()
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
            SetDungeon();
        }

        private void SetDungeon()
        {
            SetAirshipInDungeonDestructableEvent();
            switch (m_DungeonType)
            {
                case DungeonType.Wave:
                    SetWaveDungeon();
                    break;
                case DungeonType.Boss:
                    SetBossDungeon();
                    break;
                case DungeonType.SandBag:
                    SetSandbagDungeon();
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

        private void SetBossDungeon()
        {
            var dungeonData = DataTableMgr.DungeonTable.GetCurrentDungeonData();
            var bossId = dungeonData.BossID;   

            var boss = Instantiate(m_MonsterPrefabLoader.GetMonsterAnimController(bossId), m_BossSpawnPosition.position, Quaternion.identity);
            m_CachedBoss = boss.GetComponent<NewBossControllerBT>();
            var bossData = DataTableMgr.BossTable.Get(4100001);

            BigNum health = bossData.HP;
            BigNum atk = bossData.ATK;

            atk *= dungeonData.MultiplierATK;
            health *= dungeonData.MultiplierHP;

            m_CachedBoss.MaxHP = health;
            m_CachedBoss.MaxATK = atk;
            m_CachedBoss.bossStatus.speed = bossData.Speed;
            m_CachedBoss.bossStatus.chaseSpeed = bossData.ChaseSpeed;
            m_CachedBoss.bossStatus.aggroRange = bossData.AggroRange;
            m_CachedBoss.bossStatus.attackRange = bossData.AttackRange;
            m_CachedBoss.bossStatus.attackInterval = bossData.AttackInterval;

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

        private void SetWaveDungeon()
        {
            var dungeonData = DataTableMgr.DungeonTable.GetCurrentDungeonData();
                        

            atkMultiplier = dungeonData.MultiplierATK;
            hpMultiplier = dungeonData.MultiplierHP;

            m_InitialTimeLimit = 40f;
            m_RemainingTime = m_InitialTimeLimit;
            m_SpawnTimer = 0.2f;

            m_Dungeon2KillGoal = dungeonData.KillCount;
            m_Dungeon2KillCount = 0;

            m_UIMgr.InfoPanel.SetDungeonProgress(m_Dungeon2KillCount, m_Dungeon2KillGoal);
        }

        private void SetSandbagDungeon()
        {
            var dungeonData = DataTableMgr.DungeonTable.GetCurrentDungeonData();

            var boss = Instantiate(m_SandbagPrefab, m_BossSpawnPosition.position, Quaternion.identity);
            m_CachedBoss = boss.GetComponent<NewBossControllerBT>();
            var bossData = DataTableMgr.BossTable.Get(4200001);

            BigNum health = bossData.HP;
            BigNum atk = bossData.ATK;

            atk *= dungeonData.MultiplierATK;
            health *= dungeonData.MultiplierHP;

            m_CachedBoss.MaxHP = health;
            m_CachedBoss.MaxATK = atk;

            m_InitialTimeLimit = 40f;
            m_RemainingTime = m_InitialTimeLimit;

            var destructableEvent = m_CachedBoss.AddComponent<DestructableEvent>();
            if (destructableEvent.destructEvent == null)
            {
                destructableEvent.destructEvent = new UnityEngine.Events.UnityEvent();
            }
            destructableEvent.destructEvent.AddListener(OnStageClear);
        }

        private void OnStageClear()
        {

            if (m_DungeonType == DungeonType.Boss)
                m_UIMgr.InfoPanel.SetDungeonProgress(0, m_CachedBoss.MaxHP);
            if (m_DungeonType == DungeonType.SandBag)
                m_UIMgr.InfoPanel.SetDungeonProgress(0, m_CachedSandbag.MaxHP);

            switch (DungeonMgr.CurrentDungeonType)
            {
                case DungeonType.Wave:
                    var waveTicketCount = AccountMgr.ItemCount(ItemType.WaveDungeonTicket);
                    waveTicketCount -= 1;
                    AccountMgr.SetItemCount(ItemType.WaveDungeonTicket, waveTicketCount);
                    break;
                case DungeonType.Boss:
                    var bossTicketCount = AccountMgr.ItemCount(ItemType.BossDungeonTicket);
                    bossTicketCount -= 1;
                    AccountMgr.SetItemCount(ItemType.BossDungeonTicket, bossTicketCount);
                    break;
                case DungeonType.SandBag:
                    var sandbagTicketCount = AccountMgr.ItemCount(ItemType.SandbagDungeonTicket);
                    sandbagTicketCount -= 1;
                    AccountMgr.SetItemCount(ItemType.SandbagDungeonTicket, sandbagTicketCount);
                    break;
            }
            var dungeonData = DataTableMgr.DungeonTable.GetCurrentDungeonData();
            AccountMgr.AddItemCount(DataTableMgr.ItemTable.Get(dungeonData.RewardItemID).Type, dungeonData.RewardCounts);
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
                AccountMgr.Diamond += 500;
                OnStageClear();
            }
        }
    } // Scope by class DungeonProgressController

} // namespace Root