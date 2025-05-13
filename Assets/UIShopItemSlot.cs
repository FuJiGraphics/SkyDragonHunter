using SkyDragonHunter.UI;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIShopItemSlot : MonoBehaviour
{
    private int index;
    private ItemSlotData slotData;
    private const string itemCountFormat = "{0} / {1}";

    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private Image itemImage;
    [SerializeField] private TextMeshProUGUI countText;
    [SerializeField] private Image currencyImage;
    [SerializeField] private TextMeshProUGUI priceText;
    [SerializeField] private Button buyButton;

    [SerializeField] private Sprite goldIcon;
    [SerializeField] private Sprite diamondIcon;

    public void Start()
    {
        buyButton.onClick.AddListener(UpdateSlotInfo);
    }

    public void SetSlot(ItemSlotData data)
    {
        slotData = data;
        nameText.text = slotData.ItemName;
        itemImage.sprite = slotData.ItemIcon;
        if (slotData.shopType != ShopType.Reroll && slotData.refreshType == ShopRefreshType.Common)
            countText.text = string.Empty;
        else
            countText.text = string.Format(itemCountFormat, slotData.currCount, slotData.maxCount);
        currencyImage.sprite = slotData.CurrencyIcon;
        priceText.text = slotData.Price.ToUnit();
        buyButton.interactable = slotData.Purchasable;
    }

    public void AddListener(UnityAction action)
    {
        buyButton.onClick.AddListener(action);
    }

    private void UpdateSlotInfo()
    {
        if (slotData.shopType != ShopType.Reroll && slotData.refreshType == ShopRefreshType.Common)
            countText.text = string.Empty;
        else
            countText.text = string.Format(itemCountFormat, slotData.currCount, slotData.maxCount);
        buyButton.interactable = slotData.Purchasable;
    }
}
