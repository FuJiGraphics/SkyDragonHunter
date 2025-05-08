using SkyDragonHunter.Entities;
using SkyDragonHunter.Game;
using SkyDragonHunter.Gameplay;
using SkyDragonHunter.Managers;
using SkyDragonHunter.Structs;
using SkyDragonHunter.Tables;
using SkyDragonHunter.Test;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;



namespace SkyDragonHunter
{

    public struct StageInfo
    {
        public int missionLevel;
        public int zoneLevel;
    }


    public class TestWaveController : MonoBehaviour
    {
        public static int instanceNo = 0;

        // 필드 (Fields)        
        [SerializeField] private Slider waveSlider;
        [SerializeField] private TextMeshProUGUI waveLevelText;
        [SerializeField] private GameObject clearPanel;
        [SerializeField] private GameObject feildPanel;
        [SerializeField] private GameObject backGround;
        [SerializeField] private BackGroundController backGroundController;
        [SerializeField] private GameObject airship;
        [SerializeField] private GameObject monster;
        [SerializeField] private GameObject boss;
        [SerializeField] private BoxCollider2D spawnArea;
        [SerializeField] private float maxWaveTime;
        [SerializeField] private float bossTimer;
        [SerializeField] private MonsterPrefabLoader prefabLoader;
        [SerializeField] private List<GameObject> currentEnemy;
        [SerializeField] private Sprite[] TestBackGround;
        [SerializeField] private Sprite[] TestBackGroundMid;
        [SerializeField] private Sprite[] TestBackGroundFront;
        // [SerializeField] private int spawnableMonsters = 10;
        public TutorialController tutorialController;
        public Slider bossSlider;
        public Slider bossTimerSlider;
        public int CurrentTriedZonelLevel => currentZonelLevel;
        public int CurrentTriedMissionLevel => currentMissionLevel;
        private float oldAllSpeed;
        private float currentWaveTime;
        private float currentSpawnTime;
        private float inpiniteModSpawnDelay = 3;
        private float distance;
        private bool isStopped;
        private bool isCanSpawn;
        private bool isSuccessChangeBackGround;
        private int backGroundIndex;
        private int currentMissionLevel = 1;
        private int currentZonelLevel = 1;
        private int lastTriedZonelLevel = 1;
        private int lastTriedMissionLevel = 1;
        private int currentSpawnMonsters = 0;
        private float currentOpenPanel = 0f;
        private StageInfo stageInfo;
        private bool changeSuccess = true;
        private Coroutine coroutine;
        private Dictionary<Image, Color> waveSliderOriginalColors = new Dictionary<Image, Color>();
        private Dictionary<Image, Color> bossSliderOriginalColors = new Dictionary<Image, Color>();
        private StageData stageData;
        private float waveSpawnDistance;
        private float waveDuration;
        private const float c_InitialBossTimer = 40f;
        private bool isBossCleared;
        private bool checkDistance;
        private TutorialMgr m_TutorialMgr;

        // 속성 (Properties)
        public bool isInfiniteMode { get; private set; } = false;
        public bool isBossSpawn = false;
        public int LastTriedZoneLevel => lastTriedZonelLevel;
        public int LastTriedMissionLevel => lastTriedMissionLevel;

        // 외부 종속성 필드 (External dependencies field)
        // 이벤트 (Events)
        // 유니티 (MonoBehaviour 기본 메서드)
        private void Start()
        {
            m_TutorialMgr = GameMgr.FindObject<TutorialMgr>("TutorialMgr");
            if (m_TutorialMgr.IsStartTutorial)
            {
                tutorialController.tutorialPanel.gameObject.SetActive(true);
            }
        }

