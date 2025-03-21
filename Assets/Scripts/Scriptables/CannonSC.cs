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
        // 필드 (Fields)
        public GameObject projectilePrefab;

        // 속성 (Properties)
        // 외부 종속성 필드 (External dependencies field)
        // 이벤트 (Events)
        // Public 메서드
        public override void Execute(GameObject attacker, GameObject defender)
        {
            throw new System.NotImplementedException();


        }

        // Private 메서드
        // Others

    } // Scope by class NewScript
} // namespace SkyDragonHunter