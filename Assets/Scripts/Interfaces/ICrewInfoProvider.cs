using SkyDragonHunter.Structs;
using UnityEngine;

namespace SkyDragonHunter.Interfaces
{
    public interface ICrewInfoProvider
    {
        string Title { get; }
        string Name { get; }
        string Damage { get; }
        string Health { get; }
        string Defense { get; }
        Sprite Icon { get; }
        Sprite Preview { get; }
        bool IsMounted { get; }
        bool IsEquip { get; }
        int QuantityCount { get; }

    } // Scope by interface IAttackTargetProvider
} // namespace SkyDragonHunter.Interfaces