using SkyDragonHunter.Structs;
using System.Numerics;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;

namespace SkyDragonHunter.Managers
{
    public static class Math2DHelper
    {
        public static bool Equals(double a, double b)
            => System.Math.Abs(a - b) < 1e-6;

        public static BigNum Max(BigNum a, BigNum b)
            => a > b ? a : b;

        public static BigNum Min(BigNum a, BigNum b)
            => a < b ? a : b;

        public static BigNum Clamp(BigNum value, BigNum min, BigNum max)
            => Min(Max(value, min), max);

        public static BigInteger AbsTemp(BigInteger value)
            => value < 0 ? -value : value;

        public static double SmartRoundTemp(double d)
            => (double)((BigInteger)((d >= 0.0) ? d + 0.5 : d - 0.5));
        

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

        public static Quaternion GetLookAtRotation(UnityEngine.Vector3 start, UnityEngine.Vector3 end)
        {
            UnityEngine.Vector3 direction = end - start;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            return Quaternion.Euler(0, 0, angle);
        }

    } // static class Math2DHelper
} // namespace SkyDragonHunter.Managers