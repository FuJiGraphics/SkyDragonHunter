using NPOI.POIFS.Properties;
using SkyDragonHunter.Game;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SkyDragonHunter
{

    public class TestWaveController : MonoBehaviour
    {
        // 필드 (Fields)
        public Slider waveSlider;
        public TextMeshProUGUI waveLevelText;
        public GameObject ClearPanel;
        public GameObject backGround;
        public BackGroundController backGroundController;
        public GameObject airship;
        public GameObject monster;
        public GameObject boss;
        public GameObject currentEnemy;
        public float maxWaveTime;
        private Vector3 spwanPosition;
        private float oldAllSpeed;
        private float currentWaveTime;
        private float distance;
        private bool isStopped;
        private bool canSpwan;

        // 테스트용
        private int smallLevel = 1;
        private int mainLevel = 1;
        private float currentOpenPanel = 0f;
        // 테스트용

        // 속성 (Properties)
        // 외부 종속성 필드 (External dependencies field)
        // 이벤트 (Events)
        // 유니티 (MonoBehaviour 기본 메서드)
        private void Start()
        {
            maxWaveTime = 10f;
            currentWaveTime = 0f;
            waveSlider.maxValue = maxWaveTime;
            waveSlider.minValue = 0;
            ClearPanel.SetActive(false);
            backGroundController = backGround.GetComponent<BackGroundController>();
            oldAllSpeed = backGround.GetComponent<BackGroundController>().scrollSpeed;
            spwanPosition = new Vector3(airship.transform.position.x + 15,
                airship.transform.position.y, airship.transform.position.z);
            canSpwan = true;
            currentEnemy = null;
        }

        private void Update()
        {
            if (currentEnemy != null)
            {
                if (Vector3.Distance(airship.transform.position, currentEnemy.transform.position) <= 8f)
                {
                    isStopped = true;
                    backGroundController.SetScrollSpeed(0f);
                }
            }
            else
            {
                isStopped = false;
                backGroundController.SetScrollSpeed(oldAllSpeed);
            }

            if (!isStopped)
            {
                currentWaveTime += Time.deltaTime * oldAllSpeed;
                waveSlider.value = currentWaveTime;
            }

            if (currentWaveTime > 4f && currentWaveTime < 6f && !canSpwan)
            {
                canSpwan = true;
            }
            else if (currentWaveTime > 7f && currentWaveTime < 9f && !canSpwan)
            {
                canSpwan = true;
            }

            if (currentWaveTime >= 2f && currentWaveTime <= 4f && canSpwan
                || currentWaveTime >= 6f && currentWaveTime <= 7f && canSpwan)
            {
                OnSpwanMonster();
            }

            if (currentWaveTime >= 10f && canSpwan)
            {
                OnSpwanBoss();
            }

            if (currentWaveTime >= 10f && currentEnemy == null)
            {
                OnActiveClearPanel();
                currentOpenPanel += Time.deltaTime;

                if (currentOpenPanel > 2f)
                {
                    OnUnActiveClearPanel();
                    smallLevel++;
                    if (smallLevel > 10)
                    {
                        smallLevel = 0;
                        mainLevel++;
                    }

                    waveLevelText.text = string.Format("{0} - {1}", mainLevel, smallLevel);

                    currentWaveTime = 0f;
                    canSpwan = true;
                    currentOpenPanel = 0f;
                    
                }


            }

        }

        // Public 메서드
        public void ReStartAll()
        {
            ClearPanel.SetActive(false);
            smallLevel = 1;
            mainLevel = 1;
            currentWaveTime = 0f;
        }

        // Private 메서드
        private void OnSpwanMonster()
        {
            currentEnemy = Instantiate(monster, spwanPosition, Quaternion.identity);
            canSpwan = false;
        }

        private void OnSpwanBoss()
        {
            currentEnemy = Instantiate(boss, spwanPosition, Quaternion.identity);
            canSpwan = false;
        }

        private void OnActiveClearPanel()
        {
            ClearPanel.SetActive(true);
        }

        private void OnUnActiveClearPanel()
        {
            ClearPanel.SetActive(false);
            OnChangeBackGround();
        }

        private void OnChangeBackGround()
        {
            //foreach (Transform child in backGround.transform)
            //{
            //    SpriteRenderer ctrl = child.GetComponent<SpriteRenderer>();
            //    if (ctrl != null)
            //    {
            //        ctrl.color = new Color(
            //            Random.Range(0f, 1f),
            //            Random.Range(0f, 1f),
            //            Random.Range(0f, 1f),
            //            1f // 알파값은 1로 설정 (완전히 불투명)
            //        );
            //    }
            //}
        }
        // Others

    } // Scope by class TestWaveController

} // namespace Root