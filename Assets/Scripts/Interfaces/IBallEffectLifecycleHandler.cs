using SkyDragonHunter.Scriptables;
using SkyDragonHunter.Structs;
using UnityEngine;

namespace SkyDragonHunter.Interfaces
{
    public interface IBallEffectLifecycleHandler
    {
        void OnCastEffect(GameObject attacker);
        void OnHitEnterEffect(GameObject attacker, GameObject defender);   // 공격이 맞고 난 이후
        void OnHitStayEffect(GameObject attacker, GameObject defender);    // 공격에 맞고 있을 때 이후

    } // Scope by interface ISkillLifecycleHandler
} // namespace SkyDragonHunter.Interfaces

