using SkyDragonHunter.Managers;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace SkyDragonHunter
{
    public class ObjectUUID : MonoBehaviour
    {
        // 필드 (Fields)
        [Tooltip("Reset을 하면 자동 할당, 임의 설정시 충돌 위험 있음")]
        public string uuid;

        // 속성 (Properties)
        // 외부 종속성 필드 (External dependencies field)
        // 이벤트 (Events)
        // 유니티 (MonoBehaviour 기본 메서드)
        private void Awake()
        {
#if UNITY_EDITOR
            if (string.IsNullOrEmpty(uuid))
            {
                this.ResetUUID();
                EditorUtility.SetDirty(this);
            }
#endif
        }

        // Public 메서드
        public void ResetUUID()
        {
            if (uuid != null && uuid.Length > 0)
            {
                GameMgr.UnregisterObject(gameObject);
            }
            uuid = Guid.NewGuid().ToString();
            GameMgr.RegisterObject(gameObject);
        }

        // Private 메서드
        // Others

    } // Scope by class ObjectUUID
} // namespace Root
