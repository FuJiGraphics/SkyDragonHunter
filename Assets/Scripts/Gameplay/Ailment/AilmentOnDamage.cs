using SkyDragonHunter.Interfaces;
using SkyDragonHunter.Managers;
using SkyDragonHunter.Structs;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

namespace SkyDragonHunter.Gameplay {

    public class AilmentOnDamage : MonoBehaviour
        , IAilmentLifecycleHandler
    {
        // 필드 (Fields)
        private GameObject m_Caster;
        private DamageReceiver m_Receiver;
        private BigNum m_DamageNum;

        // 속성 (Properties)
        // 외부 종속성 필드 (External dependencies field)
        // 이벤트 (Events)
        // 유니티 (MonoBehaviour 기본 메서드)
        // Public 메서드
        public void OnEnter(GameObject receiver)
        {
            m_Caster = GetComponent<AilmentBase>().Caster;
            m_Receiver = receiver.GetComponent<DamageReceiver>();
            if (m_Caster.TryGetComponent<CharacterStatus>(out var status))
            {
                m_DamageNum = status.MaxHealth / 100;
            }
        }

        public void OnStay()
        {
            BigNum damage = m_DamageNum;
            DrawableMgr.TopText(m_Receiver.transform.position, damage.ToUnit(), Color.red);
            m_Receiver.TakeDamage(m_Caster, m_DamageNum);
        }

        public void OnExit()
        {
            // Empty
        }

        // Private 메서드
        // Others

    } // Scope by class AilmentOnDamage
} // namespace SkyDragonHunter