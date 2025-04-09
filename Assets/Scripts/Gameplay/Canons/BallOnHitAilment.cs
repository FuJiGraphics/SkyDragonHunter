using SkyDragonHunter.Interfaces;
using SkyDragonHunter.Scriptables;
using UnityEngine;

namespace SkyDragonHunter.Gameplay {

    public class BallOnHitAilment : MonoBehaviour
        , IBallLifecycleHandler
    {
        // 필드 (Fields)
        public StatusAilmentDefinition[] ailments;

        private BallBase m_BallBase;

        // 속성 (Properties)
        // 외부 종속성 필드 (External dependencies field)
        // 이벤트 (Events)
        // 유니티 (MonoBehaviour 기본 메서드)
        // Public 메서드
        public void OnStart(GameObject caster) 
        {
            Init();
        }

        public void OnEnd(GameObject caster) {}

        public void OnHitEnter(GameObject defender)
        {
            ApplyStatusAilment(defender);
        }

        public void OnHitExit(GameObject defender) {}
        public void OnHitStay(GameObject defender) { }
        public void OnHitBefore(GameObject caster) { }
        public void OnHitAfter(GameObject caster) { }

        // Private 메서드
        private void Init()
        {
            m_BallBase = GetComponent<BallBase>();
        }

        private void ApplyStatusAilment(GameObject target)
        {
            foreach (var ailment in ailments)
            {
                ailment.Execute(m_BallBase.Caster, target);
            }
        }

        // Others

    } // Scope by class BallOnHitAilment
} // namespace SkyDragonHunter.Gameplay