using System;
using UnityEngine;

[Serializable]
public class Item
{
    public string Name;
    public int Count;
    public float Weight;
    public int Value;
    public Rarity Rarity;
    public bool IsConsumable;

    // this is to store the icon name as a string, since i was not being able to save the icon through json
    [SerializeField]
    private string iconName; 


    //Storing the icon name for saving and loading purposes
    public Sprite Icon
    {
        get
        {
            if (!string.IsNullOrEmpty(iconName))
            {
                string iconPath = "UI_Assets/" + iconName; 
                return Resources.Load<Sprite>(iconPath);
            }
            return null;
        }
        set
        {
            iconName = value ? value.name : null;
        }
    }

    public Item()
    {
        
    }

    public Item(string name, int count, float weight, int value, Rarity rarity, bool isConsumable, Sprite icon)
    {
        Name = name;
        Count = count;
        Weight = weight;
        Value = value;
        Rarity = rarity;
        IsConsumable = isConsumable;
        Icon = icon;
    }

    //To increase the count of items in inventory
    public void IncrementCount(int amount = 1)
    {
        Count += amount;
    }
}