        private void Update()
        {
            //backGroundController.SetScrollSpeed(1f);
            CheckMonterProximity();
            OnBackGroundStop();
            if (!isInfiniteMode)
            {
                NormalWaveUpdate();
            }
            else
            {
                infiniteWaveUpdate();
            }
            // TODO: LJH
            UpdateBossSliders();
            // ~TODO

            if (!tutorialController.tutorialMgr.TutorialEnd && tutorialController.tutorialPanel.gameObject.activeSelf)
            {
                Time.timeScale = 0f;
            }
            else
            {
                Time.timeScale = 1f;
            }

            UpdateTutorialState();
        }

        // Public 메서드
        public void UpdateTutorialState()
        {
            if (!m_TutorialMgr.IsStartTutorial)
                return;

            if (isStopped && checkDistance)
            {
                if (!tutorialController.endTutorial && !tutorialController.firstActive)
                {
                    tutorialController.OnFirstActive();
                }
            }
            else
            {
                if (!tutorialController.endTutorial && tutorialController.fourthActive &&
                    !tutorialController.fifthActive && lastTriedZonelLevel == 2 && changeSuccess)
                {
                    tutorialController.OnFifthActive();
                }
                if (!tutorialController.endTutorial && tutorialController.fifthActive &&
                    !tutorialController.sixthActive && lastTriedZonelLevel == 3 && changeSuccess)
                {
                    tutorialController.OnSixthActive();
                }
            }
        }

        public void Init()
        {
            stageInfo.missionLevel = 1;
            stageInfo.zoneLevel = 1;
            maxWaveTime = 10f;
            currentWaveTime = 0f;
            waveSlider.maxValue = maxWaveTime;
            waveSlider.minValue = 0;
            backGroundController = backGround.GetComponent<BackGroundController>();
            oldAllSpeed = backGround.GetComponent<BackGroundController>().scrollSpeed;
            bossSlider.gameObject.SetActive(false);
            prefabLoader = GameMgr.FindObject("MonsterPrefabLoader").GetComponent<MonsterPrefabLoader>();
            isCanSpawn = true;
            isStopped = false;
            currentEnemy = new List<GameObject>();
            currentMissionLevel = AccountMgr.CurrentStageLevel;
            currentZonelLevel = AccountMgr.CurrentStageZoneLevel;
            isSuccessChangeBackGround = true;
            SetStageText(currentMissionLevel, currentZonelLevel);
            //OnSaveLastClearWave();
            OnTestWaveFailedUnActive();
            SpwanAreaPositionSet();
            CacheWaveSliderColors();
            CacheBossSliderColors();
            stageData = DataTableMgr.StageTable.Get(currentMissionLevel, currentZonelLevel);
            waveSpawnDistance = stageData.WaveDistance;
            waveDuration = stageData.WaveLength;
            if (currentMissionLevel >= 1 && currentZonelLevel > 1)
            {
                OnGoToLoadCurrentWave();
            }
        }

        public void SetStageText(int stageLevel, int stageZoneLevel)
        {
            waveLevelText.text = string.Format("{0} - {1}", currentMissionLevel, currentZonelLevel);
        }

        public void OnSelectWave(int misson, int zone)
        {
            if (misson > lastTriedMissionLevel)
            {
                Debug.Log("들린적이 없는 미션입니다!!");
                return;
            }
            else
            {
                currentMissionLevel = misson;
            }

            if (zone > lastTriedZonelLevel)
            {
                Debug.Log("들린적이 없는 존입니다!!");
                return;
            }
            else
            {
                currentZonelLevel = zone;
            }
            AccountMgr.SaveUserData();
            OnGoSelectCurrentWave();
        }

        public void OnOffInfiniteMod()
        {
            if (isSuccessChangeBackGround && changeSuccess)
            {
                if (isInfiniteMode)
                {
                    isInfiniteMode = false;
                    isStopped = false;
                    waveSlider.gameObject.SetActive(false);
                    OnSetLastTriedtWave();
                }
                else
                {
                    isInfiniteMode = true;
                    isStopped = false;
                    OnSetCurrentWave();
                }
                isSuccessChangeBackGround = false;
            }

        }

        public void OnTestWaveFailedActive()
        {
            feildPanel.SetActive(true);
            isInfiniteMode = true;
            OnSetCurrentWave();
        }

