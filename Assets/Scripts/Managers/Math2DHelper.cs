
using UnityEngine;

namespace SkyDragonHunter.Managers
{
    public static class Math2DHelper
    {
        public static bool Equals(double a, double b)
            => System.Math.Abs(a - b) < 1e-6;

        /*
         * @brief
         * 3D �������� 2D ������Ʈ ���� LookAt �޼���
         * @param[out]  dst: ������ ���
         * @param[in]   start: ���� ��ġ
         * @param[in]   end: �ٶ� ���
         */
        public static void LookAt(Transform dst, Transform start, Transform end)
        {
            Vector3 direction = (end.position - start.position);
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            dst.rotation = Quaternion.Euler(0, 0, angle);
        }

    } // static class Math2DHelper
} // namespace SkyDragonHunter.Managers