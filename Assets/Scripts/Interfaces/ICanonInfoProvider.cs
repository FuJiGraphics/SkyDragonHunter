using SkyDragonHunter.Structs;
using UnityEngine;

namespace SkyDragonHunter.Interfaces
{
    public interface ICanonInfoProvider
    {
        string Name { get; }
        Sprite Icon { get; }

    } // Scope by interface ICanonInfoProvider
} // namespace SkyDragonHunter.Interfaces