        public void OnTestWaveFailedUnActive()
        {
            feildPanel.SetActive(false);
        }

        public void ReStartAll()
        {
            Debug.Log("리스타트.");
            clearPanel.SetActive(false);
            currentZonelLevel = 1;
            currentMissionLevel = 1;
            currentWaveTime = 0f;
            backGroundIndex = 0;
            OnSetCurrentWave();
            OnTestWaveFailedUnActive();
        }

        // Private 메서드
        private void OnClearMonster()
        {
            GameObject[] monsters = GameObject.FindGameObjectsWithTag("Monster");

            foreach (GameObject monster in monsters)
            {
                Destroy(monster);
            }
        }


        private void NormalWaveUpdate()
        {
            if (changeSuccess && 
                (currentEnemy == null || currentEnemy.All(e => e == null || e.Equals(null))))
            {
                currentWaveTime += Time.deltaTime * oldAllSpeed;
                currentSpawnTime += Time.deltaTime * oldAllSpeed;
                waveSlider.value = currentWaveTime;
                isStopped = false;
            }
            else
            {
                isStopped = true;
            }

            if (!isCanSpawn && currentWaveTime < waveDuration && 
                (currentEnemy == null || currentEnemy.All(e => e == null || e.Equals(null))))
            {
                isCanSpawn = true;
            }

            if (isCanSpawn && currentSpawnTime >= waveSpawnDistance && 
                (currentEnemy == null || currentEnemy.All(e => e == null || e.Equals(null))))
            {
                OnSpawnMonster();
                currentSpawnTime = 0;
            }


            if (!isBossSpawn && currentWaveTime >= waveDuration && isCanSpawn && 
                (currentEnemy == null || currentEnemy.All(e => e == null || e.Equals(null))))
            {
                OnSpawnBoss();
                isBossSpawn = true;
            }

            if (isBossSpawn && currentWaveTime > waveDuration && 
                (currentEnemy == null || currentEnemy.All(e => e == null || e.Equals(null))))
            {
                OnActiveClearPanel(); // 클리어 패널 활성화
                OnSaveLastClearWave();
                currentOpenPanel += Time.deltaTime;
                if (currentOpenPanel > 2f)
                {
                    OnUnActiveClearPanel(); // 클리어 패널 종료
                    currentZonelLevel++;
                    if (currentZonelLevel > 20)
                    {
                        currentZonelLevel = 1;
                        currentMissionLevel++;

                    }

                    SetStageText(currentMissionLevel, currentZonelLevel);
                    currentWaveTime = 0f;
                    isCanSpawn = true;
                    currentOpenPanel = 0f;
                    lastTriedMissionLevel = currentMissionLevel;
                    lastTriedZonelLevel = currentZonelLevel;
                    stageData = DataTableMgr.StageTable.Get(currentMissionLevel, currentZonelLevel);
                    waveSpawnDistance = stageData.WaveDistance;
                    waveDuration = stageData.WaveLength;
                    waveSlider.maxValue = waveDuration;
                    OnChangeBackGround(currentMissionLevel - 1);
                    // TODO: 데이터 세이브
                    AccountMgr.SaveUserData();
                }
            }
        }

        private void infiniteWaveUpdate()
        {
            if (!isStopped)
            {
                currentWaveTime += Time.deltaTime * oldAllSpeed;
                waveSlider.value = maxWaveTime;
            }

            if (!isCanSpawn && (currentEnemy == null || currentEnemy.All(e => e == null || e.Equals(null))))
            {
                isCanSpawn = true;
                currentWaveTime = 0f;
            }

            if (currentWaveTime > inpiniteModSpawnDelay && isCanSpawn)
            {
                OnSpawnMonster();
            }
        }

