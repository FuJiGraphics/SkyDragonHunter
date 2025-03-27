using SkyDragonHunter.Gameplay;
using SkyDragonHunter.Interfaces;
using SkyDragonHunter.Managers;
using SkyDragonHunter.Structs;
using SkyDragonHunter.Utility;
using SkyDraonHunter.Utility;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.SceneManagement;

namespace SkyDragonHunter.Scriptables
{
    [CreateAssetMenu(fileName = "StatusAilmentBurnSC.asset", menuName = "StatusAilments/StatusAilmentBurnSC")]
    public class StatusAilmentBurnSC : StatusAilmentDefinition
    {
        // 필드 (Fields)
        // 속성 (Properties)
        // 외부 종속성 필드 (External dependencies field)
        // 이벤트 (Events)
        // 유니티 (MonoBehaviour 기본 메서드)
        // Public 메서드
        // Private 메서드
        // Others
        public override void Execute(GameObject attacker, GameObject defender)
        {
            if (defender == null)
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