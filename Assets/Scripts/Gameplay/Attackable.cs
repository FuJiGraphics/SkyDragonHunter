using SkyDragonHunter.Interfaces;
using SkyDragonHunter.Managers;
using SkyDragonHunter.Structs;
using UnityEngine;

namespace SkyDragonHunter {

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

            if (attack.isCritical)
            {
                DrawableMgr.Text(transform.position, attack.damage.ToString(), Color.red);
            }
            else
            {
                DrawableMgr.Text(transform.position,  attack.damage.ToString());
            }

            m_DamageReceiver.TakeDamage(attacker, attack.damage);
        }

    } // Scope by class Attackable
} // namespace SkyDragonHunter