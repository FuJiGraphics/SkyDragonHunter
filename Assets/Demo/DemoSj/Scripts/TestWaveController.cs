using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SkyDragonHunter
{

    public class TestWaveController : MonoBehaviour
    {
        // 필드 (Fields)
        public Slider slider;
        public GameObject airship;
        public GameObject monster;
        public GameObject boss;
        public GameObject currentEnemy;
        public float maxWaveTime;
        private Vector3 spwanPosition;
        private float currentWaveTime;
        private float distance;
        private bool isStopped;
        private bool canSpwan;

        // 테스트용
        private TestMonsterMove testMonsterMove;
        // 테스트용

        // 속성 (Properties)
        // 외부 종속성 필드 (External dependencies field)
        // 이벤트 (Events)
        // 유니티 (MonoBehaviour 기본 메서드)
        private void Start()
        {
            maxWaveTime = 10f;
            currentWaveTime = 0f;
            slider.maxValue = maxWaveTime;
            slider.minValue = 0;
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
                    testMonsterMove.OnStoppedMonster(true);
                }
            }
            else
            {
                isStopped = false;
            }

            if (!isStopped)
            {
                currentWaveTime += Time.deltaTime;
                slider.value = currentWaveTime;
            }

            if (currentWaveTime > 4f && currentWaveTime < 6f && !canSpwan)
            {
                canSpwan = true;
            }
            else if (currentWaveTime > 7f && currentWaveTime < 9f && !canSpwan)
            {
                canSpwan = true;
            }

            if (currentWaveTime >= 3f && currentWaveTime <= 4f && canSpwan
                || currentWaveTime >= 6f && currentWaveTime <= 7f && canSpwan)
            {
                OnSpwanMonster();
            }

            if (currentWaveTime >= 10f && canSpwan)
            {
                OnSpwanBoss();
            }

        }

        // Public 메서드
        // Private 메서드
        private void OnSpwanMonster()
        {
            currentEnemy = Instantiate(monster, spwanPosition, Quaternion.identity);
            testMonsterMove = currentEnemy.GetComponent<TestMonsterMove>();
            canSpwan = false;
        }

        private void OnSpwanBoss()
        {
            currentEnemy = Instantiate(boss, spwanPosition, Quaternion.identity);
            testMonsterMove = currentEnemy.GetComponent<TestMonsterMove>();
            canSpwan = false;
        }
        // Others

    } // Scope by class TestWaveController

} // namespace Root