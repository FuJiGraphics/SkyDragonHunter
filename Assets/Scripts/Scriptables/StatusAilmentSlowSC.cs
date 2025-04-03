using SkyDragonHunter.Gameplay;
using SkyDragonHunter.Interfaces;
using SkyDragonHunter.Structs;
using UnityEngine;

namespace SkyDragonHunter.Scriptables
{
    [CreateAssetMenu(fileName = "StatusAilmentSlowSC.asset", menuName = "StatusAilments/StatusAilmentSlowSC")]
    public class StatusAilmentSlowSC : StatusAilmentDefinition
    {
        // 필드 (Fields)
        [Tooltip("슬로우 강도")]
        public float slowMultiplier = 0.2f; // 5분의 1%
        [Tooltip("슬로우 지속 시간")]
        public float duration = 5f;
        [Tooltip("슬로우 내성 배율")]
        public float immunityMultiplier = 2f;
        [Tooltip("상태 이상 걸릴 확률")]
        public float chance = 0.3f;

        // 속성 (Properties)
        // 외부 종속성 필드 (External dependencies field)
        // 이벤트 (Events)
        // 유니티 (MonoBehaviour 기본 메서드)
        // Public 메서드
        // Private 메서드
        private SlowStatusAilment CreateSlowStatusAliment(CharacterStatus aStats, CharacterStatus dStats)
        {
            SlowStatusAilment ailment = new SlowStatusAilment();
            ailment.attacker = aStats.gameObject;
            ailment.defender = dStats.gameObject;
            ailment.duration = duration;
            ailment.slowMultiplier = 0.2f;
            ailment.immunityMultiplier = duration * immunityMultiplier;
            return ailment;
        }

        // Others
        public override void Execute(GameObject attacker, GameObject defender)
        {
            if (defender == null)
                return;
            if (Random.value > chance)
                return;

            CharacterStatus aStats = attacker.GetComponent<CharacterStatus>();
            CharacterStatus dStats = defender.GetComponent<CharacterStatus>();

            if (aStats == null || dStats == null)
                return;

            var burnStatusAliment = CreateSlowStatusAliment(aStats, dStats);
            IAilmentAffectable target = defender.GetComponent<IAilmentAffectable>();
            if (target == null)
                return;

            target.OnSlow(burnStatusAliment);
        }

    } // Scope by class StatusAlimentBurnSC
} // namespace SkyDragonHunter