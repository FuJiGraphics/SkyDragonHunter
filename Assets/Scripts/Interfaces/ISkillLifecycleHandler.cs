using SkyDragonHunter.Structs;
using UnityEngine;

namespace SkyDragonHunter.Interfaces
{
    public interface ISkillLifecycleHandler
    {
        void OnSkillCast(GameObject caster);        // 스킬을 시전할 때
        void OnSkillHitBefore(GameObject caster);   // 공격 전 효과
        void OnSkillHitEnter(GameObject defender);  // 공격이 맞았을 때
        void OnSkillHitStay(GameObject defender);   // 스킬에 맞고 있을 때
        void OnSkillHitExit(GameObject defender);   // 공격이 끝났을 때
        void OnSkillHitAfter(GameObject caster);    // 공격 후 효과
        void OnSkillEnd(GameObject caster);         // 스킬이 사라질 때

    } // Scope by interface ISkillLifecycleHandler
} // namespace SkyDragonHunter.Interfaces

