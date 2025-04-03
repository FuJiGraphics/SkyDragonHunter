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
        // �ʵ� (Fields)
        public int id;
        public string skillName;
        public double damage;   // ������
        public float coolDown;  // ��ٿ�

        // �Ӽ� (Properties)
        // �ܺ� ���Ӽ� �ʵ� (External dependencies field)
        // �̺�Ʈ (Events)
        // ����Ƽ (ScriptableObject �⺻ �޼���)
        // Public �޼���
        // Others

    } // Scope by class SkillDefinition
} // namespace SkyDragonHunter