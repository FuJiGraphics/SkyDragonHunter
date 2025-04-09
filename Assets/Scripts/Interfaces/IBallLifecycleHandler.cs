using SkyDragonHunter.Scriptables;
using SkyDragonHunter.Structs;
using UnityEngine;

namespace SkyDragonHunter.Interfaces
{
    public interface IBallLifecycleHandler
    {
        void OnStart(GameObject caster);        // ������ �߻���� ��
        void OnHitBefore(GameObject caster);    // ���� �� ȿ��
        void OnHitEnter(GameObject defender);   // ������ �¾��� ��
        void OnHitStay(GameObject defender);    // ���ݿ� �°� ���� ��
        void OnHitExit(GameObject defender);    // ������ ������ ��
        void OnHitAfter(GameObject caster);     // ���� �� ȿ��
        void OnEnd(GameObject caster);          // ���� ����ü�� ����� ��

    } // Scope by interface ISkillLifecycleHandler
} // namespace SkyDragonHunter.Interfaces

