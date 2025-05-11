using SkyDragonHunter.Entities;
using SkyDragonHunter.Interfaces;
using SkyDragonHunter.Managers;
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
            CastToCaster(caster);
            CastToAllCrews();
        }

        public void OnSkillEnd(GameObject caster) { }
        public void OnSkillHitAfter(GameObject caster) { }
        public void OnSkillHitBefore(GameObject caster) { }
        public void OnSkillHitEnter(GameObject defender) 
        {
            CastToReceiver(defender);
        }
        public void OnSkillHitExit(GameObject defender) { }
        public void OnSkillHitStay(GameObject defender) { }

        // Private 메서드
        private void CastToCaster(GameObject caster)
        {
            if (m_SkillBase.SkillData.buffTarget != Scriptables.BuffTarget.Caster)
                return;

            if (caster.TryGetComponent<BuffExecutor>(out var executor))
            {
                foreach (var buff in m_SkillBase.SkillData.BuffData)
                {
                    executor.Execute(buff);
                }
            }
            else
            {
                Debug.LogError($"[CastToCaster]: 버프 시전에 실패하였습니다. Caster: {caster}");
                Debug.LogError("[CastToCaster]: BuffExecutor 컴포넌트를 찾을 수 없습니다.");
            }
        }

        private void CastToAllCrews()
        {
            if (m_SkillBase.SkillData.buffTarget != Scriptables.BuffTarget.All)
                return;

            var crewInstancies = GameMgr.FindObjects("Crew");

            foreach (var crew in crewInstancies)
            {
                if (crew == null)
                    continue;

                if (crew.TryGetComponent<BuffExecutor>(out var executor))
                {
                    if (!crew.activeSelf)
                    {
                        executor.StopAll();
                    }
                    else
                    {
                        foreach (var buff in m_SkillBase.SkillData.BuffData)
                        {
                            executor.Execute(buff);
                        }
                    }
                }
                else
                {
                    Debug.LogError($"[CastToAllCrews]: 버프 시전에 실패하였습니다. Target: {crew}");
                    Debug.LogError("[CastToAllCrews]: BuffExecutor 컴포넌트를 찾을 수 없습니다.");
                }
            }
        }

        private void CastToReceiver(GameObject receiver)
        {
            if (m_SkillBase.SkillData.buffTarget != Scriptables.BuffTarget.Receiver)
                return;

            if (receiver.TryGetComponent<BuffExecutor>(out var executor))
            {
                foreach (var buff in m_SkillBase.SkillData.BuffData)
                {
                    executor.Execute(buff);
                }
            }
            else
            {
                Debug.LogError("[CastToCaster]: BuffExecutor 컴포넌트를 찾을 수 없습니다.");
            }
        }

        // Others

    } // Scope by class SkillBuffApplier
} // namespace SkyDragonHunter