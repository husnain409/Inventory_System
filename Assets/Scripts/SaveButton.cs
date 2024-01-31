using UnityEngine;

public class SaveButton : MonoBehaviour
{
    public InventoryUIManager inventoryUIManager; // Reference to inventory UI manager

    public void SaveInventory()
    {
        string saveData = inventoryUIManager.SaveInventory();
        Debug.Log(saveData);
        PlayerPrefs.SetString("InventorySaveData", saveData);
        PlayerPrefs.Save();
        Debug.Log("Inventory saved!");
    }

    public void LoadInventory()
    {
        string saveData = PlayerPrefs.GetString("InventorySaveData");
        inventoryUIManager.LoadInventory(saveData);
    }
}
