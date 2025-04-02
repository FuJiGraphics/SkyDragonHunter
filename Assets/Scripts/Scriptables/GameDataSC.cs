using SkyDragonHunter.Gameplay;
using SkyDragonHunter.Interfaces;
using SkyDragonHunter.Structs;
using SkyDragonHunter.Utility;
using SkyDraonHunter.Utility;
using System;
using UnityEngine;
using UnityEngine.U2D;

namespace SkyDragonHunter.Scriptables {

    [CreateAssetMenu(fileName = "GameDataSC.asset", menuName = "Skills/GameDataSC")]
    public class GameDataSC : ScriptableObject
    {
        // �ʵ� (Fields)
        public int ID;
        public string Name;
        public double HP;
        public float Speed;

        // �Ӽ� (Properties)
        // �ܺ� ���Ӽ� �ʵ� (External dependencies field)
        // �̺�Ʈ (Events)
        // ����Ƽ (ScriptableObject �⺻ �޼���)
        // Public �޼���
        // Others

    } // Scope by class GameDataSC
} // namespace SkyDragonHunter