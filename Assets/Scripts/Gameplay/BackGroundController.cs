using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SkyDragonHunter.Game {

    public class BackGroundController : MonoBehaviour
    {
        // 필드 (Fields)
        public float scrollSpeed = 1f;
        public FloatingEffect[] floatingObjects;

        private BackGroundScrolling[] m_Backgrounds;

        // 속성 (Properties)
        public bool IsStopped { get => scrollSpeed.Equals(0f); }

        // 외부 종속성 필드 (External dependencies field)
        // 이벤트 (Events)
        // 유니티 (MonoBehaviour 기본 메서드)
        private void Awake()
        {
            m_Backgrounds = GetComponentsInChildren<BackGroundScrolling>();
        }

        private void Update()
        {
            UpdateScrollSpeed();
            UpdateFloatingSpeed();
        }

        // Public 메서드
        public void SetScrollSpeed(float speed)
        {
            scrollSpeed = speed;
        }

        // Private 메서드
        private void UpdateScrollSpeed()
        {
            foreach (var back in m_Backgrounds)
            {
                back.scrollSpeed = scrollSpeed;
            }
        }

        private void UpdateFloatingSpeed()
        {
            foreach (var obj in floatingObjects)
            {
                if (scrollSpeed.Equals(0f))
                    obj.floatSpeedOffset = 0f;
                else if (scrollSpeed <= float.MaxValue && scrollSpeed >= 0f)
                    obj.floatSpeedOffset = Mathf.Clamp(scrollSpeed, 1f, float.MaxValue);
                else if (scrollSpeed <= 0f && scrollSpeed >= -1f * float.MaxValue)
                    obj.floatSpeedOffset = Mathf.Clamp(scrollSpeed * -1f, 1f, float.MaxValue);
            }
        }
        // Others

    } // Scope by class BackGroundManaged
}
