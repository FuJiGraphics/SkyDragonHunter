using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SkyDragonHunter
{

    public class TestMonsterMove : MonoBehaviour
    {
        // 필드 (Fields)
        public float moveSpeed;
        private float distroyTime;
        private float currentDistroyTime;
        private bool isStopped;
        // 속성 (Properties)
        // 외부 종속성 필드 (External dependencies field)
        // 이벤트 (Events)
        // 유니티 (MonoBehaviour 기본 메서드)

        private void Start()
        {
            distroyTime = 1f;
            currentDistroyTime = 0;
        }

        private void Update()
        {
            if (!isStopped)
            {
                gameObject.transform.position += Vector3.left * moveSpeed * Time.deltaTime;
            }
            else
            {
                currentDistroyTime += Time.deltaTime;
            }

            if (currentDistroyTime > distroyTime)
            {
                Destroy(gameObject);
            }
        }

        public void OnStoppedMonster(bool stop)
        {
            isStopped = stop;
        }

        // Public 메서드
        // Private 메서드
        // Others

    } // Scope by class TestMonsterMove

} // namespace Root