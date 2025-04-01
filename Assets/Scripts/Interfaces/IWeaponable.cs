using SkyDragonHunter.Scriptables;
using UnityEngine;

namespace SkyDragonHunter.Interfaces
{
    public interface IWeaponable
    {
        public AttackDefinition WeaponData { get; }
        public void Execute(GameObject attacker, GameObject defender);

    } // Scope by interface IWeaponable
} // namespace SkyDragonHunter.Interfaces