using SkyDragonHunter.Structs;
using UnityEngine;

namespace SkyDragonHunter.Interfaces
{
    public interface IAttackTargetProvider
    {
        public string[] AllowedTargetTags { get; }
        public bool IsAllowedTarget(string tag);

    } // Scope by interface IAttackTargetProvider
} // namespace SkyDragonHunter.Interfaces