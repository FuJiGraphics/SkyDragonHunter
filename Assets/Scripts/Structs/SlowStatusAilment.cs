using UnityEngine;
using SkyDragonHunter.Utility;

namespace SkyDragonHunter.Structs
{
    /*
     * @brief 둔화(상태 이상)을 정의하는 구조체
     */
    public struct SlowStatusAilment
    {
        public GameObject attacker;         // 공격하는 주체
        public GameObject defender;         // 공격 받는 대상
        public float slowMultiplier;        // 슬로우 배율
        public float duration;              // 지속 시간
        public float immunityMultiplier;    // 둔화 내성 배율 (빙결 내성 시간 = 지속 시간 * 빙결 배율)
    }; // struct SlowStatusAilment

} // namespace SkyDragonHunter.Structs