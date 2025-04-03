using SkyDragonHunter.Gameplay;
using SkyDragonHunter.Interfaces;
using SkyDragonHunter.Structs;
using UnityEngine;

namespace SkyDragonHunter.Scriptables
{
    [CreateAssetMenu(fileName = "StatusAilmentBurnSC.asset", menuName = "StatusAilments/StatusAilmentBurnSC")]
    public class StatusAilmentBurnSC : StatusAilmentDefinition
    {
        // 필드 (Fields)
        [Tooltip("화상 데미지 배율")]
        public float burnMultiplier = 1.0f;
        [Tooltip("화상 지속 시간")]
        public float duration = 5f;
        [Tooltip("상태 이상 걸릴 확률")]
        public float chance = 0.3f;

        // 속성 (Properties)
        // 외부 종속성 필드 (External dependencies field)
        // 이벤트 (Events)
        // 유니티 (MonoBehaviour 기본 메서드)
        // Public 메서드
        // Private 메서드
        private BurnStatusAilment CreateBurnStatusAliment(CharacterStatus aStats, CharacterStatus dStats)
        {
            BurnStatusAilment ailment = new BurnStatusAilment();
            ailment.attacker = aStats.gameObject;
            ailment.defender = dStats.gameObject;
            ailment.damagePerSeconds = (aStats.Damage * burnMultiplier) / duration;
            ailment.duration = duration;
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

            var burnStatusAliment = CreateBurnStatusAliment(aStats, dStats);
            IAilmentAffectable target = defender.GetComponent<IAilmentAffectable>();
            if (target == null)
                return;

            target.OnBurn(burnStatusAliment);
        }

    } // Scope by class StatusAlimentBurnSC
} // namespace SkyDragonHunter