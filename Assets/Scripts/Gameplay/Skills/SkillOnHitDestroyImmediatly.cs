using SkyDragonHunter.Interfaces;
using UnityEngine;

namespace SkyDragonHunter.Gameplay {

    public class SkillOnHitDestroyImmediatly : MonoBehaviour
        , ISkillEffectLifecycleHandler
    {
        // 필드 (Fields)
        // 속성 (Properties)
        // 외부 종속성 필드 (External dependencies field)
        // 이벤트 (Events)
        // 유니티 (MonoBehaviour 기본 메서드)
        // Public 메서드
        // Private 메서드
        // Others
        public void OnCastEffect(GameObject caster)
        {
        }

        public void OnHitEnterEffect(GameObject caster, GameObject receiver)
        {
            Destroy(gameObject);
        }

        public void OnHitStayEffect(GameObject caster, GameObject receiver)
        {
        }

    } // Scope by class SkillOnHitDestroyImmediatly
} // namespace SkyDragonHunter