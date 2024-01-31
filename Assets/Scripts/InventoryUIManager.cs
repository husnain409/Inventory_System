using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUIManager : MonoBehaviour
{
    public GameObject itemButtonPrefab;
    public Transform itemContentPanel;

    private Inventory inventory = new Inventory();

    public GameObject popUpPanel;
    public Text popUpNameText;
    public Text popUpCountText;
    public Text popUpWeightText;
    public Text popUpValueText;
    public Text popUpRarityText;
    public Image popUpImage;


    public Button useButton;
    public Button removeButton;

    private List<Item> currentItems;

    //Adding item to inventory
    public void AddItemToInventory(string itemName, int itemCount, float weight, int value, Rarity rarity, bool isConsumable, Sprite icon)
    {
        Item newItem = CreateItem(itemName, itemCount, weight, value, rarity, isConsumable, icon);
        HandleNewItem(newItem);
    }


    private void HandleNewItem(Item newItem)
    {
        // Check if the item is already in the inventory
        Item existingItem = inventory.GetAllItems().Find(item => item.Name == newItem.Name);

        if (existingItem != null)
        {
            // If item already exists, increment its count
            existingItem.IncrementCount();
        }
        else
        {
            // If item doesn't exist, add it to the inventory
            inventory.AddItem(newItem);

            // Instantiate a new button for the item
            InstantiateItemButton(newItem);
        }

        // Update the UI
        UpdateUI();
    }


    // Creating a new item
    private Item CreateItem(string itemName, int itemCount, float weight, int value, Rarity rarity, bool isConsumable, Sprite icon)
    {
        return new Item(itemName, itemCount, weight, value, rarity, isConsumable, icon);
    }

    //Spawning item button in inventory
    private void InstantiateItemButton(Item item)
    {

        GameObject newItemButton = Instantiate(itemButtonPrefab, itemContentPanel);
        ItemButtonUI newItemButtonUI = newItemButton.GetComponent<ItemButtonUI>();

        if (newItemButtonUI != null)
        {
            //Setting new item info
            newItemButtonUI.SetItemInfo(item.Name, item.Count, item.Icon); 
            // Add a click listener to the button to show the pop-up panel
            newItemButtonUI.GetComponent<Button>().onClick.AddListener(() => ShowPopUpPanel(item));

            // changing the spawned button name
            newItemButton.name = item.Name + "Button";
        }
    }

    // Additional methods...

    private void Start()
    {
        
    }

    //Updating ui based on items available in inventory
    private void UpdateUI(List<Item> items = null)
    {
        ClearUI();

        currentItems = items != null ? items : inventory.GetAllItems();

        foreach (Item item in currentItems)
        {
            InstantiateItemButton(item);
        }
    }


    //Popup panel to show on button click
    private void ShowPopUpPanel(Item item)
    {
        // Show the pop-up panel with detailed information
        popUpNameText.text = item.Name;
        popUpCountText.text = item.Count.ToString();
        popUpWeightText.text = item.Weight.ToString();
        popUpValueText.text = item.Value.ToString();
        popUpRarityText.text = item.Rarity.ToString();
        popUpImage.sprite = item.Icon;

        popUpPanel.SetActive(true);


        useButton.onClick.RemoveAllListeners();
        useButton.onClick.AddListener(() => UseItem(item));

        removeButton.onClick.RemoveAllListeners();
        removeButton.onClick.AddListener(() => RemoveItemFromInventory(item.Name));
    }

    private void UseItem(Item item)
    {
        if (item.IsConsumable)
        {
            Debug.Log($"Using consumable item: {item.Name}");

            item.IncrementCount(-1);

            if (item.Count <= 0)
            {
                inventory.GetAllItems().Remove(item);

                // Find the corresponding button and remove it from the content panel
                ItemButtonUI buttonToRemove = FindExistingButton(item.Name);
                if (buttonToRemove != null)
                {
                    Destroy(buttonToRemove.gameObject);
                }
                popUpPanel.SetActive(false);
            }

            UpdateUI();
            popUpCountText.text = item.Count.ToString();
        }
        else
        {
            Debug.Log($"Using non-consumable item: {item.Name}");
        }
    }

    public void RemoveItemFromInventory(string itemName)
    {
        // Find and remove the item 
        Item itemToRemove = inventory.GetAllItems().Find(item => item.Name == itemName);
        if (itemToRemove != null)
        {
            itemToRemove.IncrementCount(-1); 

            if (itemToRemove.Count <= 0)
            {
                inventory.GetAllItems().Remove(itemToRemove);

                // Find the corresponding button and remove it from the content panel
                ItemButtonUI buttonToRemove = FindExistingButton(itemName);
                if (buttonToRemove != null)
                {
                    Destroy(buttonToRemove.gameObject);
                }

                // Disable the popup panel when count reaches zero
                popUpPanel.SetActive(false);
            }
            else
            {
                popUpCountText.text = itemToRemove.Count.ToString();
            }
            UpdateUI();
        }
    }

    //Finding the existing button for item in inventory to be used in use and remove functions of inventory
    private ItemButtonUI FindExistingButton(string itemName)
    {
        foreach (Transform child in itemContentPanel)
        {
            ItemButtonUI button = child.GetComponent<ItemButtonUI>();
            if (button != null && button.GetItemName() == itemName + "Button")
            {
                return button;
            }
        }
        return null;
    }

    private void ClearUI()
    {
        // Destroy existing UI buttons
        foreach (Transform child in itemContentPanel)
        {
            Destroy(child.gameObject);
        }
    }

    //Sort by name
    public void SortByNameAndRefreshUI()
    {
        currentItems.Sort((a, b) => a.Name.CompareTo(b.Name));
        UpdateUI();
    }

    //Sort by count
    public void SortByCountAndRefreshUI()
    {
        currentItems.Sort((a, b) => a.Count.CompareTo(b.Count));
        UpdateUI();
    }

    //Filter consumeabel
    public void ShowConsumableItems()
    {
        List<Item> consumableItems = inventory.GetConsumableItems();
        UpdateUI(consumableItems);
    }

    //Filter non-consumeabel
    public void ShowNonConsumableItems()
    {
        List<Item> nonConsumableItems = inventory.GetNonConsumableItems();
        UpdateUI(nonConsumableItems);
    }

    //Filter all
    public void ShowAllItems()
    {
        UpdateUI(inventory.GetAllItems());
    }

    public string SaveInventory()
    {
        // Serialize the inventory to JSON
        string saveData = JsonUtility.ToJson(inventory);
        return saveData;
    }

    public void LoadInventory(string saveData)
    {
        // Deserialize the JSON to inventory
        inventory = JsonUtility.FromJson<Inventory>(saveData);
        // Update the UI after loading
        UpdateUI();
    }

}
