using Org.BouncyCastle.Security;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace SkyDragonHunter.Structs {

    public struct BigInt2
    {
        // const Fields
        private const int BaseVal = 1_000;

        private int[] m_Values;
        private char[] m_Units;
        private string m_Significance;
        private string m_StringNumber;
        private int m_Digits;

        // Properties
        private int UnitCount => (m_Digits - 1) / 3 + 1;
        public string StringNumber
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                for(int i = m_Values.Length - 1; i >= 0; --i)
                {
                    if(i != m_Values.Length - 1)
                    {
                        if (m_Values[i] < 100)
                            sb.Append('0');
                        if (m_Values[i] < 10)
                            sb.Append("0");
                    }
                    sb.Append(m_Values[i]);
                    if(i != 0)
                    {
                        sb.Append(",");
                    }
                }
                return sb.ToString();
            }
        }

        public BigInt2(double number)
        {
            m_Values = new int[1];
            m_StringNumber = "0";
            m_Significance = "0";
            m_Units = new char[1] { ' ' };
            m_Digits = 1;

            // No need to deal with value less than 1.
            if(number < 1)
            {
                return;
            }

            // 10x Digit of the input number will be cached in m_Digits.
            m_Digits = (int)Math.Floor(Math.Log10(number)) + 1;
            ResetUnitCharacter();
            if (m_Digits < 17)
            {
                HandleDoubleUnder17Digits(number);
                return;
            }
            HandleDoubleNumberOver17Digits(number);
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(m_Significance);
            foreach( var c in m_Units)
            {
                sb.Append(c);
            }
            return sb.ToString();
        }

        // Private Methods
        private void HandleDoubleUnder17Digits(double number)
        {
            long longNum = (long)Math.Floor(number);

            m_Values = new int[UnitCount];

            for (int i = 0; i < UnitCount; ++i)
            {
                m_Values[i] = (int)(longNum % BaseVal);
                longNum /= BaseVal;
            }

            if (m_Values.Length == 1)
            {
                m_Significance = m_Values[0].ToString();
            }
            else
            {
                int underDecimalCount = 2;
                if (m_Values[m_Values.Length - 1] < 100)
                    underDecimalCount--;
                if (m_Values[m_Values.Length - 1] < 10)
                    underDecimalCount--;

                StringBuilder significanceSB = new StringBuilder();
                significanceSB.Append(m_Values[m_Values.Length - 1]);
                significanceSB.Append('.');

                int cached = m_Values[m_Values.Length - 2];

                if (cached < 100)
                    significanceSB.Append('0');
                if (cached < 10)
                    significanceSB.Append('0');

                for (int i = 0; i < underDecimalCount; ++i)
                {
                    cached /= 10;
                }
                significanceSB.Append(cached);
                m_Significance = significanceSB.ToString();
            }

            Debug.Log($"Digits: {m_Digits}, Number = {number}, Num of Arrays {m_Values.Length}");
        }

        private void HandleDoubleNumberOver17Digits(double number)
        {
            double divisor = Math.Pow(10, m_Digits - 17);
            long frontDigits = (long)(number / divisor);
            
            m_Values = new int[UnitCount];
            int firstArrDigit = (m_Digits+2) % 3;
            int repeatCount = 6;
            if (firstArrDigit == 0)
                repeatCount = 7;
            int lastUnitIndex = UnitCount - 1;
            int underDecimalCount = 3 - firstArrDigit;

            // (n + 2) % 3 == 0 : 1자리
            // (n + 2) % 3 == 1 : 2자리
            // (n + 2) % 3 == 2 : 3자리

            StringBuilder significanceSB = new StringBuilder();

            for (int i = 0; i < repeatCount; ++i)
            {
                m_Values[lastUnitIndex - i] = (int)(frontDigits % BaseVal);
                
                // Set Significance in first two loop
                if(i == 0)
                {
                    significanceSB.Append(m_Values[lastUnitIndex - i]);
                    significanceSB.Append('.');
                }
                if(i == 1)
                {
                    for (int j = 0; j < underDecimalCount; ++j)
                    {
                        int cached = m_Values[lastUnitIndex - i];
                        for (int k = j; k < underDecimalCount; ++k)
                        {

                        }
                    }
                }


                

                // calibrate value of last index in int array with guaranteed precesion ( 17digits )
                if (i == repeatCount - 1)
                {
                    for (int j = 1; j < underDecimalCount; ++j)
                    {
                        m_Values[lastUnitIndex - i] *= 10;
                    }
                }
                frontDigits /= BaseVal;
            }
        }

        private void ResetUnitCharacter()
        {
            if (m_Digits < 4)
            {
                return;
            }
            int unitIndex = ((m_Digits - 4) / 3) + 1;
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

    } // Scope by class BigInt2

} // namespace Root