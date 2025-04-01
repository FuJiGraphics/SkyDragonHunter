using System;
using System.Numerics;

namespace SkyDragonHunter.Structs {
    [System.Serializable]
    public struct AlphaUnit : IComparable<AlphaUnit>, IEquatable<AlphaUnit>
    {
        public const double Epsilon = double.Epsilon;
        public const double MaxValue = double.MaxValue;
        public const double MinValue = double.MinValue;
        public const double NaN = double.NaN;
        public const double NegativeInfinity = double.NegativeInfinity;
        public const double PositiveInfinity = double.PositiveInfinity;

        private double m_OriginNumber;
        private string m_StringNumber;
        private string m_StringBase;
        private char[] m_Base;
        private bool m_IsInfinity;
        private bool m_IsNaN;

        public double Value => m_OriginNumber;

        public AlphaUnit(double number)
        {
            m_OriginNumber = number < 0.0 ? 0.0 : number;

            m_StringNumber = "0";
            m_StringBase = null;
            m_Base = new char[1] { (char)('a' - 1) };

            m_IsInfinity = double.IsInfinity(m_OriginNumber);
            m_IsNaN = double.IsNaN(m_OriginNumber);
        }

        private AlphaUnit(double number, bool rawInit)
        {
            m_OriginNumber = number < 0.0 ? 0.0 : number;

            m_StringNumber = "0";
            m_StringBase = null;
            m_Base = new char[1] { (char)('a' - 1) };

            m_IsInfinity = double.IsInfinity(m_OriginNumber);
            m_IsNaN = double.IsNaN(m_OriginNumber);
        }

        public int CompareTo(AlphaUnit other)
            => m_OriginNumber.CompareTo(other.m_OriginNumber);

        public bool Equals(AlphaUnit other)
            => m_OriginNumber == other.m_OriginNumber;

        private void Normalize()
        {
            if (m_StringBase != null || m_OriginNumber <= 0.0 || m_IsInfinity || m_IsNaN)
                return;

            double targetNumber = m_OriginNumber;

            int unitShift = (int)(Math.Log10(targetNumber) / 3);
            targetNumber *= Math.Pow(0.001, unitShift);

            m_Base = new char[1] { (char)('a' - 1) };
            for (int i = 0; i < unitShift; ++i)
                IncreaseUnit();

            if (targetNumber >= 999.9995)
            {
                targetNumber = 1.0;
                IncreaseUnit();
            }

            targetNumber = Math.Truncate(targetNumber * 1000) / 1000;
            m_StringNumber = targetNumber.ToString("F1");
            m_StringBase = GetBaseString();
        }

        private void IncreaseUnit()
        {
            int index = 0;
            while (index <= m_Base.Length)
            {
                Resize(index);
                m_Base[index]++;
                if (m_Base[index] > 'z')
                {
                    m_Base[index] = 'a';
                    index++;
                }
                else
                {
                    break;
                }
            }
        }

        private void Resize(int index)
        {
            if (m_Base == null || index < m_Base.Length)
                return;

            int newSize = index + 1;
            char[] newBase = new char[newSize];
            for (int i = 0; i < newSize; ++i)
                newBase[i] = i < m_Base.Length ? m_Base[i] : (char)('a' - 1);
            m_Base = newBase;
        }

        private string GetBaseString()
        {
            int validCount = 0;
            for (int i = 0; i < m_Base.Length; ++i)
            {
                if (m_Base[i] < 'a' || m_Base[i] > 'z')
                    break;
                validCount++;
            }

            return new string(m_Base, 0, validCount);
        }

        public static implicit operator AlphaUnit(double number) => new AlphaUnit(number);
        public static explicit operator double(AlphaUnit a) => a.m_OriginNumber;

        public static AlphaUnit operator +(AlphaUnit a, AlphaUnit b)
            => new AlphaUnit((double)(new BigInteger(a.m_OriginNumber) + new BigInteger(b.m_OriginNumber)), true);
        public static AlphaUnit operator -(AlphaUnit a, AlphaUnit b)
            => new AlphaUnit((double)(new BigInteger(a.m_OriginNumber) - new BigInteger(b.m_OriginNumber)), true);
        public static AlphaUnit operator *(AlphaUnit a, AlphaUnit b)
            => new AlphaUnit((double)(new BigInteger(a.m_OriginNumber) * new BigInteger(b.m_OriginNumber)), true);
        public static AlphaUnit operator /(AlphaUnit a, AlphaUnit b)
            => new AlphaUnit((double)(new BigInteger(a.m_OriginNumber) / new BigInteger(b.m_OriginNumber)), true);

        public static bool operator <(AlphaUnit a, AlphaUnit b) => a.CompareTo(b) < 0;
        public static bool operator >(AlphaUnit a, AlphaUnit b) => a.CompareTo(b) > 0;
        public static bool operator <=(AlphaUnit a, AlphaUnit b) => a.CompareTo(b) <= 0;
        public static bool operator >=(AlphaUnit a, AlphaUnit b) => a.CompareTo(b) >= 0;
        public static bool operator ==(AlphaUnit a, AlphaUnit b) => a.CompareTo(b) == 0;
        public static bool operator !=(AlphaUnit a, AlphaUnit b) => a.CompareTo(b) != 0;

        public override string ToString()
        {
            if (m_IsInfinity) return "Inf";
            if (m_IsNaN) return "NaN";

            Normalize();
            return m_StringNumber + m_StringBase;
        }

        public override bool Equals(object obj)
        {
            if (obj is AlphaUnit other)
                return this == other;
            return false;
        }

        public override int GetHashCode() => m_OriginNumber.GetHashCode();

    } // class AlphaUnit
} // namespace SkyDragonHunter.Utility
