using SkyDragonHunter.Structs;
using UnityEngine;

namespace SkyDragonHunter.Interfaces {
    public interface IAttackable
    {
        public void OnAttack(GameObject attacker, Attack attack);

    } // Scope by interface IAttackable
} // namespace SkyDragonHunter.Interfaces