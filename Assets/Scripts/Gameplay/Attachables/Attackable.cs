using SkyDragonHunter.Interfaces;
using SkyDragonHunter.Managers;
using SkyDragonHunter.Structs;
using UnityEngine;

namespace SkyDragonHunter.Gameplay {

    public class Attackable : MonoBehaviour
        , IAttackable
    {
        // 필드 (Fields)
        // 속성 (Properties)
        // 외부 종속성 필드 (External dependencies field)
        private DamageReceiver m_DamageReceiver;

        // 이벤트 (Events)
        // 유니티 (MonoBehaviour 기본 메서드)
        // Public 메서드
        // Private 메서드
        private void Awake()
        {
            m_DamageReceiver = GetComponent<DamageReceiver>();
        }
        // Others
        public void OnAttack(GameObject attacker, Attack attack)
        {
            //Debug.Log($"attacker: {attacker} -> defender: {gameObject}");
            if (TryGetComponent<RepairExecutor>(out var repairExecutor))
            {
                if (repairExecutor.IsActiveDivineShield)
                {
                    repairExecutor.IsActiveDivineShield = false;
                    DrawableMgr.TopText(transform.position, "Immue!" + attack.damage.ToUnit(), Color.black);

                    return;
                }
            }

            if (attack.isCritical)
            {
                DrawableMgr.TopText(transform.position, "Critical!", Color.red);
                DrawableMgr.Text(transform.position, attack.damage.ToUnit(), Color.red);
            }
            else
            {
                DrawableMgr.Text(transform.position, attack.damage.ToUnit());
            }

            m_DamageReceiver.TakeDamage(attacker, attack.damage);
        }

    } // Scope by class Attackable
} // namespace SkyDragonHunter