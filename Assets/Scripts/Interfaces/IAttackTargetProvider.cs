using SkyDragonHunter.Structs;
using UnityEngine;

namespace SkyDragonHunter.Interfaces
{
    public interface IAttackTargetProvider
    {
        public string[] AllowedTargetTags { get; }

    } // Scope by interface IAttackTargetProvider
} // namespace SkyDragonHunter.Interfaces