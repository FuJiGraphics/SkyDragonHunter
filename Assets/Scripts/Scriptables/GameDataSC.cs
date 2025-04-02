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
        // 필드 (Fields)
        public int ID;
        public string Name;
        public double HP;
        public float Speed;

        // 속성 (Properties)
        // 외부 종속성 필드 (External dependencies field)
        // 이벤트 (Events)
        // 유니티 (ScriptableObject 기본 메서드)
        // Public 메서드
        // Others

    } // Scope by class GameDataSC
} // namespace SkyDragonHunter