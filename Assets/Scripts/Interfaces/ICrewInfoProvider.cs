using SkyDragonHunter.Structs;
using UnityEngine;

namespace SkyDragonHunter.Interfaces
{
    public interface ICrewInfoProvider
    {
        string Name { get; }
        string Damage { get; }
        string Health { get; }
        string Defense { get; }
        Sprite Icon { get; }
        Sprite Preview { get; }
        bool IsMounted { get; }

    } // Scope by interface IAttackTargetProvider
} // namespace SkyDragonHunter.Interfaces