using SkyDragonHunter.Structs;
using UnityEngine;

namespace SkyDragonHunter.Interfaces
{
    public interface IAttacker
    {
        public abstract void Execute(GameObject attacker, GameObject defender);

    } // Scope by interface IAttacker
} // namespace SkyDragonHunter.Interfaces