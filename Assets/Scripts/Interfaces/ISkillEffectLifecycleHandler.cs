using SkyDragonHunter.Scriptables;
using SkyDragonHunter.Structs;
using UnityEngine;

namespace SkyDragonHunter.Interfaces
{
    public interface ISkillEffectLifecycleHandler
    {
        void OnHitEnterEffect(GameObject caster, GameObject receiver);   // 스킬이 맞고 난 이후
        void OnHitStayEffect(GameObject caster, GameObject receiver);    // 스킬에 맞고 있을 때 이후

    } // Scope by interface ISkillEffectLifecycleHandler
} // namespace SkyDragonHunter.Interfaces

