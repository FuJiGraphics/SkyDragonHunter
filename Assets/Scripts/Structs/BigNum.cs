using System;
using System.Text;
using UnityEngine;

namespace SkyDragonHunter.Structs
{

    [Serializable]
    public struct BigNum : IComparable<BigNum>, IEquatable<BigNum>, ISerializationCallbackReceiver
    {
        // const Fields
        private const int BaseVal = 1_000;
        private int[] m_Values;
        private char[] m_Units;
        private string m_Significance;
        private int m_Digits;

        [SerializeField]
        private string m_StringNumber;

        // Properties
        private int UnitCount => (m_Digits - 1) / 3 + 1;
        public string StringNumber => m_StringNumber;

        // Public Methods
        public BigNum(double number)
        {
            m_Values = new int[1];
            m_StringNumber = "0";
            m_Significance = "0";
            m_Units = new char[1] { ' ' };
            m_Digits = 1;

            // No need to deal with value less than 1.
            if (number < 1)
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

        public BigNum(int number)
        {
            var temp = new BigNum(number.ToString());

            m_Digits = temp.m_Digits;
            m_StringNumber = temp.m_StringNumber;
            m_Significance = temp.m_Significance;
            m_Values = (int[])temp.m_Values.Clone();
            m_Units = (char[])temp.m_Units.Clone();
        }

        public BigNum(string stringNum)
        {
            m_Values = null;
            m_Units = null;
            m_Significance = "";
            m_StringNumber = "";
            m_Digits = 0;
            InitializeFromString(stringNum);
        }

        public BigNum(BigNum other)
        {
            m_Digits = other.m_Digits;
            m_StringNumber = other.m_StringNumber;
            m_Significance = other.m_Significance;

            // 배열은 Clone()으로 복제하여 원본 배열을 건드리지 않도록 함
            m_Values = (int[])other.m_Values.Clone();
            m_Units = (char[])other.m_Units.Clone();
        }

        public string ToUnit()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(m_Significance);
            foreach (var c in m_Units)
            {
                sb.Append(c);
            }
            return sb.ToString().TrimEnd(' ');
        }
        public override string ToString()
        {
            return m_StringNumber;
        }

        public int CompareTo(BigNum other)
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
            if (obj is BigNum other)
                return this == other;
            return false;
        }

        public override int GetHashCode() => m_StringNumber.GetHashCode();

        public bool Equals(BigNum other) => m_StringNumber == other.m_StringNumber;

        public static float GetPercentage(BigNum numerator, BigNum denominator)
        {
            float numSig = float.Parse(numerator.m_Significance);
            float denomSig = float.Parse(denominator.m_Significance);
            while (numSig >= 10f)
            {
                numSig /= 10f;
            }
            while (denomSig >= 10f)
            {
                denomSig /= 10f;
            }
            //Debug.Log($"numerator : {numSig}E+{numerator.m_Digits-1}, " +
            //    $"denominator : {denomSig}E+{denominator.m_Digits-1}");

            float newSig = numSig / denomSig;
            int newDigit = numerator.m_Digits - denominator.m_Digits;

            while (newDigit > 0)
            {
                newSig *= 10f;
                newDigit--;
            }
            while (newDigit < 0)
            {
                newSig /= 10f;
                newDigit++;
            }
            
            return newSig;
        }

        public void OnBeforeSerialize()
        {
            if (m_Values == null || m_Values.Length == 0)
            {
                m_StringNumber = string.Empty;   // 또는 "0" 등 원하는 기본값
            }
            m_StringNumber = GetStringNumber(m_Values);
        }

        public void OnAfterDeserialize()
        {
            InitializeFromString(m_StringNumber);
        }

