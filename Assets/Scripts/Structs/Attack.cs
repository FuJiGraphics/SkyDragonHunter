
using UnityEngine;
using SkyDragonHunter.Utility;
using SkyDragonHunter.Interfaces;

namespace SkyDragonHunter.Structs {

    public struct Attack
    {
        public GameObject attacker;
        public GameObject defender;    // 공격 받는 대상
        public BigNum damage;       // 데미지
        public bool isCritical;

    }; // struct Attack

} // namespace SkyDragonHunter.Structs