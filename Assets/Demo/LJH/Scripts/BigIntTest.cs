using SkyDragonHunter.Utility;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SkyDragonHunter {

    public class BigIntTest : MonoBehaviour
    {
        public Button button;
        public TextMeshProUGUI numberText;
        public double valueToToss;
        public BigNum bignum;

        private void Awake()
        {
            valueToToss = 1;
            bignum = new BigNum(valueToToss);
            numberText.text = bignum.ToString();
        }

        private void Start()
        {
            button.onClick.AddListener(OnButtonClicked);
        }

        public void OnButtonClicked()
        {
            valueToToss *= 2;
            bignum = new BigNum(valueToToss);
            numberText.text = $"{bignum.StringNumber} \n{bignum.ToString()}";
        }

    } // Scope by class BigIntTest

} // namespace Root