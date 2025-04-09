using SkyDragonHunter.Gameplay;
using SkyDragonHunter.Interfaces;
using SkyDragonHunter.Scriptables;
using SkyDragonHunter.Structs;
using UnityEngine;

namespace SkyDragonHunter.Gameplay {

    public class BallOnHitDamage : MonoBehaviour
        , IBallLifecycleHandler
    {
        // 필드 (Fields)
        // 속성 (Properties)
        // 외부 종속성 필드 (External dependencies field)
        private BallBase m_BallBase;
        private CanonDefinition m_CanonData;
        private bool isOnHit = false;

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
            if (isOnHit)
                return;
            isOnHit = true;

            CharacterStatus aStat = m_BallBase.Caster.GetComponent<CharacterStatus>();
            CharacterStatus bStat = defender.GetComponent<CharacterStatus>();
            if (aStat == null || bStat == null)
                return;
            if (m_BallBase.CanonData == null)
            {
                Debug.LogWarning("[BallOnHitDamage]: m_CanonData가 null입니다.");
                return;
            }

            Attack attack = m_BallBase.CanonData.CreateAttack(aStat, bStat);
            if (bStat.TryGetComponent<IAttackable>(out var attackable))
            {
                attackable.OnAttack(m_BallBase.Caster, attack);
            }
        }

        public void OnHitExit(GameObject defender) {}
        public void OnHitStay(GameObject defender) {}
        public void OnHitBefore(GameObject caster) {}
        public void OnHitAfter(GameObject caster) {}

        // Private 메서드
        private void Init()
        {
            m_BallBase = GetComponent<BallBase>();
            isOnHit = false;
        }

        // Others

    } // Scope by class BallOnHitDamage
} // namespace SkyDragonHunter