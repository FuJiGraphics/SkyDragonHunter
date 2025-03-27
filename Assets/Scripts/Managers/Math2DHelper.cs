
using UnityEngine;

namespace SkyDragonHunter.Managers
{
    public static class Math2DHelper
    {
        public static bool Equals(double a, double b)
            => System.Math.Abs(a - b) < 1e-6;

        /*
         * @brief
         * 3D 공간에서 2D 오브젝트 간의 LookAt 메서드
         * @param[out]  dst: 적용할 대상
         * @param[in]   start: 시작 위치
         * @param[in]   end: 바라볼 대상
         */
        public static void LookAt(Transform dst, Transform start, Transform end)
        {
            Vector3 direction = (end.position - start.position);
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            dst.rotation = Quaternion.Euler(0, 0, angle);
        }

    } // static class Math2DHelper
} // namespace SkyDragonHunter.Managers