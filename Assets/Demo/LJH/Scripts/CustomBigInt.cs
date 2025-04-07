using Org.BouncyCastle.Security;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

namespace SkyDragonHunter.Structs {

    public struct CustomBigInt : IComparable<CustomBigInt>, IEquatable<CustomBigInt>
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
        public string StringNumber => m_StringNumber;

        // Public Methods
        public CustomBigInt(double number)
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
            m_Units = GetUnitAlphabetArray(UnitCount);
            if (m_Digits < 17)
            {
                HandleDoubleDigits1to16(number);
                return;
            }
            HandleDoubleDigitsOver17(number);
        }

        public CustomBigInt(int number)
        {
            var result = new CustomBigInt(number.ToString());
            this = result;
        }

        public CustomBigInt(string stringNum)
        {
            StringBuilder sb = new StringBuilder();
            foreach(var c in stringNum)
            {
                if (char.IsDigit(c))
                    sb.Append(c);
            }
            m_StringNumber = sb.ToString().TrimStart('0');
            if(m_StringNumber.Length == 0)
            {
                m_StringNumber = "0";
                m_Significance = "0";
                m_Units = new char[1] { ' ' };
                m_Values = new int[1];
                m_Digits = 1;
                Debug.LogError($"Invalid string value, returned Bignumber with 0 value");
                return;
            }

            m_Digits = m_StringNumber.Length;

            m_Values = new int[(m_Digits - 1) / 3 + 1];
            int unitIndex = 0;
            for (int i = m_StringNumber.Length; i > 0; i -= 3)
            {
                int start = Math.Max(i - 3, 0);
                int length = i - start;
                string unitString = m_StringNumber.Substring(start, length);
                m_Values[unitIndex] = int.Parse(unitString);
                unitIndex++;
            }
            m_Units = GetUnitAlphabetArray((m_Digits - 1) / 3 + 1);
            m_Significance = GetSignificance(m_Values);
        }

        public CustomBigInt(CustomBigInt other)
        {
            m_Values = other.m_Values;
            m_StringNumber = other.m_StringNumber;
            m_Significance = other.m_Significance;
            m_Units = other.m_Units;
            m_Digits = other.m_Digits;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(m_Significance);
            foreach (var c in m_Units)
            {
                sb.Append(c);
            }
            return sb.ToString().TrimEnd(' ');
        }

        public int CompareTo(CustomBigInt other)
        {
            if (m_Digits > other.m_Digits)
                return 1;
            else if (other.m_Digits > m_Digits)
                return -1;
            for (int i = UnitCount - 1; i >= 0; --i)
            {
                if (m_Values[i] > other.m_Values[i])
                    return 1;
                else if (other.m_Values[i] > m_Values[i])
                    return -1;
            }
            return 0;
        }

        public override bool Equals(object obj)
        {
            if (obj is CustomBigInt other)
                return this == other;
            return false;
        }

        public override int GetHashCode() => m_StringNumber.GetHashCode();

        public bool Equals(CustomBigInt other) => m_StringNumber == other.m_StringNumber;

        // Private Methods
        private void HandleDoubleDigits1to16(double number)
        {
            long longNum = (long)Math.Floor(number);
            m_StringNumber = longNum.ToString("N0");
            m_Values = new int[UnitCount];

            for (int i = 0; i < UnitCount; ++i)
            {
                m_Values[i] = (int)(longNum % BaseVal);
                longNum /= BaseVal;
            }

            if (m_Values.Length == 1)
            {
                m_Significance = m_Values[0].ToString("N0");
            }
            else
            {
                int underDecimalCount = 1;
                if (m_Values[m_Values.Length - 1] < 100)
                    underDecimalCount++;
                if (m_Values[m_Values.Length - 1] < 10)
                    underDecimalCount++;

                StringBuilder significanceSB = new StringBuilder();
                significanceSB.Append(m_Values[m_Values.Length - 1]);
                significanceSB.Append('.');
                string decimalPart = m_Values[m_Values.Length - 2].ToString("D3");
                significanceSB.Append(decimalPart.Substring(0, underDecimalCount));
                m_Significance = significanceSB.ToString();
            }
        }

        private void HandleDoubleDigitsOver17(double number)
        {
            double deminished = number;
            for (int i = m_Digits - 17; i > 0; --i)
            {
                deminished /= 10;
            }
            long frontDigits = (long)Math.Floor(deminished);
            int unitCnt = UnitCount;
            m_Values = new int[unitCnt];
            int firstArrDigit = m_Digits % 3;

            if (firstArrDigit == 1)
            {
                frontDigits /= 10;
            }
            else if (firstArrDigit == 0)
            {
                frontDigits *= 10;
            }            
            for (int i = unitCnt - 6; i < unitCnt; ++i)
            {
                m_Values[i] = (int)(frontDigits % BaseVal);
                frontDigits /= BaseVal;
            }
            m_StringNumber = GetStringNumber(m_Values);
            m_Significance = GetSignificance(m_Values);
        }
        private static string GetStringNumber(int[] values)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(values[values.Length - 1]);
            for (int i = values.Length - 2; i >= 0; --i)
            {
                if (values[i] < 100)
                    sb.Append('0');
                if (values[i] < 10)
                    sb.Append("0");
                sb.Append(values[i]);
            }
            return sb.ToString();
        }

        private static string GetSignificance(int[] values)
        {
            int length = values.Length;
            if(length < 2)
            {
                return values[0].ToString();
            }
            StringBuilder sb = new StringBuilder();
            int underDecimalCount = 1;
            if (values[length - 1] < 100)
                underDecimalCount++;
            if (values[length - 1] < 10)
                underDecimalCount++;

            sb.Append($"{values[length - 1]}.");
            string decimalPart = values[length - 2].ToString("D3");
            sb.Append(decimalPart.Substring(0, underDecimalCount));
            return sb.ToString();
        }

        private static char[] GetUnitAlphabetArray(int unitIndex)
        {
            if(unitIndex <= 1)
            {
                return new char[1] { ' ' };
            }
            int cachedIndex = unitIndex - 1;
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
        
        private static int[] GetAddedArray(CustomBigInt a, CustomBigInt b, out int digit)
        {
            int unitSizeA = a.m_Values.Length;
            for (int i = 0; i < b.m_Values.Length; ++i)
            {   
                // Add from smaller to bigger value
                a.m_Values[i] += b.m_Values[i];

                // Handle units going over BaseValue '1,000'
                if (a.m_Values[i] >= BaseVal)
                {
                    // Making sure not to access invalid index, return new bigger array.
                    if (i == unitSizeA - 1)
                    {
                        var newArr = new int[a.m_Values.Length + 1];
                        for (int j = 0; j < newArr.Length - 1; ++j)
                        {
                            newArr[j] = a.m_Values[j];
                        }

                        // using reverse index '[^1]' could be risky if built in IL2CPP
                        newArr[newArr.Length - 1] = newArr[newArr.Length - 2] / BaseVal;
                        newArr[newArr.Length - 2] %= BaseVal;
                        digit = newArr.Length * 3 - GetDigitCalibrator(newArr[newArr.Length - 1]);
                        return newArr;
                    }
                    else
                    {
                        a.m_Values[i + 1] += a.m_Values[i] / BaseVal;
                        a.m_Values[i] %= BaseVal;
                    }
                }
            }
            digit = unitSizeA * 3 - GetDigitCalibrator(a.m_Values[unitSizeA - 1]);
            return a.m_Values;
        }

        private static int[] GetMultipliedArray(CustomBigInt a, CustomBigInt b, out int digit)
        {
            int unitSizeA = a.m_Values.Length;
            int unitSizeB = b.m_Values.Length;
            int newUnitSize = (unitSizeA - 1) + (unitSizeB - 1) + 1;
            int[] values = new int[newUnitSize];

            for(int i = 0; i < unitSizeB; ++i)
            {
                for(int j = 0; j < unitSizeA; ++j)
                {
                    int unitIndex = i + j;
                    values[unitIndex] += a.m_Values[j] * b.m_Values[i];
                }
            }

            for(int i = 0; i < newUnitSize - 1; ++i)
            {
                if(values[i] >= BaseVal)
                {
                    values[i + 1] = values[i] / BaseVal;
                    values[i] %= BaseVal;
                }
            }
            // Check First Unit Val
            if (values[values.Length - 1] >= BaseVal)
            {
                int[] newValues = new int[values.Length + 1];
                for(int i = 0; i < values.Length; ++i)
                {
                    newValues[i] = values[i];
                }
                newValues[newValues.Length - 1] = newValues[newValues.Length - 2] / BaseVal;
                newValues[newValues.Length - 2] %= BaseVal;
                digit = newValues.Length * 3 - GetDigitCalibrator(newValues[newValues.Length - 1]);
                return newValues;

            }

            digit = values.Length * 3 - GetDigitCalibrator(values[values.Length - 1]);
            return values;
        }

        private static int[] GetSubtractedArray(CustomBigInt a, CustomBigInt b, out int digit)
        {
            for (int i = 0; i < a.UnitCount; ++i)
            {
                a.m_Values[i] -= b.m_Values[i];
                if (a.m_Values[i] < 0)
                {
                    a.m_Values[i + 1]--;
                    a.m_Values[i] += BaseVal;
                }
            }
            int index = a.m_Values.Length - 1;
            while (a.m_Values[index] == 0)
            {
                index--;
            }
            var result = new int[index + 1];
            for (int i = 0; i < index + 1; ++i)
            {
                result[i] = a.m_Values[i];
            }
            digit = (index + 1) * 3 - GetDigitCalibrator(a.m_Values[index]);
            return result;
        }

        private static int GetDigitCalibrator(int lastUnitVal)
        {
            int digitCalibrator = 2;
            while (lastUnitVal >= 10)
            {
                lastUnitVal /= 10;
                digitCalibrator--;
            }
            return digitCalibrator;
        }

        // Operators  
        public static CustomBigInt operator +(CustomBigInt a, CustomBigInt b)
        {
            CustomBigInt result = new CustomBigInt(0);
            if (a.UnitCount.CompareTo(b.UnitCount) >= 0)
            {
                result.m_Values = GetAddedArray(a, b, out result.m_Digits);
            }
            else
            {
                result.m_Values = GetAddedArray(b, a, out result.m_Digits);
            }
            // TODO: TEST ONLY
            StringBuilder sb = new StringBuilder();
            for (int i = result.m_Values.Length - 1; i >=0; --i)
            {
                sb.Append(result.m_Values[i].ToString("D3"));
            }
            Debug.Log($"added arr value {sb}");
            result.m_Significance = GetSignificance(result.m_Values);
            result.m_StringNumber = GetStringNumber(result.m_Values);
            result.m_Units = GetUnitAlphabetArray(result.UnitCount);
            StringBuilder sb2 = new StringBuilder();
            foreach( var c in result.m_Units )
            {
                sb2.Append(c);
            }            
            Debug.LogError($"result unitCount: {result.UnitCount}, unitChar: {sb2}");
            return result;
        }

        public static CustomBigInt operator -(CustomBigInt a, CustomBigInt b)
        {
            // TODO : Not Supporting Negative Values
            if(a <= b)
            {
                return new CustomBigInt(0);
            }
            CustomBigInt result = new CustomBigInt(0);
            result.m_Values = GetSubtractedArray(a, b, out result.m_Digits);
            result.m_Units = GetUnitAlphabetArray(result.UnitCount);
            result.m_StringNumber = GetStringNumber(result.m_Values);
            result.m_Significance = GetSignificance(result.m_Values);
            return result;
        }

        public static CustomBigInt operator *(CustomBigInt a, CustomBigInt b)
        {
            CustomBigInt result = new CustomBigInt(0);
            result.m_Values = GetMultipliedArray(a, b, out result.m_Digits);
            // TODO: TEST ONLY
            StringBuilder sb = new StringBuilder();
            for (int i = result.m_Values.Length - 1; i >= 0; --i)
            {
                sb.Append(result.m_Values[i].ToString("D3"));
            }
            result.m_Significance = GetSignificance(result.m_Values);
            result.m_StringNumber = GetStringNumber(result.m_Values);
            result.m_Units = GetUnitAlphabetArray(result.UnitCount);
            StringBuilder sb2 = new StringBuilder();
            foreach (var c in result.m_Units)
            {
                sb2.Append(c);
            }
            return result;
        }

        // Operators
        public static implicit operator CustomBigInt(int number) => new CustomBigInt(number);
        public static implicit operator CustomBigInt(double number) => new CustomBigInt(number);        

        public static bool operator >(CustomBigInt a, CustomBigInt b) => a.CompareTo(b) > 0;
        public static bool operator >=(CustomBigInt a, CustomBigInt b) => a.CompareTo(b) >= 0;
        public static bool operator <(CustomBigInt a, CustomBigInt b) => a.CompareTo(b) < 0;
        public static bool operator <=(CustomBigInt a, CustomBigInt b) => a.CompareTo(b) <= 0;
        public static bool operator ==(CustomBigInt a, CustomBigInt b) => a.CompareTo(b) == 0;
        public static bool operator !=(CustomBigInt a, CustomBigInt b) => a.CompareTo(b) != 0;
    } // Scope by class BigInt2

} // namespace Root