using SkyDragonHunter.Structs;
using UnityEngine;

namespace SkyDragonHunter.Interfaces
{
    public interface ISkillLifecycleHandler
    {
        void OnSkillCast(GameObject caster);        // ��ų�� ������ ��
        void OnSkillHitBefore(GameObject caster);   // ���� �� ȿ��
        void OnSkillHitEnter(GameObject defender);  // ������ �¾��� ��
        void OnSkillHitStay(GameObject defender);   // ��ų�� �°� ���� ��
        void OnSkillHitExit(GameObject defender);   // ������ ������ ��
        void OnSkillHitAfter(GameObject caster);    // ���� �� ȿ��
        void OnSkillEnd(GameObject caster);         // ��ų�� ����� ��

    } // Scope by interface ISkillLifecycleHandler
} // namespace SkyDragonHunter.Interfaces

