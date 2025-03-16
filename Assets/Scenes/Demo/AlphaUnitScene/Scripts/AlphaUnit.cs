using System;

namespace SkyDragonHunter.Test
{
    public struct AlphaUnit
        : IComparable<AlphaUnit>
    {
        private double m_OriginNumber;
        private string m_StringNumber;
        private string m_StringBase;
        private char[] m_Base;
        private bool m_IsInfinity;
        private bool m_IsNaN;

        public AlphaUnit(double number)
        {
            m_OriginNumber = number;
            m_StringNumber = "0";
            m_StringBase = "";
            m_Base = new char[1];
            m_Base[0] = (char)('a' - 1);
            m_IsInfinity = false;
            m_IsNaN = false;
            if (double.IsNaN(m_OriginNumber))
                m_IsNaN = true;
            else if (double.IsInfinity(m_OriginNumber))
                m_IsInfinity = true;
            else
                this.Normalize();
        }

        private void Normalize()
        {
            double targetNumber = m_OriginNumber;
            while (targetNumber >= 1000.0)
            {
                targetNumber /= 1000.0;
                this.IncreaseUnit();
            }
            m_StringNumber = targetNumber.ToString("G3");
            m_StringBase = this.GetBaseString();
        }

        private void IncreaseUnit()
        {
            int index = 0;
            while (index < m_Base.Length)
            {
                this.Resize(index);
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

        // 인덱스가 현재 할당된 크기보다 클 경우
        // 새로운 크기를 할당 받습니다.
        private void Resize(int index)
        {
            if (m_Base == null || index < m_Base.Length)
                return;

            int newSize = index + 1;
            char[] newBase = new char[newSize];
            for (int i = 0; i < newBase.Length; ++i)
            {
                if (i < m_Base.Length)
                    newBase[i] = m_Base[i];
                else
                    newBase[i] = (char)('a' - 1);
            }
            m_Base = newBase;
        }

        private string GetBaseString()
        {
            if (m_Base[0] < 'a')
                return "";

            char[] result = null;
            char[] newStr = new char[m_Base.Length];
            int validCount = 0;
            for (int i = 0; i < m_Base.Length; ++i)
            {
                if (m_Base[i] >= 'a' && m_Base[i] <= 'z')
                {
                    newStr[i] = m_Base[i];
                    validCount++;
                }
                else
                    break;
            }
            result = new char[validCount];
            for (int i = 0; i < validCount; ++i)
            {
                result[i] = newStr[i];
            }
            return new string(result);
        }

        public int CompareTo(AlphaUnit other)
        {
            return m_OriginNumber.CompareTo(other.m_OriginNumber);
        }

        public static implicit operator AlphaUnit(double number)
            => new AlphaUnit(number);

        public static explicit operator double(AlphaUnit a)
            => a.m_OriginNumber;

        public static AlphaUnit operator +(AlphaUnit a, AlphaUnit b)
            => new AlphaUnit(a.m_OriginNumber + b.m_OriginNumber);

        public static AlphaUnit operator -(AlphaUnit a, AlphaUnit b)
            => new AlphaUnit(a.m_OriginNumber - b.m_OriginNumber);

        public static AlphaUnit operator *(AlphaUnit a, AlphaUnit b)
            => new AlphaUnit(a.m_OriginNumber * b.m_OriginNumber);

        public static AlphaUnit operator /(AlphaUnit a, AlphaUnit b)
            => new AlphaUnit(a.m_OriginNumber / b.m_OriginNumber);

        public static bool operator <(AlphaUnit a, AlphaUnit b) => a.CompareTo(b) < 0;
        public static bool operator >(AlphaUnit a, AlphaUnit b) => a.CompareTo(b) > 0;
        public static bool operator <=(AlphaUnit a, AlphaUnit b) => a.CompareTo(b) <= 0;
        public static bool operator >=(AlphaUnit a, AlphaUnit b) => a.CompareTo(b) >= 0;
        public static bool operator ==(AlphaUnit a, AlphaUnit b) => a.CompareTo(b) == 0;
        public static bool operator !=(AlphaUnit a, AlphaUnit b) => a.CompareTo(b) != 0;

        public override string ToString()
        {
            if (m_IsInfinity)
                return "Inf";
            else if (m_IsNaN)
                return "NaN";
            else
                return m_StringNumber + m_StringBase;
        }

        public override bool Equals(object obj)
        {
            if (obj is AlphaUnit other)
                return this == other;
            return false;
        }

        public override int GetHashCode()
            => m_OriginNumber.GetHashCode();

    } // class AlphaUnit
} // namespace SkyDragonHunter.Utility
