using System.Collections;
using System.Collections.Generic;
using System;
using System.Text;

namespace SkyDragonHunter.Utility
{

    public struct BigNum
    {
        // 필드 (Fields)
        private const int MaxArrayValue = 1_000_000_000;         // 10 ^ 9
        private const int MaxUnitValue = 1_000;             // 10 ^ 3
        private const double ReverseMax = 0.00_000_000_1;
        private const int DoubleDigits = 17;
        private const int AlphabetCount = 26;

        private int[] m_Values;
        private string m_StringNumber;
        private string m_Significance;
        private char[] m_Units;
        private int m_Digits;

        // 속성 (Properties)        
        public string StringNumber => m_StringNumber;
        // Public 메서드
        public BigNum(double number)
        {
            m_Values = new int[1];
            m_StringNumber = "0";
            m_Significance = "0";
            m_Units = new char[1] { ' ' };
            m_Digits = 1;
            if (number < 1)
            {
                return;
            }

            m_Digits = (int)Math.Floor(Math.Log10(number)) + 1;
            ResetUnitCharacter();
            int arrayShift = (m_Digits / 9);
            m_Values = new int[arrayShift + 1];
            double targetNumber = number * Math.Pow(ReverseMax, arrayShift);
            int[] significances = new int[2] { -1, -1 };
            int count = 0;
            for (int i = 0; i < arrayShift + 1; ++i)
            {
                int significance = (int)Math.Truncate(targetNumber);
                significances[i] = significance;
                m_Values[i] = significance;
                if (count++ > 0)
                {
                    break;
                }
                targetNumber -= significance;
                targetNumber *= MaxArrayValue;
            }
            m_Significance = significances[0].ToString();
            if (m_Significance.Length > 4)
            {
                m_Significance = m_Significance.Substring(0, 4);
            }
            else if (m_Significance.Length < 4)
            {
                int targetLength = 4 - m_Significance.Length;
                if (significances[1] != -1)
                {
                    string additional = significances[1].ToString("D10");
                    additional = additional.Substring(0, targetLength);
                    m_Significance += additional;
                }
                else
                {
                    for (int i = 0; i < targetLength; ++i)
                    {
                        m_Significance += "0";
                    }
                }
            }

            StringBuilder sb = new StringBuilder();            
            foreach(var value in m_Values)
            {
                sb.Append(value.ToString());
            }
            m_StringNumber = sb.ToString();

            int significanceDotIndex = (m_Digits + 1) % 4;
            if (significanceDotIndex != 0)
            {
                m_Significance.Insert(significanceDotIndex, ".");
            }
        }

        public override string ToString()
        {
            string units = "";
            foreach(var c in m_Units)
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
            int unitIndex = ((m_Digits - 4) / 4) + 1;
            m_Units = GetUnitAlphabetArray(unitIndex);
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