        private void OnSetLastTriedtWave()
        {
            OnClearMonster();
            isCanSpawn = true;
            currentWaveTime = 0f;
            currentSpawnTime = 0f;
            currentMissionLevel = lastTriedMissionLevel;
            currentZonelLevel = lastTriedZonelLevel;
            waveLevelText.text = string.Format("{0} - {1}", currentMissionLevel, currentZonelLevel);
            OnChangeBackGround(currentMissionLevel - 1);
        }

        private void OnSetCurrentWave()
        {
            OnClearMonster();
            isCanSpawn = true;
            currentWaveTime = 0f;
            currentSpawnTime = 0f;
            currentMissionLevel = stageInfo.missionLevel;
            currentZonelLevel = stageInfo.zoneLevel;
            waveLevelText.text = string.Format("{0} - {1}", currentMissionLevel, currentZonelLevel);
            OnChangeBackGround(currentMissionLevel - 1);
        }

        private void OnGoSelectCurrentWave()
        {
            OnClearMonster();
            isCanSpawn = true;
            currentWaveTime = 0f;
            isInfiniteMode = true;
            waveLevelText.text = string.Format("{0} - {1}", currentMissionLevel, currentZonelLevel);
            OnChangeBackGround(currentMissionLevel - 1);
        }

        private void OnGoToLoadCurrentWave()
        {
            OnClearMonster();
            isCanSpawn = true;
            currentWaveTime = 0f;
            if (lastTriedMissionLevel > currentMissionLevel ||
                lastTriedZonelLevel > currentZonelLevel)
            {
                isInfiniteMode = true;
            }
            else
            {
                isInfiniteMode = false;
            }
            waveLevelText.text = string.Format("{0} - {1}", currentMissionLevel, currentZonelLevel);
            OnChangeBackGround(currentMissionLevel - 1);
        }

        private void OnSaveLastClearWave()
        {
            stageInfo.missionLevel = currentMissionLevel;
            stageInfo.zoneLevel = currentZonelLevel;
        }

        private void OnBackGroundStop()
        {
            if (isStopped)
            {
                backGroundController.SetScrollSpeed(0f);
            }
            else
            {
                backGroundController.SetScrollSpeed(oldAllSpeed);
            }
            
        }

        private void CheckMonterProximity()
        {
            checkDistance = false;

            foreach (var monster in currentEnemy)
            {
                if (monster == null) continue;

                if (!monster.CompareTag("Monster")) continue; // 몬스터 태그 필터링

                float distance = Vector3.Distance(airship.transform.position, monster.transform.position);
                if (distance <= 50f)
                {
                    checkDistance = true;
                }
                else
                {
                    checkDistance = false; 
                }
            }
        }

        private void OnSpawnMonster()
        {
            // TODO: LJH
            var currentWaveData = DataTableMgr.WaveTable.Get(stageData.WaveTableID);

            var waveSpawnCount = currentWaveData.MonsterIDs.Length;
            if (waveSpawnCount != currentWaveData.MonsterCounts.Length)
                Debug.LogError($"Length of Monster ID, Count do not match");
            

            for(int i = 0; i < waveSpawnCount; ++i)
            {
                for(int j = 0; j < currentWaveData.MonsterCounts[i]; ++j)
                {
                    var spawned = Instantiate(prefabLoader.GetMonsterAnimController(currentWaveData.MonsterIDs[i]), GetRandomSpawnAreaInPosition(), Quaternion.identity);
                    spawned.name = $"{DataTableMgr.MonsterTable.Get(currentWaveData.MonsterIDs[i]).Name}{instanceNo++}";
                    var bt = spawned.GetComponent<NewMonsterControllerBT>();
                    bt.SetDataFromTable(currentWaveData.MonsterIDs[i], stageData.MonsterMultiplierHP, stageData.MonsterMultiplierATK);

                    var destructableEvent = spawned.AddComponent<DestructableEvent>();
                    destructableEvent.destructEvent = new UnityEngine.Events.UnityEvent();
                    destructableEvent.destructEvent.AddListener(() =>
                    {
                        AccountMgr.Coin += stageData.MonsterGOLD;

                        var randVal = Random.Range(0, 1f);
                        bool isGenerateDungenTicket = randVal < 0.7f;
                        if (isGenerateDungenTicket)
                        {
                            AccountMgr.WaveDungeonTicket += 1;
                        }

                        AccountMgr.CurrentExp += stageData.MonsterEXP;
                    });

                    currentEnemy.Add(spawned.gameObject);
                    currentSpawnMonsters++;
                }
            }
            isCanSpawn = false;
        }

