using System;
using System.Numerics;

namespace SkyDragonHunter.Structs {
    [System.Serializable]
    public struct AlphaUnit : IComparable<AlphaUnit>, IEquatable<AlphaUnit>
    {
        private BigInteger m_OriginNumber;
        private string m_StringNumber;
        private string m_StringBase;
        private char[] m_Base;

        public BigInteger Value => m_OriginNumber;

        public AlphaUnit(int number)
        {
            m_OriginNumber = number < 0 ? 0 : number;

            m_StringNumber = "0";
            m_StringBase = null;
            m_Base = new char[1] { (char)('a' - 1) };
        }

        public AlphaUnit(string strNumber)
        {
            BigInteger number = BigInteger.Parse(strNumber);
            m_OriginNumber = number < 0 ? 0 : number;

            m_StringNumber = "0";
            m_StringBase = null;
            m_Base = new char[1] { (char)('a' - 1) };
        }

        public AlphaUnit(BigInteger number)
        {
            m_OriginNumber = number < 0 ? 0 : number;

            m_StringNumber = "0";
            m_StringBase = null;
            m_Base = new char[1] { (char)('a' - 1) };
        }

        private AlphaUnit(BigInteger number, bool rawInit)
        {
            m_OriginNumber = number < 0 ? 0 : number;

            m_StringNumber = "0";
            m_StringBase = null;
            m_Base = new char[1] { (char)('a' - 1) };
        }

        public int CompareTo(AlphaUnit other)
            => m_OriginNumber.CompareTo(other.m_OriginNumber);

        public bool Equals(AlphaUnit other)
            => m_OriginNumber == other.m_OriginNumber;

        private void Normalize()
        {
            if (m_StringBase != null || m_OriginNumber <= 0)
                return;

            BigInteger targetNumber = m_OriginNumber;
            int digits = targetNumber.ToString().Length;
            int unitShift = (digits - 1) / 3; 

            m_Base = new char[1] { (char)('a' - 1) };
            for (int i = 0; i < unitShift; i++)
            {
                IncreaseUnit();
            }

            BigInteger pow = BigInteger.Pow(1000, unitShift);
            BigInteger scaled = targetNumber / pow;   
            BigInteger remainder = targetNumber % pow; 

            if (scaled >= 1000)
            {
                scaled = 1;
                remainder = 0;
                IncreaseUnit();
            }

            BigInteger fractionTimes10 = (remainder * 10) / pow;
            int fractionDigit = (int)fractionTimes10; // 0~9

            m_StringNumber = $"{scaled}.{fractionDigit}";
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

         public static implicit operator AlphaUnit(BigInteger number) => new AlphaUnit(number);
        public static explicit operator BigInteger(AlphaUnit a) => a.m_OriginNumber;

        public static AlphaUnit operator +(AlphaUnit a, AlphaUnit b)
            => new AlphaUnit(a.m_OriginNumber + b.m_OriginNumber, true);
        public static AlphaUnit operator -(AlphaUnit a, AlphaUnit b)
            => new AlphaUnit(a.m_OriginNumber - b.m_OriginNumber, true);
        public static AlphaUnit operator *(AlphaUnit a, AlphaUnit b)
            => new AlphaUnit(a.m_OriginNumber * b.m_OriginNumber, true);
        public static AlphaUnit operator /(AlphaUnit a, AlphaUnit b)
            => new AlphaUnit(a.m_OriginNumber / b.m_OriginNumber, true);

        public static bool operator <(AlphaUnit a, AlphaUnit b) => a.CompareTo(b) < 0;
        public static bool operator >(AlphaUnit a, AlphaUnit b) => a.CompareTo(b) > 0;
        public static bool operator <=(AlphaUnit a, AlphaUnit b) => a.CompareTo(b) <= 0;
        public static bool operator >=(AlphaUnit a, AlphaUnit b) => a.CompareTo(b) >= 0;
        public static bool operator ==(AlphaUnit a, AlphaUnit b) => a.CompareTo(b) == 0;
        public static bool operator !=(AlphaUnit a, AlphaUnit b) => a.CompareTo(b) != 0;

        public static implicit operator AlphaUnit(int v)
        {
            return new AlphaUnit((BigInteger)v);
        }

        public static implicit operator AlphaUnit(double v)
        {
            return new AlphaUnit((BigInteger)v);
        }

        public string ToUnit()
        {
            Normalize();
            return m_StringNumber + m_StringBase;
        }

        public override string ToString()
            => m_OriginNumber.ToString();

        public override bool Equals(object obj)
        {
            if (obj is AlphaUnit other)
                return this == other;
            return false;
        }

        public override int GetHashCode() => m_OriginNumber.GetHashCode();

    } // class AlphaUnit
} // namespace SkyDragonHunter.Utility
