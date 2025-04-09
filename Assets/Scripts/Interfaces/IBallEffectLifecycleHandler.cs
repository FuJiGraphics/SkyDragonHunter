using SkyDragonHunter.Scriptables;
using SkyDragonHunter.Structs;
using UnityEngine;

namespace SkyDragonHunter.Interfaces
{
    public interface IBallEffectLifecycleHandler
    {
        void OnHitEnterEffect(GameObject attacker, GameObject defender);   // ������ �°� �� ����
        void OnHitStayEffect(GameObject attacker, GameObject defender);    // ���ݿ� �°� ���� �� ����

    } // Scope by interface ISkillLifecycleHandler
} // namespace SkyDragonHunter.Interfaces

