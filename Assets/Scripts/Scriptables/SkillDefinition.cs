using SkyDragonHunter.Gameplay;
using SkyDragonHunter.Interfaces;
using SkyDragonHunter.Structs;
using SkyDragonHunter.Utility;
using SkyDraonHunter.Utility;
using System;
using UnityEngine;

namespace SkyDragonHunter.Scriptables {

    public class SkillDefinition : ScriptableObject
    {
        // 필드 (Fields)
        public int id;
        public string skillName;
        public double damage;   // 데미지
        public float coolDown;  // 쿨다운

        // 속성 (Properties)
        // 외부 종속성 필드 (External dependencies field)
        // 이벤트 (Events)
        // 유니티 (ScriptableObject 기본 메서드)
        // Public 메서드
        // Others

    } // Scope by class SkillDefinition
} // namespace SkyDragonHunter