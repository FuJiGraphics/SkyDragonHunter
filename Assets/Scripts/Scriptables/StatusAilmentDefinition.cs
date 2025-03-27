using SkyDragonHunter.Gameplay;
using SkyDragonHunter.Interfaces;
using SkyDragonHunter.Structs;
using SkyDragonHunter.Utility;
using SkyDraonHunter.Utility;
using System;
using UnityEngine;

namespace SkyDragonHunter.Scriptables
{
    [CreateAssetMenu(fileName = "StatusAilmentDefinition.asset", menuName = "StatusAilments/StatusAilmentDefinition")]
    public class StatusAilmentDefinition : ScriptableObject, IAttacker
    {
        // 필드 (Fields)
        [Tooltip("화상 데미지 배율")]
        public float burnMultiplier = 1.0f;
        [Tooltip("화상 지속 시간")]
        public float duration = 5f;

        // 속성 (Properties)
        // 외부 종속성 필드 (External dependencies field)
        // 이벤트 (Events)
        // 유니티 (ScriptableObject 기본 메서드)
        // Public 메서드
        public virtual void Execute(GameObject attacker, GameObject defender)
        {
            throw new System.NotImplementedException();
        }

        protected BurnStatusAliment CreateBurnStatusAliment(CharacterStatus aStats, CharacterStatus dStats)
        {
            BurnStatusAliment ailment = new BurnStatusAliment();
            ailment.attacker = aStats.gameObject;
            ailment.defender = dStats.gameObject;
            ailment.damagePerSeconds = (aStats.currentDamage * burnMultiplier) / duration;
            ailment.duration = duration;
            return ailment;
        }

        // Private 메서드
        // Others

    } // Scope by class NewScript
} // namespace SkyDragonHunter