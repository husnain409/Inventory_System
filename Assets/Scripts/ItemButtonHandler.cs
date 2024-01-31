using UnityEngine;

public class ItemButtonHandler : MonoBehaviour
{
    public InventoryUIManager inventoryUIManager;

    public string itemName;
    public int itemCount;
    public float weight;
    public int value;
    public Rarity rarity;
    public bool isConsumable;
    public Sprite itemIcon;

    public void AddItemToInventory()
    {
        if (inventoryUIManager != null)
        {
            inventoryUIManager.AddItemToInventory(itemName, itemCount, weight, value, rarity, isConsumable, itemIcon);
        }

        UpdateUI();
    }

    private void UpdateUI()
    {
        ItemButtonUI itemButtonUI = GetComponent<ItemButtonUI>();
        if (itemButtonUI != null)
        {
            itemButtonUI.SetItemCount(itemCount);
        }
    }

    public void SetItemDetails(string name, int count, float itemWeight, int itemValue, Rarity itemRarity, bool consumeable)
    {
        itemName = name;
        itemCount = count;
        weight = itemWeight;
        value = itemValue;
        rarity = itemRarity;
        isConsumable = consumeable;
    }
}
