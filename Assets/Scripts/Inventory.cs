using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class Inventory
{
    public List<Item> items;

    public Inventory()
    {
        items = new List<Item>();
    }

    // Add an item to the inventory
    public void AddItem(Item newItem)
    {
        Item existingItem = items.Find(item => item.Name == newItem.Name);

        if (existingItem != null)
        {
            existingItem.IncrementCount();
        }
        else
        {
            items.Add(newItem);
        }
    }

    // Get all items in the inventory
    public List<Item> GetAllItems()
    {
        return items;
    }

    public List<Item> GetConsumableItems()
    {
        return items.Where(item => item.IsConsumable).ToList();
    }

    public List<Item> GetNonConsumableItems()
    {
        return items.Where(item => !item.IsConsumable).ToList();
    }

    public void SetItems(List<Item> newItems)
    {
        items = newItems;
    }

    public void ClearInventory()
    {
        items.Clear();
    }

    public string ToJson()
    {
        return JsonUtility.ToJson(this);
    }

    // Deserialize the JSON-formatted string to an inventory
    public static Inventory FromJson(string json)
    {
        return JsonUtility.FromJson<Inventory>(json);
    }

}
