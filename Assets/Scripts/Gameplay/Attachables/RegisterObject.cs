using SkyDragonHunter.Managers;
using SkyDragonHunter.Structs;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.Events;

namespace SkyDragonHunter.Gameplay {

    public class RegisterObject : MonoBehaviour
    {
        // 필드 (Fields)
        public string id;
        public UnityEvent onInitEvents;
        public bool bindRumtime = false;

        // 속성 (Properties)
        // 외부 종속성 필드 (External dependencies field)
        // 이벤트 (Events)
        // 유니티 (MonoBehaviour 기본 메서드)
        private void Awake()
        {
            if (bindRumtime)
            {
                GameMgr.RegisterObject(id, gameObject);
            }
        }

        private void Start()
        {
            if (bindRumtime)
            {
                onInitEvents?.Invoke();
            }
        }

        // Public 메서드
        // Private 메서드
        // Others

    } // Scope by class NewScript
} // namespace SkyDragonHunter