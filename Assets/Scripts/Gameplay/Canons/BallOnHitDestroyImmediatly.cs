using SkyDragonHunter.Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SkyDragonHunter.Gameplay {

    public class BallOnHitDestroyImmediatly : MonoBehaviour
        , IBallEffectLifecycleHandler
    {
        // 필드 (Fields)
        // 속성 (Properties)
        // 외부 종속성 필드 (External dependencies field)
        // 이벤트 (Events)
        // 유니티 (MonoBehaviour 기본 메서드)
        // Public 메서드
        public void OnCastEffect(GameObject attacker)
        {
        }

        public void OnHitEnterEffect(GameObject attacker, GameObject defender)
        {
            Destroy(gameObject);
        }

        public void OnHitStayEffect(GameObject attacker, GameObject defender)
        {
            // Empty
        }

        // Private 메서드
        // Others
    } // Scope by class BallOnHitDestroyImmediatly
} // namespace SkyDragonHunter