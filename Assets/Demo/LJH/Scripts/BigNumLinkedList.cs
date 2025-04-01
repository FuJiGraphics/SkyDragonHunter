using System.Collections;
using System.Collections.Generic;
using System.Text;
using System;

namespace SkyDragonHunter {

    public struct BigNumLinkedList
    {
        // 필드 (Fields)
        private const int MaxUnitValue = 1_000;             // 10 ^ 3
        private const double ReverseMax = 0.00_000_000_1;
        private const int DoubleDigits = 17;
        private const int AlphabetCount = 26;

        private LinkedList<int> m_Values;
        private string m_StringNumber;
        private string m_Significance;
        private char[] m_Units;
        private int m_Digits;
        private int m_UnitCount;

        // 속성 (Properties)
        public string StringNumber => m_StringNumber;
        // Public 메서드
        public BigNumLinkedList(double number)
        {
            m_Values = new LinkedList<int>();
            m_Values.AddLast(0);
            m_StringNumber = "0";
            m_Significance = "0";
            m_Units = new char[1] { ' ' };
            m_Digits = 1;
            m_UnitCount = 1;
            if (number < 1)
            {
                return;
            }

            m_Digits = (int)Math.Floor(Math.Log10(number)) + 1;
            ResetUnitCharacter();

            int unitShift = (m_Digits / 3);
            

        }

        public override string ToString()
        {
            string units = "";
            foreach (var c in m_Units)
            {
                units += c;
            }

            return m_Significance + units;
        }

        // Private 메서드       
        private void ResetUnitCharacter()
        {
            if (m_Digits < 4)
            {
                return;
            }
            m_UnitCount = ((m_Digits - 4) / 4) + 1;
            m_Units = GetUnitAlphabetArray(m_UnitCount);
        }

        private char[] GetUnitAlphabetArray(int unitIndex)
        {
            int cachedIndex = unitIndex;

            int length = 0;
            int temp = cachedIndex;
            while (temp > 0)
            {
                length++;
                temp = (temp - 1) / 26;
            }

            char[] newUnits = new char[length];

            for (int i = length - 1; i >= 0; i--)
            {
                int remainder = (cachedIndex - 1) % 26;
                newUnits[i] = (char)('a' + remainder);
                cachedIndex = (cachedIndex - 1) / 26;
            }
            return newUnits;
        }

        // Others

    } // Scope by class BigNumber

} // namespace Root