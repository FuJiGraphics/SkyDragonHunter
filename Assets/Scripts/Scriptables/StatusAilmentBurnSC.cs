using SkyDragonHunter.Gameplay;
using SkyDragonHunter.Interfaces;
using SkyDragonHunter.Structs;
using UnityEngine;

namespace SkyDragonHunter.Scriptables
{
    [CreateAssetMenu(fileName = "StatusAilmentBurnSC.asset", menuName = "StatusAilments/StatusAilmentBurnSC")]
    public class StatusAilmentBurnSC : StatusAilmentDefinition
    {
        // �ʵ� (Fields)
        [Tooltip("ȭ�� ������ ����")]
        public float burnMultiplier = 1.0f;
        [Tooltip("ȭ�� ���� �ð�")]
        public float duration = 5f;
        [Tooltip("���� �̻� �ɸ� Ȯ��")]
        public float chance = 0.3f;

        // �Ӽ� (Properties)
        // �ܺ� ���Ӽ� �ʵ� (External dependencies field)
        // �̺�Ʈ (Events)
        // ����Ƽ (MonoBehaviour �⺻ �޼���)
        // Public �޼���
        // Private �޼���
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