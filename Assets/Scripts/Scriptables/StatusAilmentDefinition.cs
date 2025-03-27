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
        // �Ӽ� (Properties)
        // �ܺ� ���Ӽ� �ʵ� (External dependencies field)
        // �̺�Ʈ (Events)
        // ����Ƽ (ScriptableObject �⺻ �޼���)
        // Public �޼���
        public virtual void Execute(GameObject attacker, GameObject defender)
        {
            throw new System.NotImplementedException();
        }

        // Private �޼���
        // Others

    } // Scope by class NewScript
} // namespace SkyDragonHunter