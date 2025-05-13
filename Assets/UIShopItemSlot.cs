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
    [SerializeField] private Button lockButton;
    [SerializeField] private Image lockedImage;

    [SerializeField] private Sprite lockedSprite;
    [SerializeField] private Sprite unlockedSprite;

    public void Start()
    {
        buyButton.onClick.AddListener(UpdateSlotInfo);
        lockButton.onClick.AddListener(UpdateSlotInfo);
    }

    public void SetSlot(ItemSlotData data)
    {
        lockButton.interactable = false;
        slotData = data;
        nameText.text = slotData.ItemName;
        itemImage.sprite = slotData.ItemIcon;        
        if (slotData.shopType != ShopType.Reroll && slotData.refreshType == ShopRefreshType.Common)
            countText.text = string.Empty;
        else if (slotData.shopType != ShopType.Reroll)
            countText.text = string.Format(itemCountFormat, slotData.currCount, slotData.maxCount);
        else
        {
            lockButton.interactable = true;
            lockedImage.color = Color.white;
            countText.text = slotData.maxCount.ToString();
            lockedImage.sprite = slotData.locked ? lockedSprite : unlockedSprite;
        }
        currencyImage.sprite = slotData.CurrencyIcon;
        priceText.text = slotData.price.ToUnit();
        buyButton.interactable = slotData.Purchasable;
    }

    public void AddListener(UnityAction action)
    {
        buyButton.onClick.AddListener(action);
    }

    public void AddLockButtonListener(UnityAction action)
    {
        lockButton.onClick.AddListener(action);
    }

    private void UpdateSlotInfo()
    {
        if (slotData.shopType != ShopType.Reroll && slotData.refreshType == ShopRefreshType.Common)
            countText.text = string.Empty;
        else if (slotData.shopType != ShopType.Reroll)
            countText.text = string.Format(itemCountFormat, slotData.currCount, slotData.maxCount);
        else
        {
            lockedImage.sprite = slotData.locked ? lockedSprite : unlockedSprite;
        }
        buyButton.interactable = slotData.Purchasable;
    }
}
