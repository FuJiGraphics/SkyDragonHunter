using SkyDragonHunter.Scriptables;
using SkyDragonHunter.Structs;
using UnityEngine;

namespace SkyDragonHunter.Interfaces
{
    public interface IBallLifecycleHandler
    {
        void OnStart(GameObject caster);        // 대포가 발사됐을 때
        void OnHitBefore(GameObject caster);    // 공격 전 효과
        void OnHitEnter(GameObject defender);   // 공격이 맞았을 때
        void OnHitStay(GameObject defender);    // 공격에 맞고 있을 때
        void OnHitExit(GameObject defender);    // 공격이 끝났을 때
        void OnHitAfter(GameObject caster);     // 공격 후 효과
        void OnEnd(GameObject caster);          // 대포 투사체가 사라질 때

    } // Scope by interface ISkillLifecycleHandler
} // namespace SkyDragonHunter.Interfaces