        private void OnSpawnBoss()
        {
            //GameObject spawned = Instantiate(boss, GetRandomSpawnAreaInPosition(), Quaternion.identity);
            //currentEnemy.Add(spawned);
            //currentSpawnMonsters++;
            //isCanSpawn = false;
            isBossCleared = false;
            bossTimer = c_InitialBossTimer;
            waveSlider.gameObject.SetActive(false);
            bossSlider.gameObject.SetActive(true);
            RestoreBossSliderColors();

            int bossId = stageData.ChallengeBossID;
            Vector3 bossSpawnPos = spawnArea.transform.position + new Vector3(spawnArea.size.x * 0.5f, spawnArea.size.y * -0.5f, 0);
            var spawned = Instantiate(prefabLoader.GetMonsterAnimController(bossId), bossSpawnPos, Quaternion.identity);
            var bossBT = spawned.GetComponent<NewBossControllerBT>();
            bossBT.SetDataFromTable(bossId, stageData.BossMultiplierHP, stageData.BossMultiplierATK);

            var destructableEvent = spawned.AddComponent<DestructableEvent>();
            destructableEvent.destructEvent = new UnityEngine.Events.UnityEvent();
            destructableEvent.destructEvent.AddListener(() =>
            {
                isBossCleared = true;
                bossSlider.value = 0;
                AccountMgr.Diamond += 200;
            });
            currentEnemy.Add(spawned.gameObject);
            currentSpawnMonsters++;
            isCanSpawn = false;

            ///
            //int tempId = 300_001;
            //var prefabLoader = GameMgr.FindObject("MonsterPrefabLoader").GetComponent<MonsterPrefabLoader>();
            //Vector3 bossSpawnPos = spawnArea.transform.position + new Vector3(spawnArea.size.x * 0.5f, spawnArea.size.y * -0.5f, 0);
            //var spawned = Instantiate(prefabLoader.GetMonsterAnimController(tempId), bossSpawnPos, Quaternion.identity);
            //
            //BigNum newHP = 20000;
            //int stage = 0;
            //stage += (currentMissionLevel - 1) * 20;
            //stage += currentZonelLevel;
            //float multiplier = Mathf.Pow(1.6f, stage);
            //var bossBT = spawned.GetComponent<NewBossControllerBT>();
            //bossBT.MaxHP = newHP * multiplier;
            //
            //var destructableEvent = spawned.AddComponent<DestructableEvent>();
            //destructableEvent.destructEvent = new UnityEngine.Events.UnityEvent();
            //destructableEvent.destructEvent.AddListener(() =>
            //{
            //    isBossCleared = true;
            //    bossSlider.value = 0;
            //});
            //
            //currentEnemy.Add(spawned.gameObject);
            //currentSpawnMonsters++;
            //isCanSpawn = false;
        }

        private void OnActiveClearPanel()
        {
            clearPanel.SetActive(true);
        }

        private void OnUnActiveClearPanel()
        {
            clearPanel.SetActive(false);
            isBossSpawn = false;
            currentSpawnTime = 0;
            // ItemMgr.Reset();
        }

        private void OnFadeInSlider()
        {
            foreach (Image image in waveSlider.GetComponentsInChildren<Image>())
            {
                string name = image.gameObject.name;
                if (name == "Background" || name == "Fill" || name == "Handle")
                {
                    StartCoroutine(WaveSliderFade(image));
                }
            }
        }

