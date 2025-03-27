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
        // �ʵ� (Fields)
        [Tooltip("ȭ�� ������ ����")]
        public float burnMultiplier = 1.0f;
        [Tooltip("ȭ�� ���� �ð�")]
        public float duration = 5f;

        // �Ӽ� (Properties)
        // �ܺ� ���Ӽ� �ʵ� (External dependencies field)
        // �̺�Ʈ (Events)
        // ����Ƽ (ScriptableObject �⺻ �޼���)
        // Public �޼���
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

        // Private �޼���
        // Others

    } // Scope by class NewScript
} // namespace SkyDragonHunter