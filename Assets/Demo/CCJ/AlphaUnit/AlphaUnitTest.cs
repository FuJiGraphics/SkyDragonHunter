using TMPro;
using UnityEngine;
using UnityEngine.UI;
using SkyDragonHunter.Utility;

namespace SkyDragonHunter.Test
{
    public class AlphaUnitTest : MonoBehaviour
    {
        public double minusNumber;
        public double plusNumber;
        public double alphaNumber;

        public Vector2 equals;
        public Vector2 comparision;

        public TextMeshProUGUI tmp;
        public Button plusUI;
        public Button minusUI;
        public Button updateUI;

        public TextMeshProUGUI equalsUI;
        public TextMeshProUGUI comparision1UI;
        public TextMeshProUGUI comparision2UI;

        private AlphaUnit m_TestUnit;

        private void Awake()
        {
            tmp = GetComponent<TextMeshProUGUI>();
            updateUI.onClick.AddListener(OnUpdateClick);
            plusUI.onClick.AddListener(OnPlusClick);
            minusUI.onClick.AddListener(OnMinusClick);
            m_TestUnit = new AlphaUnit(alphaNumber);
        }

        private void Update()
        {
            AlphaUnit e1 = equals.x;
            AlphaUnit e2 = equals.y;
            if (e1 == e2)
                equalsUI.text = $"{e1} == {e2} : {"true"}";
            else
                equalsUI.text = $"{e1} == {e2} : {"false"}";

            AlphaUnit c1 = comparision.x;
            AlphaUnit c2 = comparision.y;
            if (c1 > c2)
                comparision1UI.text = $"{c1} > {c2} : {"true"}";
            else
                comparision1UI.text = $"{c1} > {c2} : {"false"}";

            if (c1 < c2)
                comparision2UI.text = $"{c1} < {c2} : {"true"}";
            else
                comparision2UI.text = $"{c1} < {c2} : {"false"}";

        }

        public void OnUpdateClick()
        {
            tmp.text = m_TestUnit.ToString();
        }

        public void OnPlusClick()
        {
            m_TestUnit = m_TestUnit + plusNumber;
        }

        public void OnMinusClick()
        {
            m_TestUnit = m_TestUnit - plusNumber;
        }

    } // class AlphaUnitTest
} // namespace SkyDragonHunter.Test
