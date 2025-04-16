using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SkyDragonHunter.Gameplay {

    public class SkillAttackEndDestroy : MonoBehaviour
    {
        // 필드 (Fields)
        private SkillOnHitDamage m_SkillHitDamage;

        // 속성 (Properties)
        // 외부 종속성 필드 (External dependencies field)
        // 이벤트 (Events)
        // 유니티 (MonoBehaviour 기본 메서드)
        private void Start()
        {
            m_SkillHitDamage = GetComponent<SkillOnHitDamage>();
        }
    
        private void Update()
        {
            if (m_SkillHitDamage.IsAttackEnd)
            {
                Destroy(gameObject);
            }
        }
    
        // Public 메서드
        // Private 메서드
        // Others
    
    } // Scope by class SkillOnAttackEndDestroy
} // namespace SkyDragonHunter