using SkyDragonHunter.Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SkyDragonHunter.Gameplay {

    public class BallMovementLinear : MonoBehaviour
        , IDirectional
    {
        // 필드 (Fields)
        public float speed = 1f;

        // 속성 (Properties)
        public Vector3 Direction { get; private set; } = Vector3.right;

        // 외부 종속성 필드 (External dependencies field)
        private BallBase m_BallBase;

        // 이벤트 (Events)
        // 유니티 (MonoBehaviour 기본 메서드)
        private void Awake()
        {
            Init();
        }

        private void Update()
        {
            Move();
        }

        // Public 메서드
        public void SetDirection(Vector2 newDirection)
        {
            Direction = newDirection.normalized;
        }

        // Private 메서드
        private void Init()
        {
            m_BallBase = GetComponent<BallBase>();
        }

        private void Move()
        {
            transform.position += Direction * speed * Time.deltaTime;
        }

        // Others

    } // Scope by class BallMovementLinear
} // namespace SkyDragonHunter.Gameplay