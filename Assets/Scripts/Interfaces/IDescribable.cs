using SkyDragonHunter.Structs;
using UnityEngine;

namespace SkyDragonHunter.Interfaces {

    public interface IDescribable
    {
        string Name { get; }
        string Desc { get; }
        Sprite Icon { get; }

    } // Scope by interface IDescribable
} // namespace SkyDragonHunter.Interfaces