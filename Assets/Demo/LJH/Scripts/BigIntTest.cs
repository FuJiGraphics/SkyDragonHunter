using SkyDragonHunter.Structs;
using SkyDragonHunter.Utility;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SkyDragonHunter {

    public class BigIntTest : MonoBehaviour
    {
        public TextMeshProUGUI stringNum;
        public TextMeshProUGUI unitNum;
        public double valueToToss;
        public BigInt2 bignum;

        private void Awake()
        {
            valueToToss = 1;
            bignum = new BigInt2(valueToToss);
            stringNum.text = $"{bignum.StringNumber}";
            unitNum.text = $"{bignum.ToString()}";
        }

        public void OnButtonClickedX2()
        {
            valueToToss *= 2;
            bignum = new BigInt2(valueToToss);
            stringNum.text = $"{bignum.StringNumber}";
            unitNum.text = $"{bignum.ToString()}";
        }

        public void OnButtonClickedX10()
        {
            valueToToss *= 10;
            bignum = new BigInt2(valueToToss);
            stringNum.text = $"{bignum.StringNumber}";
            unitNum.text = $"{bignum.ToString()}";
        }

        public void Reset1()
        {
            valueToToss = 1;
            bignum = new BigInt2(valueToToss);
            stringNum.text = $"{bignum.StringNumber}";
            unitNum.text = $"{bignum.ToString()}";
        }

        public void Reset10D()
        {
            valueToToss = 10_000_000_000_000;
            bignum = new BigInt2(valueToToss);
            stringNum.text = $"{bignum.StringNumber}";
            unitNum.text = $"{bignum.ToString()}";
        }

    } // Scope by class BigIntTest

} // namespace Root