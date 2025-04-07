using SkyDragonHunter.Structs;
using SkyDragonHunter.Utility;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SkyDragonHunter {
    public enum TestType
    {
        DoubleNum,
        StringNum,
    }


    public class BigIntTest : MonoBehaviour
    {
        [SerializeField] private TestType testType;

        public TextMeshProUGUI stringNum;
        public TextMeshProUGUI unitNum;
        public double valueToToss;
        public CustomBigInt bignum;
        public string stringValuesToToss;
        public CustomBigInt m_internalBignum;
        public TMP_InputField addVal;
        public TMP_InputField multiplyVal;

        private void Awake()
        {
            valueToToss = 1;
            stringValuesToToss = "1";


            if (testType == TestType.DoubleNum)
            {
                bignum = new CustomBigInt(valueToToss);
                m_internalBignum = bignum;
                stringNum.text = m_internalBignum.StringNumber;
                unitNum.text = m_internalBignum.ToString();
            }
            else if (testType == TestType.StringNum)
            {
                bignum = new CustomBigInt(stringValuesToToss);
                m_internalBignum = bignum;
                stringNum.text = m_internalBignum.StringNumber;
                unitNum.text = m_internalBignum.ToString();
            }
        }

        public void OnButtonClickedX2()
        {
            if (testType == TestType.DoubleNum)
            {
                valueToToss *= 2;
                bignum = new CustomBigInt(valueToToss);
                m_internalBignum = bignum;
                stringNum.text = m_internalBignum.StringNumber;
                unitNum.text = m_internalBignum.ToString();
            }
            else if (testType == TestType.StringNum)
            {

            }
        }

        public void OnButtonClickedX10()
        {
            if (testType == TestType.DoubleNum)
            {
                valueToToss *= 10;
                bignum = new CustomBigInt(valueToToss);
                m_internalBignum = bignum;
                stringNum.text = m_internalBignum.StringNumber;
                unitNum.text = m_internalBignum.ToString();
            }
            else if (testType == TestType.StringNum)
            {

            }
        }

        public void Reset1()
        {
            if (testType == TestType.DoubleNum)
            {
                valueToToss = 1;
                bignum = new CustomBigInt(valueToToss);
                m_internalBignum = bignum;
                stringNum.text = bignum.StringNumber;
                unitNum.text = bignum.ToString();
            }
            else if (testType == TestType.StringNum)
            {

            }
        }
        
        public void Reset10D()
        {
            if (testType == TestType.DoubleNum)
            {
                valueToToss = 10_000_000_000_000;
                bignum = new CustomBigInt(valueToToss);
                m_internalBignum = bignum;
                stringNum.text = bignum.StringNumber;
                unitNum.text = bignum.ToString();
            }
            else if (testType == TestType.StringNum)
            {
                
            }
        }

        public void TestADD()
        {
            m_internalBignum += new CustomBigInt(addVal.text);
            stringNum.text = m_internalBignum.StringNumber;
            unitNum.text = m_internalBignum.ToString();
        }

        public void TestMultiply()
        {
            m_internalBignum *= new CustomBigInt(multiplyVal.text);
            stringNum.text = m_internalBignum.StringNumber;
            unitNum.text = m_internalBignum.ToString();
        }

    } // Scope by class BigIntTest

} // namespace Root