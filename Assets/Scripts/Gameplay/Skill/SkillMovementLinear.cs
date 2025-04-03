using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SkyDragonHunter {
    public class SkillMovementLinear : MonoBehaviour
    {
        // 필드 (Fields)
        public float speed = 1f;

        // 속성 (Properties)
        public Vector3 Direction { get; private set; } = Vector3.right;

        // 외부 종속성 필드 (External dependencies field)
        // 이벤트 (Events)
        // 유니티 (MonoBehaviour 기본 메서드)
        private void Update()
        {
            transform.position += Direction * speed * Time.deltaTime;
        }

        // Public 메서드
        public void SetDirection(Vector3 newDirection)
        {
            Direction = newDirection.normalized;
        }

        // Private 메서드
        // Others

    } // Scope by class SkillMovementLinear
} // namespace SkyDragonHunter