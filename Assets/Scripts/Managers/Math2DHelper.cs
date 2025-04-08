using SkyDragonHunter.Structs;
using System.Numerics;
using UnityEngine;

namespace SkyDragonHunter.Managers
{
    public static class Math2DHelper
    {
        public static bool Equals(double a, double b)
            => System.Math.Abs(a - b) < 1e-6;

        public static BigInteger Max(BigInteger a, BigInteger b)
            => a > b ? a : b;

        public static BigInteger Min(BigInteger a, BigInteger b)
            => a < b ? a : b;

        public static BigInteger Clamp(BigInteger value, BigInteger min, BigInteger max)
            => Min(Max(value, min), max);

        public static BigInteger Abs(BigInteger value)
            => value < 0 ? -value : value;

        /*
         * @brief
         * 3D �������� 2D ������Ʈ ���� LookAt �޼���
         * @param[out]  dst: ������ ���
         * @param[in]   start: ���� ��ġ
         * @param[in]   end: �ٶ� ���
         */
        public static void LookAt(Transform dst, Transform start, Transform end)
        {
            UnityEngine.Vector3 direction = (end.position - start.position);
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            dst.rotation = UnityEngine.Quaternion.Euler(0, 0, angle);
        }

    } // static class Math2DHelper
} // namespace SkyDragonHunter.Managers