using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SkyDragonHunter.Utility {

    public class Billboard : MonoBehaviour
    {
        // 필드 (Fields)
        public Transform targetCamera;

        // 속성 (Properties)
        // 외부 종속성 필드 (External dependencies field)
        // 이벤트 (Events)
        // 유니티 (MonoBehaviour 기본 메서드)
        private void Start()
        {
            if (targetCamera == null)
                targetCamera = Camera.main.transform;
        }

        private void Update()
        {
            transform.rotation = targetCamera.rotation;
        }

        private void Reset()
        {
            targetCamera = Camera.main.transform;
        }

        // Public 메서드
        // Private 메서드
        // Others

    } // Scope by class Billboard
} // namespace SkyDragonHunter.Utility