        // Private Methods
        private void InitializeFromString(string stringNum)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var c in stringNum)
            {
                if (char.IsDigit(c))
                    sb.Append(c);
            }
            m_StringNumber = sb.ToString().TrimStart('0');
            if (m_StringNumber.Length == 0)
            {
                m_StringNumber = "0";
                m_Significance = "0";
                m_Units = new char[1] { ' ' };
                m_Values = new int[1];
                m_Digits = 1;
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
            m_Significance = GetSignificance(m_Values);
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
            if (values == null || values.Length == 0)
                return "";
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
            return sb.ToString().TrimStart('0');
        }

        private static string GetSignificance(int[] values)
        {
            int length = values.Length;
            if (length < 2)
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
            if (unitIndex <= 1)
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

        private static int[] GetAddedArray(BigNum a, BigNum b, out int digit)
        {
            //int unitSizeA = a.m_Values.Length;
            //for (int i = 0; i < b.m_Values.Length; ++i)
            //{
            //    // Add from smaller to bigger value
            //    a.m_Values[i] += b.m_Values[i];
            //
            //    // Handle units going over BaseValue '1,000'
            //    if (a.m_Values[i] >= BaseVal)
            //    {
            //        // Making sure not to access invalid index, return new bigger array.
            //        if (i == unitSizeA - 1)
            //        {
            //            var newArr = new int[a.m_Values.Length + 1];
            //            for (int j = 0; j < newArr.Length - 1; ++j)
            //            {
            //                newArr[j] = a.m_Values[j];
            //            }
            //
            //            // using reverse index '[^1]' could be risky if built in IL2CPP
            //            newArr[newArr.Length - 1] = newArr[newArr.Length - 2] / BaseVal;
            //            newArr[newArr.Length - 2] %= BaseVal;
            //            digit = newArr.Length * 3 - GetDigitCalibrator(newArr[newArr.Length - 1]);
            //            return newArr;
            //        }
            //        else
            //        {
            //            a.m_Values[i + 1] += a.m_Values[i] / BaseVal;
            //            a.m_Values[i] %= BaseVal;
            //        }
            //    }
            //}
            //digit = unitSizeA * 3 - GetDigitCalibrator(a.m_Values[unitSizeA - 1]);
            //return a.m_Values;


            int aLen = a.m_Values.Length;
            int bLen = b.m_Values.Length;
            // 최대 단위 수 +1 (캐리 가능성)
            int maxLen = Math.Max(aLen, bLen);
            int[] temp = new int[maxLen + 1];

            // 1) a의 값 복사
            for (int i = 0; i < aLen; i++)
                temp[i] = a.m_Values[i];

            // 2) b를 더함
            for (int i = 0; i < bLen; i++)
                temp[i] += b.m_Values[i];

            // 3) 캐리 처리
            for (int i = 0; i < temp.Length - 1; i++)
            {
                if (temp[i] >= BaseVal)
                {
                    temp[i + 1] += temp[i] / BaseVal;
                    temp[i] %= BaseVal;
                }
            }

            // 4) 실제 사용되는 마지막 인덱스 결정
            int lastIndex = temp[temp.Length - 1] > 0
                ? temp.Length - 1
                : temp.Length - 2;

            // 5) 결과 배열로 복사
            int[] result = new int[lastIndex + 1];
            Array.Copy(temp, result, lastIndex + 1);

            // 6) digit 계산: (유닛 개수 * 3) – 자리 보정
            digit = result.Length * 3
                  - GetDigitCalibrator(result[result.Length - 1]);

            return result;
        }

        private static int[] GetMultipliedArray(BigNum a, BigNum b, out int digit)
        {
            int unitSizeA = a.m_Values.Length;
            int unitSizeB = b.m_Values.Length;
            int newUnitSize = (unitSizeA - 1) + (unitSizeB - 1) + 1;
            int[] values = new int[newUnitSize];

            for (int i = 0; i < unitSizeB; ++i)
            {
                for (int j = 0; j < unitSizeA; ++j)
                {
                    int unitIndex = i + j;
                    values[unitIndex] += a.m_Values[j] * b.m_Values[i];
                }
            }

            StringBuilder sb = new StringBuilder();
            for (int i = values.Length - 1; i >= 0; --i)
            {
                sb.Append(values[i].ToString("D3"));
                sb.Append(',');
            }

            for (int i = 0; i < newUnitSize - 1; ++i)
            {
                if (values[i] >= BaseVal)
                {
                    values[i + 1] += values[i] / BaseVal;
                    values[i] %= BaseVal;
                }
            }

            sb = new StringBuilder();
            for (int i = values.Length - 1; i >= 0; --i)
            {
                sb.Append(values[i].ToString("D3"));
                sb.Append(',');
            }

            // Check First Unit Val
            if (values[values.Length - 1] >= BaseVal)
            {
                int[] newValues = new int[values.Length + 1];
                for (int i = 0; i < values.Length; ++i)
                {
                    newValues[i] = values[i];
                }
                newValues[newValues.Length - 1] = newValues[newValues.Length - 2] / BaseVal;
                newValues[newValues.Length - 2] %= BaseVal;
                digit = newValues.Length * 3 - GetDigitCalibrator(newValues[newValues.Length - 1]);
                return newValues;
            }

            if (values[values.Length - 1] == 0)
            {
                int[] newValues = new int[values.Length - 1];
                for (int i = 0; i < newValues.Length; ++i)
                {
                    newValues[i] = values[i];
                }
                digit = newValues.Length * 3 - GetDigitCalibrator(newValues[newValues.Length - 1]);
                return newValues;
            }

            digit = values.Length * 3 - GetDigitCalibrator(values[values.Length - 1]);
            return values;
        }

        private static int[] GetSubtractedArray(BigNum a, BigNum b, out int digit)
        {
            //for (int i = 0; i < b.UnitCount; ++i)
            //{
            //    a.m_Values[i] -= b.m_Values[i];
            //}
            //for (int i = 0; i < a.UnitCount; ++i)
            //{
            //    if (i != a.UnitCount - 1)
            //    {
            //        while (a.m_Values[i] < 0)
            //        {
            //            a.m_Values[i + 1]--;
            //            a.m_Values[i] += BaseVal;
            //        }
            //    }
            //}
            //
            //int index = a.m_Values.Length - 1;
            //while (a.m_Values[index] <= 0)
            //{
            //    index--;
            //}
            //var result = new int[index + 1];
            //for (int i = 0; i < index + 1; ++i)
            //{
            //    result[i] = a.m_Values[i];
            //}
            //digit = (index + 1) * 3 - GetDigitCalibrator(a.m_Values[index]);
            //return result;
            // 1) 원본 a.m_Values 복제
            int[] temp = (int[])a.m_Values.Clone();
            int unitCountA = temp.Length;

            // 2) 뺄셈 수행 (복제한 temp만 수정)
            for (int i = 0; i < b.UnitCount; ++i)
            {
                temp[i] -= b.m_Values[i];
            }

            // 3) borrow 처리: 음수가 나올 경우 다음 유닛에서 빌려옴
            for (int i = 0; i < unitCountA - 1; ++i)
            {
                if (temp[i] < 0)
                {
                    temp[i + 1] -= 1;
                    temp[i] += BaseVal;
                }
            }

            // 4) 최상위 0유닛 제거: 마지막으로 값이 0이 아닌 인덱스 찾기
            int lastIndex = unitCountA - 1;
            while (lastIndex > 0 && temp[lastIndex] == 0)
            {
                lastIndex--;
            }

            // 5) 결과 배열 생성 및 복사
            int[] result = new int[lastIndex + 1];
            Array.Copy(temp, result, lastIndex + 1);

            // 6) digit 계산: (유닛 개수 * 3) – calibrator
            digit = result.Length * 3 - GetDigitCalibrator(result[result.Length - 1]);

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
        public static BigNum operator +(BigNum a, BigNum b)
        {
            //BigNum result = new BigNum(0);
            //if (a.UnitCount.CompareTo(b.UnitCount) >= 0)
            //{
            //    result.m_Values = GetAddedArray(a, b, out result.m_Digits);
            //}
            //else
            //{
            //    result.m_Values = GetAddedArray(b, a, out result.m_Digits);
            //}
            //// TODO: TEST ONLY
            //StringBuilder sb = new StringBuilder();
            //for (int i = result.m_Values.Length - 1; i >= 0; --i)
            //{
            //    sb.Append(result.m_Values[i].ToString("D3"));
            //}
            //result.m_Significance = GetSignificance(result.m_Values);
            //result.m_StringNumber = GetStringNumber(result.m_Values);
            //result.m_Units = GetUnitAlphabetArray(result.UnitCount);
            //StringBuilder sb2 = new StringBuilder();
            //foreach (var c in result.m_Units)
            //{
            //    sb2.Append(c);
            //}
            //return result;
            // a의 깊은 복제본을 만들어서 result에 담음
            BigNum result = new BigNum(a);

            if (a.UnitCount >= b.UnitCount)
            {
                result.m_Values = GetAddedArray(result, b, out result.m_Digits);
            }
            else
            {
                result.m_Values = GetAddedArray(b, result, out result.m_Digits);
            }

            // 연산 후 문자열/유닛 정보 갱신
            result.m_Significance = GetSignificance(result.m_Values);
            result.m_StringNumber = GetStringNumber(result.m_Values);
            result.m_Units = GetUnitAlphabetArray(result.UnitCount);

            return result;
        }

        public static BigNum operator -(BigNum a, BigNum b)
        {
            //// TODO : Not Supporting Negative Values
            //if (a <= b)
            //{
            //    return new BigNum(0);
            //}
            //BigNum result = new BigNum(0);
            //result.m_Values = GetSubtractedArray(a, b, out result.m_Digits);
            //result.m_Units = GetUnitAlphabetArray(result.UnitCount);
            //result.m_StringNumber = GetStringNumber(result.m_Values);
            //result.m_Significance = GetSignificance(result.m_Values);
            //return result;

            if (a <= b)
                return new BigNum(0);

            // ① 원본 a를 깊은 복제
            BigNum result = new BigNum(a);

            // ② 복제된 result.m_Values만 수정
            result.m_Values = GetSubtractedArray(result, b, out result.m_Digits);
            result.m_Units = GetUnitAlphabetArray(result.UnitCount);
            result.m_StringNumber = GetStringNumber(result.m_Values);
            result.m_Significance = GetSignificance(result.m_Values);
            return result;
        }

        public static BigNum operator *(BigNum a, BigNum b)
        {
            if (a == 0 || b == 0)
            {
                return new BigNum(0);
            }

            BigNum result = new BigNum(0);
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

        public static BigNum operator *(int a, BigNum b) => b * a;

        public static BigNum operator *(BigNum a, int b) => a * new BigNum(b);

        public static BigNum operator *(float a, BigNum b)
        {
            return b * a;
        }

        public static BigNum operator *(BigNum a, float b)
        {
            int newDigit = a.m_Digits;
            float newSig = float.Parse(a.m_Significance);

            // make sigValue into single digit number.
            while (newSig >= 10)
            {
                newSig /= 10;
            }

            newSig *= b;

            // Calibrate digits
            while (newSig >= 10)
            {
                newSig /= 10;
                newDigit++;
            }
            while (newSig < 1f)
            {
                newSig *= 10;
                newDigit--;
            }

            // Handle Default Value
            if (newDigit < 1)
                return new BigNum(0);


            // Handle upto 4 ditis;
            if (newDigit < 4)
            {
                while (newDigit > 1)
                {
                    newSig *= 10;
                    newDigit--;
                }
                return new BigNum(newSig);
            }

            newSig *= BaseVal;
            StringBuilder sb = new StringBuilder();
            sb.Append((int)Math.Floor(newSig));
            int newSigInt = (int)Math.Floor(newSig);
            while (newDigit > 4)
            {
                sb.Append('0');
                newDigit--;
            }
            return new BigNum(sb.ToString());
        }

        public static BigNum operator *(double a,  BigNum b)
        {
            return b * a;
        }

        public static BigNum operator *(BigNum a, double b)
        {
            int newDigit = a.m_Digits;
            double newSig = float.Parse(a.m_Significance);

            // make sigValue into single digit number.
            while (newSig >= 10)
            {
                newSig /= 10;
            }

            newSig *= b;

            // Calibrate digits
            while (newSig >= 10)
            {
                newSig /= 10;
                newDigit++;
            }
            while (newSig < 1f)
            {
                newSig *= 10;
                newDigit--;
            }

            // Handle Default Value
            if (newDigit < 1)
                return new BigNum(0);


            // Handle upto 4 ditis;
            if (newDigit < 4)
            {
                while (newDigit > 1)
                {
                    newSig *= 10;
                    newDigit--;
                }
                return new BigNum(newSig);
            }

            newSig *= BaseVal;
            StringBuilder sb = new StringBuilder();
            sb.Append((int)Math.Floor(newSig));
            int newSigInt = (int)Math.Floor(newSig);
            while (newDigit > 4)
            {
                sb.Append('0');
                newDigit--;
            }
            return new BigNum(sb.ToString());
        }

        public static BigNum operator /(BigNum a, BigNum b)
        {
            float numSig = float.Parse(a.m_Significance);
            float denomSig = float.Parse(b.m_Significance);
            while (numSig >= 10f)
            {
                numSig /= 10f;
            }
            while (denomSig >= 10f)
            {
                denomSig /= 10f;
            }

            float newSig = numSig / denomSig;
            int newDigit = a.m_Digits - b.m_Digits + 1;

            while (newSig >= 10)
            {
                newSig /= 10;
                newDigit++;
            }
            while (newSig < 1f)
            {
                newSig *= 10;
                newDigit--;
            }

            if (newDigit < 1)
                return new BigNum(0);

            // Handle upto 4 ditis;
            if (newDigit < 4)
            {
                while (newDigit > 1)
                {
                    newSig *= 10;
                    newDigit--;
                }
                return new BigNum(newSig);
            }

            newSig *= BaseVal;
            StringBuilder sb = new StringBuilder();
            sb.Append((int)Math.Floor(newSig));
            int newSigInt = (int)Math.Floor(newSig);
            while (newDigit > 4)
            {
                sb.Append('0');
                newDigit--;
            }
            return new BigNum(sb.ToString());
        }

        public static implicit operator BigNum(int number) => new BigNum(number);
        public static implicit operator BigNum(double number) => new BigNum(number);

        public static bool operator >(BigNum a, BigNum b) => a.CompareTo(b) > 0;
        public static bool operator >=(BigNum a, BigNum b) => a.CompareTo(b) >= 0;
        public static bool operator <(BigNum a, BigNum b) => a.CompareTo(b) < 0;
        public static bool operator <=(BigNum a, BigNum b) => a.CompareTo(b) <= 0;
        public static bool operator ==(BigNum a, BigNum b) => a.CompareTo(b) == 0;
        public static bool operator !=(BigNum a, BigNum b) => a.CompareTo(b) != 0;
    } // Scope by class BigNum

} // namespace Root