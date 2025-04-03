using SkyDragonHunter.Structs;
using UnityEngine;

namespace SkyDragonHunter.Interfaces
{
    public interface IDirectional
    {
        Vector3 Direction { get; }
        void SetDirection(Vector2 newDirection);

    } // Scope by interface IDirectional
} // namespace SkyDragonHunter.Interfaces