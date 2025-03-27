using SkyDragonHunter.Structs;
using UnityEngine;

namespace SkyDragonHunter.Interfaces
{
    public interface IPassiveSkill
    {
        public void OnExecute(GameObject attacker, Attack attack);

    } // Scope by interface IAttackable
} // namespace SkyDragonHunter.Interfaces