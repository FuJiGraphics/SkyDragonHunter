using SkyDragonHunter.Structs;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SkyDragonHunter {

    public class UIStageInfoSlot : MonoBehaviour
    {
        [SerializeField] private Image image;
        [SerializeField] private TextMeshProUGUI tmp;

        public void SetSlot(Sprite icon, string text = "")
        {
            if(icon != null)
                image.sprite = icon;
            tmp.text = text;
        }

        public void SetSlot(Sprite icon, int count = 0)
        {
            if (icon != null)
                image.sprite = icon;
            if (count == 0)
                tmp.text = "";
            else
                tmp.text = count.ToString();
        }
        public void SetSlot(Sprite icon, BigNum count)
        {
            if (icon != null)
                image.sprite = icon;
            tmp.text = count.ToUnit();
        }
    } // Scope by class UIStageInfoSlot

} // namespace Root