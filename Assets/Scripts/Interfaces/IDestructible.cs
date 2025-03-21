using SkyDragonHunter.Structs;
using UnityEngine;

namespace SkyDragonHunter.Interfaces {

    public interface IDestructible
    {
        public void OnDestruction(GameObject attacker);

    } // Scope by interface IDestruction
} // namespace SkyDragonHunter.Interfaces