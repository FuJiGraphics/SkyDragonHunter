using SkyDragonHunter.Scriptables;
using SkyDragonHunter.Structs;
using UnityEngine;

namespace SkyDragonHunter.Interfaces
{
    public interface ISkillEffectLifecycleHandler
    {
        void OnCastEffect(GameObject caster);                            // ��ų ���� ����Ʈ
        void OnHitEnterEffect(GameObject caster, GameObject receiver);   // ��ų�� �°� �� ����
        void OnHitStayEffect(GameObject caster, GameObject receiver);    // ��ų�� �°� ���� �� ����

    } // Scope by interface ISkillEffectLifecycleHandler
} // namespace SkyDragonHunter.Interfaces

