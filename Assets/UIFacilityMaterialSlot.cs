using SkyDragonHunter.Managers;
using SkyDragonHunter.Structs;
using SkyDragonHunter.Tables;
using SkyDragonHunter.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIFacilityMaterialSlot : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI requiredAmt;
    private const string countFormatSufficient = "<color=#00FF00>{0}</color> / {1}";
    private const string countFormatInsufficient = "<color=#FF0000>{0}</color> / {1}";

    public void SetSlot(ItemType type, BigNum count)
    {
        icon.sprite = DataTableMgr.ItemTable.Get(type).Icon;
        if(AccountMgr.ItemCount(type) >= count)
        {
            requiredAmt.text = string.Format(countFormatSufficient, AccountMgr.ItemCount(type).ToUnit(), count.ToUnit());
        }
        else
        {
            requiredAmt.text = string.Format(countFormatInsufficient, AccountMgr.ItemCount(type).ToUnit(), count.ToUnit());
        }
    }
}
