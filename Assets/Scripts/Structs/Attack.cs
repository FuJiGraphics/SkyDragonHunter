
using UnityEngine;
using SkyDragonHunter.Utility;
using SkyDragonHunter.Interfaces;

namespace SkyDragonHunter.Structs {

    public struct Attack
    {
        public GameObject defender;    // 공격 받는 대상
        public AlphaUnit damage;       // 데미지
        public AlphaUnit critical;     // 치명타

        public bool IsCritical => critical > 0.0;
    }; // struct Attack

} // namespace SkyDragonHunter.Structs