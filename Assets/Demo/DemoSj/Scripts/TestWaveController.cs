using SkyDragonHunter.Game;
using SkyDragonHunter.Gameplay;
using SkyDragonHunter.Managers;
using SkyDragonHunter.Test;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
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
        public Slider waveSlider;
        public TextMeshProUGUI waveLevelText;
        public GameObject clearPanel;
        public GameObject feildPanel;
        public GameObject backGround;
        public BackGroundController backGroundController;
        public GameObject airship;
        public GameObject monster;
        public GameObject boss;
        public BoxCollider2D spawnArea;
        public float maxWaveTime;
        private List<GameObject> currentEnemy;
        private float oldAllSpeed;
        private float currentWaveTime;
        private float inpiniteModSpawnDelay = 3;
        private float distance;
        private bool isStopped;
        private bool isCanSpawn;

        // 테스트용
        public Sprite[] TestBackGround;
        public Sprite[] TestBackGroundMid;
        public Sprite[] TestBackGroundFront;
        public int spawnableMonsters = 10;
        public bool isInfiniteMode { get; private set; } = false;
        public bool isRewardSet { get; private set; } = false;
        private int backGroundIndex;
        private int currentZonelLevel = 1;
        private int currentMissionLevel = 1;
        private int lastTriedZonelLevel = 1;
        private int lastTriedMissionLevel = 1;
        private int currentSpawnMonsters = 0;
        private float currentOpenPanel = 0f;
        private StageInfo stageInfo;

        private bool changeSuccess = true;
        // 테스트용
        // Test drop


        // Test drop
        // 속성 (Properties)
        // 외부 종속성 필드 (External dependencies field)
        // 이벤트 (Events)
        // 유니티 (MonoBehaviour 기본 메서드)
        private void Awake()
        {
            GameMgr.RegisterObject(gameObject);
        }

        private void Start()
        {
            maxWaveTime = 10f;
            currentWaveTime = 0f;
            waveSlider.maxValue = maxWaveTime;
            waveSlider.minValue = 0;
            backGroundController = backGround.GetComponent<BackGroundController>();
            oldAllSpeed = backGround.GetComponent<BackGroundController>().scrollSpeed;
            isCanSpawn = true;
            isStopped = false;
            currentEnemy = new List<GameObject>();
            currentMissionLevel = 1;
            currentZonelLevel = 1;
            OnSaveLastClearWave();
            OnTestWaveFailedUnActive();
            SpwanAreaPositionSet();
        }

        private void Update()
        {
            //backGroundController.SetScrollSpeed(1f);
            OnBackGroundStop();
            if (!isInfiniteMode)
            {
                NormalWaveUpdate();
            }
            else
            {
                infiniteWaveUpdate();
            }
        }

        // Public 메서드
        public void OnSelectWave(int misson, int zone)
        {
            currentMissionLevel = misson;
            currentZonelLevel = zone;
            OnGoSelectCurrentWave();
        }

        public void OnOffInfiniteMod()
        {
            if (isInfiniteMode)
            {
                isInfiniteMode = false;
                OnSetLastTriedtWave();
            }
            else
            {
                isInfiniteMode = true;
                OnSetCurrentWave();
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
            if (!isStopped && changeSuccess)
            {
                currentWaveTime += Time.deltaTime * oldAllSpeed;
                waveSlider.value = currentWaveTime;
            }

            if (currentWaveTime > 2f && currentWaveTime < 4f && !isCanSpawn)
            {
                isCanSpawn = true;
            }
            else if (currentWaveTime > 7f && currentWaveTime < 8f && !isCanSpawn)
            {
                isCanSpawn = true;
            }

            if (currentWaveTime >= 1f && currentWaveTime <= 2f && isCanSpawn
                || currentWaveTime >= 6f && currentWaveTime <= 7f && isCanSpawn)
            {
                OnSpwanMonster();
            }

            if (currentWaveTime >= 10f && isCanSpawn)
            {
                OnSpwanBoss();
            }

            if (currentWaveTime >= 10f && (currentEnemy == null || currentEnemy.All(e => e == null || e.Equals(null))))
            {
                OnActiveClearPanel(); // 클리어 패널 활성화
                OnSaveLastClearWave();
                currentOpenPanel += Time.deltaTime;
                if (currentOpenPanel > 2f)
                {
                    OnUnActiveClearPanel(); // 클리어 패널 종료
                    currentZonelLevel++;
                    if (currentZonelLevel > 10)
                    {
                        currentZonelLevel = 1;
                        currentMissionLevel++;
                    }

                    waveLevelText.text = string.Format("{0} - {1}", currentMissionLevel, currentZonelLevel);

                    currentWaveTime = 0f;
                    isCanSpawn = true;
                    currentOpenPanel = 0f;
                    lastTriedMissionLevel = currentMissionLevel;
                    lastTriedZonelLevel = currentZonelLevel;
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
                OnSpwanMonster();
            }
        }

        private void OnSetLastTriedtWave()
        {
            OnClearMonster();
            isCanSpawn = true;
            currentWaveTime = 0f;
            currentMissionLevel = lastTriedMissionLevel;
            currentZonelLevel = lastTriedZonelLevel;
            waveLevelText.text = string.Format("{0} - {1}", currentMissionLevel, currentZonelLevel);
            OnChangeBackGround(currentZonelLevel - 1);
        }

        private void OnSetCurrentWave()
        {
            OnClearMonster();
            isCanSpawn = true;
            currentWaveTime = 0f;
            currentMissionLevel = stageInfo.missionLevel;
            currentZonelLevel = stageInfo.zoneLevel;
            waveLevelText.text = string.Format("{0} - {1}", currentMissionLevel, currentZonelLevel);
            OnChangeBackGround(currentZonelLevel - 1);
        }

        private void OnGoSelectCurrentWave()
        {
            waveLevelText.text = string.Format("{0} - {1}", currentMissionLevel, currentZonelLevel);
            OnChangeBackGround(currentZonelLevel - 1);
        }

        private void OnSaveLastClearWave()
        {
            stageInfo.missionLevel = currentMissionLevel;
            stageInfo.zoneLevel = currentZonelLevel;
        }

        private void OnBackGroundStop()
        {
            bool shouldStop = false;

            foreach (var monster in currentEnemy)
            {
                if (monster == null) continue;

                if (!monster.CompareTag("Monster")) continue; // 몬스터 태그 필터링

                float distance = Vector3.Distance(airship.transform.position, monster.transform.position);
                if (distance <= 50f)
                {
                    shouldStop = true;
                    break;
                }
            }

            if (shouldStop && !isStopped)
            {
                isStopped = true;
                backGroundController.SetScrollSpeed(0f);
            }
            else if (!shouldStop && isStopped)
            {
                if (currentEnemy == null || currentEnemy.All(e => e == null || e.Equals(null)))
                {
                    isStopped = false;
                    backGroundController.SetScrollSpeed(oldAllSpeed);
                }
            }
        }

        private void OnSpwanMonster()
        {
            for (int i = 0; i < spawnableMonsters; i++)
            {
                GameObject spawned = Instantiate(monster, GetRandomSpawnAreaInPosition(), Quaternion.identity);
                spawned.name = $"Monster({instanceNo++})";
                currentEnemy.Add(spawned);
                currentSpawnMonsters++;
            }

            isCanSpawn = false;
        }

        private void OnSpwanBoss()
        {
            GameObject spawned = Instantiate(boss, GetRandomSpawnAreaInPosition(), Quaternion.identity);
            currentEnemy.Add(spawned);
            currentSpawnMonsters++;
            isCanSpawn = false;
        }

        private void OnActiveClearPanel()
        {
            clearPanel.SetActive(true);
            isRewardSet = true;
        }

        private void OnUnActiveClearPanel()
        {
            clearPanel.SetActive(false);
            OnChangeBackGround(currentZonelLevel);
            isRewardSet = false;
            ItemMgr.Reset();
        }

        private void OnChangeBackGround(int bgIndex)
        {
            backGroundIndex = bgIndex;
            if (backGroundIndex > 9)
            {
                backGroundIndex = 9;
            }

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
        IEnumerator ChangeBackgroundWithFade(SpriteRenderer ctrl, int backGroundIndex)
        {
            if (ctrl == null) yield break;
            changeSuccess = false;

            Color from = new Color(1, 1, 1, 1); // 밝은 상태 (흰색)
            Color to = new Color(0, 0, 0, 1);   // 검정색
            float duration = 1.5f;

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
        }


    } // Scope by class TestWaveController

} // namespace Root