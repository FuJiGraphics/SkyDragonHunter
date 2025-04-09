using SkyDragonHunter.Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SkyDragonHunter {
    public class BallOnHitDestroyImmediatly : MonoBehaviour
        , ISkillLifecycleHandler
    {
        // 필드 (Fields)
        // 속성 (Properties)
        // 외부 종속성 필드 (External dependencies field)
        // 이벤트 (Events)
        // 유니티 (MonoBehaviour 기본 메서드)
        // Public 메서드
        // Private 메서드
        // Others
        public void OnSkillCast(GameObject caster) { }
        public void OnSkillEnd(GameObject caster) { }

        public void OnSkillHitAfter(GameObject caster)
        {
            Destroy(gameObject);
        }

        public void OnSkillHitBefore(GameObject caster) { }
        public void OnSkillHitEnter(GameObject defender) { }
        public void OnSkillHitExit(GameObject defender) { }
        public void OnSkillHitStay(GameObject defender) { }

    } // Scope by class SkillOnHitDestroyImmediatly
} // namespace SkyDragonHunter