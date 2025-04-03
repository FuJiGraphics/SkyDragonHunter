using SkyDragonHunter.Gameplay;
using SkyDragonHunter.Interfaces;
using SkyDragonHunter.Structs;
using UnityEngine;

namespace SkyDragonHunter.Scriptables
{
    [CreateAssetMenu(fileName = "StatusAilmentFreezeSC.asset", menuName = "StatusAilments/StatusAilmentFreezeSC")]
    public class StatusAilmentFreezeSC : StatusAilmentDefinition
    {
        // �ʵ� (Fields)
        [Tooltip("���� ���� �ð�")]
        public float duration = 5f;
        [Tooltip("���� ���� ����")]
        public float immunityMultiplier = 2f;
        [Tooltip("���� �̻� �ɸ� Ȯ��")]
        public float chance = 0.3f;

        // �Ӽ� (Properties)
        // �ܺ� ���Ӽ� �ʵ� (External dependencies field)
        // �̺�Ʈ (Events)
        // ����Ƽ (MonoBehaviour �⺻ �޼���)
        // Public �޼���
        // Private �޼���
        private FreezeStatusAilment CreateFreezeStatusAliment(CharacterStatus aStats, CharacterStatus dStats)
        {
            FreezeStatusAilment ailment = new FreezeStatusAilment();
            ailment.attacker = aStats.gameObject;
            ailment.defender = dStats.gameObject;
            ailment.duration = duration;
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

            var burnStatusAliment = CreateFreezeStatusAliment(aStats, dStats);
            IAilmentAffectable target = defender.GetComponent<IAilmentAffectable>();
            if (target == null)
                return;

            target.OnFreeze(burnStatusAliment);
        }

    } // Scope by class StatusAlimentBurnSC
} // namespace SkyDragonHunter