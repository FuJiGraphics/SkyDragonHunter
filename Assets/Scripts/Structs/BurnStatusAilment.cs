using UnityEngine;
using SkyDragonHunter.Utility;

namespace SkyDragonHunter.Structs
{
    /*
     * @brief 화상(상태 이상)을 정의하는 구조체
     */
    public struct BurnStatusAilment
    {
        public GameObject attacker;         // 공격하는 주체
        public GameObject defender;         // 공격 받는 대상
        public AlphaUnit damagePerSeconds;  // 초당 데미지
        public float duration;              // 지속 시간
    
    }; // struct Attack

} // namespace SkyDragonHunter.Structs