using SkyDragonHunter.Structs;
using UnityEngine;

namespace SkyDragonHunter.Interfaces
{
    public interface ICanonInfoProvider
    {
        string Name { get; }
        Sprite Icon { get; }
        UnityEngine.Color Color { get; }

    } // Scope by interface ICanonInfoProvider
} // namespace SkyDragonHunter.Interfaces