        private void OnFadeOutSlider()
        {
            foreach (Image image in bossSlider.GetComponentsInChildren<Image>())
            {
                string name = image.gameObject.name;
                if (name == "HealthBackground" || name == "HealthFill" || 
                    name == "TimerBackground" || name == "TimerFill")
                {
                    StartCoroutine(BossSliderFadeOut(image));
                }
            }
        }

        private void OnChangeBackGround(int bgIndex)
        {
            backGroundIndex = bgIndex;
            if (backGroundIndex > 9)
            {
                backGroundIndex = 9;
            }
            clearPanel.SetActive(false);
            foreach (Transform child in backGround.transform)
            {
                SpriteRenderer ctrl = child.GetComponent<SpriteRenderer>();
                if (ctrl != null)
                {
                    if (ctrl.gameObject.name == "BackGround")
                    {
                        StartCoroutine(ChangeBackgroundWithFade(ctrl, backGroundIndex));
                    }
                    else if (ctrl.gameObject.name == "MidGround")
                    {
                        StartCoroutine(ChangeBackgroundWithFade(ctrl, backGroundIndex));
                    }
                    else if (ctrl.gameObject.name == "ForeGround")
                    {
                        StartCoroutine(ChangeBackgroundWithFade(ctrl, backGroundIndex));
                    }
                }
            }
        }

        private void SpwanAreaPositionSet()
        {
            float cameraHeight = Camera.main.orthographicSize * 2f;
            float cameraWidth = cameraHeight * 2f;
            spawnArea.transform.position = new Vector3(airship.transform.position.x + cameraWidth,
                Camera.main.orthographicSize * 0.5f, airship.transform.position.z);
        }

        public Vector2 GetRandomSpawnAreaInPosition()
        {
            if (spawnArea == null)
            {
                Debug.LogError("SpawnArea(BoxCollider2D)가 할당되지 않았습니다.");
                return Vector2.zero;
            }

            Vector2 size = spawnArea.size;
            Vector2 offset = spawnArea.offset;

            // 로컬 기준 랜덤 위치
            Vector2 localRandomPos = new Vector2(
                Random.Range(-size.x * 0.5f, size.x * 0.5f),
                Random.Range(-size.y * 0.5f, size.y * 0.5f)
            );

            // Offset 더하고, 월드 위치로 변환
            Vector2 worldPos = (Vector2)spawnArea.transform.position + offset + localRandomPos;

            return worldPos;
        }


        // Others
        IEnumerator BossSliderFadeOut(Image ctrl)
        {
            if (ctrl == null) yield break;

            changeSuccess = false;

            Color from = ctrl.color;
            Color to = new Color(0, 0, 0, 1);
            float duration = 1.5f;

            float t = 0f;
            while (t < 1f)
            {
                t += Time.deltaTime / duration;
                ctrl.color = Color.Lerp(from, to, t);
                yield return null;
            }

            ctrl.color = to;

            // 슬라이더 비활성화는 페이드 아웃 후 한 번만 처리
            if (ctrl.transform == bossSlider.transform.Find("HealthBackground"))
            {
                bossSlider.gameObject.SetActive(false);
            }
        }

        //IEnumerator BossTimerSliderFadeOut(Image ctrl)
        //{
        //    if (ctrl == null) yield break;
        //    changeSuccess = false;
        //    var ctrlR = ctrl.color.r;
        //    var ctrlG = ctrl.color.g;
        //    var ctrlB = ctrl.color.b;
        //    var ctrlA = ctrl.color.a;
        //    Color from = new Color(ctrlR, ctrlG, ctrlB, ctrlA); // 밝은 상태 (흰색)
        //    Color to = new Color(0, 0, 0, 1);   // 검정색
        //    float duration = 1.5f;

        //    // 1. 페이드 아웃
        //    float t = 0f;
        //    while (t < 1f)
        //    {
        //        t += Time.deltaTime / duration;
        //        ctrl.color = Color.Lerp(from, to, t);
        //        yield return null;
        //    }
        //}

