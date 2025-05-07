using SkyDragonHunter.Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SkyDragonHunter.Gameplay {

    public class SkillBuffApplier : MonoBehaviour
        , ISkillLifecycleHandler
    {
        // 필드 (Fields)
        private SkillBase m_SkillBase;

        // 속성 (Properties)
        // 외부 종속성 필드 (External dependencies field)
        // 이벤트 (Events)
        // 유니티 (MonoBehaviour 기본 메서드)
        private void Awake()
        {
            m_SkillBase = GetComponent<SkillBase>();
        }

        // Public 메서드
        public void OnSkillCast(GameObject caster)
        {
            if (caster.TryGetComponent<BuffExecutor>(out var executor))
            {
                foreach (var buff in m_SkillBase.SkillData.BuffData)
                {
                    executor.Execute(buff);
                }
            }
            else
            {
                Debug.LogError($"[SkillBuffApplier]: 버프 시전에 실패하였습니다. Caster: {caster}");
                Debug.LogError("[SkillBuffApplier]: BuffExecutor 컴포넌트를 찾을 수 없습니다.");
            }
        }

        public void OnSkillEnd(GameObject caster) { }
        public void OnSkillHitAfter(GameObject caster) { }
        public void OnSkillHitBefore(GameObject caster) { }
        public void OnSkillHitEnter(GameObject defender) { }
        public void OnSkillHitExit(GameObject defender) { }
        public void OnSkillHitStay(GameObject defender) { }

        // Private 메서드
        // Others

    } // Scope by class SkillBuffApplier
} // namespace SkyDragonHunter