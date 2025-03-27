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
        // �ʵ� (Fields)
        // �Ӽ� (Properties)
        // �ܺ� ���Ӽ� �ʵ� (External dependencies field)
        // �̺�Ʈ (Events)
        // ����Ƽ (MonoBehaviour �⺻ �޼���)
        // Public �޼���
        // Private �޼���
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