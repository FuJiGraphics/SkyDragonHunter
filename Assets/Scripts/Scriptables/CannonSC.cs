using SkyDragonHunter.Gameplay;
using SkyDragonHunter.Interfaces;
using SkyDragonHunter.Structs;
using SkyDragonHunter.Utility;
using SkyDraonHunter.Utility;
using System;
using UnityEngine;

namespace SkyDragonHunter.Scriptables
{
    [CreateAssetMenu(fileName = "CanonSC.asset", menuName = "Weapon/CanonSC")]
    public class CanonSC : AttackDefinition
    {
        // �ʵ� (Fields)
        public GameObject projectilePrefab;

        // �Ӽ� (Properties)
        // �ܺ� ���Ӽ� �ʵ� (External dependencies field)
        // �̺�Ʈ (Events)
        // Public �޼���
        public override void Execute(GameObject attacker, GameObject defender)
        {
            throw new System.NotImplementedException();


        }

        // Private �޼���
        // Others

    } // Scope by class NewScript
} // namespace SkyDragonHunter