        IEnumerator WaveSliderFade(Image ctrl)
        {
            if (ctrl == null || !waveSliderOriginalColors.ContainsKey(ctrl)) yield break;

            changeSuccess = false;

            waveSlider.gameObject.SetActive(true);

            Color from = new Color(0f, 0f, 0f, 1f); // 검정에서 시작
            Color to = waveSliderOriginalColors[ctrl]; // 원래 색상으로 복원
            ctrl.color = from;

            float duration = 1.5f;
            float t = 0f;

            while (t < 1f)
            {
                t += Time.deltaTime / duration;
                ctrl.color = Color.Lerp(from, to, t);
                yield return null;
            }

            ctrl.color = to;
        }

        IEnumerator ChangeBackgroundWithFade(SpriteRenderer ctrl, int backGroundIndex)
        {
            if (ctrl == null) yield break;
            changeSuccess = false;
            Color from = new Color(1, 1, 1, 1); // 밝은 상태 (흰색)
            Color to = new Color(0, 0, 0, 1);   // 검정색
            float duration = 1.5f;
            OnFadeOutSlider();
            // 1. 페이드 아웃
            float t = 0f;
            while (t < 1f)
            {
                t += Time.deltaTime / duration;
                ctrl.color = Color.Lerp(from, to, t);
                yield return null;
            }


            // 2. 배경 이미지 변경
            if (ctrl.gameObject.name == "BackGround")
            {
                ctrl.sprite = TestBackGround[backGroundIndex];
            }
            else if (ctrl.gameObject.name == "MidGround")
            {
                ctrl.sprite = TestBackGroundMid[backGroundIndex];
            }
            else if (ctrl.gameObject.name == "ForeGround")
            {
                ctrl.sprite = TestBackGroundFront[backGroundIndex];
            }

            OnFadeInSlider();
            // 3. 페이드 인
            t = 0f;
            while (t < 1f)
            {
                t += Time.deltaTime / duration;
                ctrl.color = Color.Lerp(to, from, t);
                yield return null;
            }

            ctrl.color = from;
            changeSuccess = true;
            isSuccessChangeBackGround = true;
        }

        private void CacheWaveSliderColors()
        {
            waveSliderOriginalColors.Clear();
            foreach (Image image in waveSlider.GetComponentsInChildren<Image>())
            {
                string name = image.gameObject.name;
                if (name == "Background" || name == "Fill" || name == "Handle")
                {
                    waveSliderOriginalColors[image] = image.color;
                }
            }
        }

        private void CacheBossSliderColors()
        {
            bossSliderOriginalColors.Clear();
            foreach (Image image in bossSlider.GetComponentsInChildren<Image>())
            {
                string name = image.gameObject.name;
                if (name == "HealthBackground" || name == "HealthFill" ||
                    name == "TimerBackground" || name == "TimerFill")
                {
                    bossSliderOriginalColors[image] = image.color;
                }
            }
        }

        private void RestoreBossSliderColors()
        {
            foreach (var kv in bossSliderOriginalColors)
            {
                if (kv.Key != null)
                {
                    kv.Key.color = kv.Value;
                }
            }
        }

        // TODO: LJH
        private void UpdateBossSliders()
        {
            if (!bossSlider.gameObject.activeSelf || isBossCleared)
                return;
            bossTimer -= Time.deltaTime;
            if(bossTimer < 0)
            {
                bossTimer = 0;
                OnTestWaveFailedActive();
            }
            bossTimerSlider.value = bossTimer / c_InitialBossTimer;

            var bossGo = currentEnemy.Last();
            if (bossGo == null)
            {
                bossSlider.value = 0;
                return;
            }
            
            var bossBT = bossGo.GetComponent<NewBossControllerBT>();
            if (bossBT == null)
                return;

            var sb = new StringBuilder();
            float hpPercentage = BigNum.GetPercentage(bossBT.HP, bossBT.MaxHP);
            bossSlider.value = hpPercentage;
        }
        // ~TODO

    } // Scope by class TestWaveController

} // namespace Root