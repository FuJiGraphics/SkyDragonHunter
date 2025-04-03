using SkyDragonHunter.Gameplay;
using SkyDragonHunter.Interfaces;
using SkyDragonHunter.Structs;
using UnityEngine;

namespace SkyDragonHunter.Scriptables
{
    [CreateAssetMenu(fileName = "StatusAilmentSlowSC.asset", menuName = "StatusAilments/StatusAilmentSlowSC")]
    public class StatusAilmentSlowSC : StatusAilmentDefinition
    {
        // �ʵ� (Fields)
        [Tooltip("���ο� ����")]
        public float slowMultiplier = 0.2f; // 5���� 1%
        [Tooltip("���ο� ���� �ð�")]
        public float duration = 5f;
        [Tooltip("���ο� ���� ����")]
        public float immunityMultiplier = 2f;
        [Tooltip("���� �̻� �ɸ� Ȯ��")]
        public float chance = 0.3f;

        // �Ӽ� (Properties)
        // �ܺ� ���Ӽ� �ʵ� (External dependencies field)
        // �̺�Ʈ (Events)
        // ����Ƽ (MonoBehaviour �⺻ �޼���)
        // Public �޼���
        // Private �޼���
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