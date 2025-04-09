using SkyDragonHunter.Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SkyDragonHunter.Gameplay {

    public class BallOnDestructible : MonoBehaviour
        , IBallLifecycleHandler
    {
        // 필드 (Fields)
        [SerializeField] private float m_LifeTime = 10f;

        public void OnEnd(GameObject caster) { }
        public void OnHitAfter(GameObject caster) { }
        public void OnHitBefore(GameObject caster) { }
        public void OnHitEnter(GameObject defender) { }
        public void OnHitExit(GameObject defender) { }
        public void OnHitStay(GameObject defender) { }
        public void OnStart(GameObject caster)
        {
            Destroy(gameObject, m_LifeTime);
        }

        // 속성 (Properties)
        // 외부 종속성 필드 (External dependencies field)
        // 이벤트 (Events)
        // 유니티 (MonoBehaviour 기본 메서드)
        // Public 메서드
        // Private 메서드
        // Others
    
    } // Scope by class BallOnDestructible
} // namespace SkyDragonHunter.Gameplay