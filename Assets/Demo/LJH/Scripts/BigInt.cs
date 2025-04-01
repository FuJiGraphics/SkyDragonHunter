using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace SkyDragonHunter.Structs
{
    public enum BigIntType
    {
        Numbers,
        WithCharacter,
        Exponential,        
    }

    public struct BigInt 
    {
        // 필드 (Fields)
        private static readonly int Base = 1000;
        private static readonly float ReverseBase = 0.001f;
        private static readonly int AlphabetsCount = 26;
        private int[] m_Values;
        private char[] m_Units;

        // 속성 (Properties)
        public int Length
        {
            get
            {
                return m_Values.Length;
            }
        }

        // Public 메서드        
        public BigInt(string stringNumber, BigIntType type = BigIntType.Numbers)
        {
            m_Values = new int[1];
            m_Units = new char[1] { ' ' };
            if (string.IsNullOrEmpty(stringNumber))
            {
                return;
            }
            Initialize(stringNumber, type);
        }

        public BigInt(double doubleNumber)
        {
            if(doubleNumber <= 0)
            {
                m_Values = new int[1];
                m_Units = new char[1];
                return;
            }

            double cached = doubleNumber;
            int numberOfDigits = 1;

            while(cached >= Base)
            {
                cached *= ReverseBase;
                numberOfDigits++;
            }         

            m_Values = new int[numberOfDigits];
            
            int alphabetsLength = 1;
            int charDigitDeterminer = AlphabetsCount;
            while (charDigitDeterminer + alphabetsLength < numberOfDigits)
            {
                charDigitDeterminer = AlphabetsCount;
                for(int i = 0; i < alphabetsLength; ++i)
                {
                    var newDigit = charDigitDeterminer;
                    charDigitDeterminer *= AlphabetsCount;
                }
                alphabetsLength++;
            }
            m_Units = new char[alphabetsLength];

            


        }

        // Private 메서드
        private void IncreaseUnit()
        {
           


        }

        private void Initialize(string stringNumber, BigIntType type)
        {          
            switch (type)
            {
                case BigIntType.Numbers:
                    InitializeNumbersOnly(stringNumber);
                    break;
                case BigIntType.WithCharacter:
                    InitializeNumbersWithCharacter(stringNumber);
                    break;
                case BigIntType.Exponential:
                    InitializeExponential(stringNumber);
                    break;
            }
        }        

        private void InitializeNumbersOnly(string stringNumber)
        {
            #region ValidityCheck
            //foreach (char c in stringNumber)
            //{
            //    if (!char.IsDigit(c))
            //    {
            //        throw new ArgumentException("Only Numbers are acceptable");
            //    }
            //}
            #endregion
            var newLength = stringNumber.Length;
            var firstArrayLength = newLength % 4 + 1;
            var numberOfDigits = newLength / 4 + 1;

            m_Values = new int[numberOfDigits];

            


        }

        private void InitializeNumbersWithCharacter(string stringNumber)
        {
            #region Validity Check & Apply Split Index
            //int splitIndex = -1;
            //int index = 0;
            //foreach (char c in stringNumber)
            //{
            //    if (splitIndex == -1)
            //    {
            //        if (char.IsDigit(c) || c == '.')
            //        {
            //            index++;
            //        }
            //        else
            //        {
            //            splitIndex = index;
            //        }
            //    }
            //    if (char.IsLetterOrDigit(c) || c == '.')
            //    {
            //        continue;
            //    }
            //    else
            //    {
            //        throw new ArgumentException("Only Numbers and Characters are acceptable");
            //    }
            //}
            #endregion


        }

        private void InitializeExponential(string stringNumber)
        {
            #region Validity Check & Apply Split Index
            //int splitIndex = -1;
            //int index = 0;
            //bool plusSignFound = false;
            //foreach (char c in stringNumber)
            //{
            //    if (plusSignFound)
            //    {
            //        if (!(c == 'E' || c == 'e'))
            //        {
            //            throw new ArgumentException("Exponential expression was not properly used. follow example : 5.048+E45");
            //        }
            //        else
            //        {
            //            break;
            //        }
            //    }
            //    if (c == '+')
            //    {
            //        splitIndex = index;
            //        plusSignFound = true;
            //    }
            //    index++;
            //}
            //if (!plusSignFound)
            //{
            //    throw new ArgumentException("Exponential expression was not properly used. follow example : 5.048+E45");
            //}
            #endregion


        }

        // Others

    } // Scope by class BigInt

} // namespace Root