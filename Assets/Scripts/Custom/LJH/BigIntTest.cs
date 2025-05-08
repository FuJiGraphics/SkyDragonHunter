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
                m_internalBignum = new CustomBigInt(valueToToss);
                stringNum.text = m_internalBignum.StringNumber;
                unitNum.text = m_internalBignum.ToString();
            }
            else if (testType == TestType.StringNum)
            {
                m_internalBignum = new CustomBigInt(stringValuesToToss);
                stringNum.text = m_internalBignum.StringNumber;
                unitNum.text = m_internalBignum.ToString();
            }
        }

        public void OnButtonClickedX2()
        {
            m_internalBignum *= 2;
            stringNum.text = m_internalBignum.StringNumber;
            unitNum.text = m_internalBignum.ToString();
        }

        public void OnButtonClickedX10()
        {
            m_internalBignum *= 10;
            stringNum.text = m_internalBignum.StringNumber;
            unitNum.text = m_internalBignum.ToString();
        }

        public void Reset1()
        {
            m_internalBignum = 1;
            stringNum.text = m_internalBignum.StringNumber;
            unitNum.text = m_internalBignum.ToString();
        }
        
        public void Reset10D()
        {
            m_internalBignum = new CustomBigInt("10000000000000");
            stringNum.text = m_internalBignum.StringNumber;
            unitNum.text = m_internalBignum.ToString();
        }

        public void TestADD()
        {
            m_internalBignum += new CustomBigInt(addVal.text);
            stringNum.text = m_internalBignum.StringNumber;
            unitNum.text = m_internalBignum.ToString();
        }

        public void TestSubstract()
        {
            m_internalBignum -= new CustomBigInt(addVal.text);
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