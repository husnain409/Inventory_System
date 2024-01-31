using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SerializationManager
{
    public static string SerializeObject<T>(T toSerialize)
    {
        // Using JSON serialization for all types other than Item
        if (typeof(T) != typeof(Item))
        {
            return JsonUtility.ToJson(toSerialize);
        }

        // If serializing an Item, use the custom method for serialization
        return SerializeItem(toSerialize as Item);
    }

    public static T DeserializeObject<T>(string toDeserialize)
    {
        // Use JSON deserialization for all types other than Item
        if (typeof(T) != typeof(Item))
        {
            return JsonUtility.FromJson<T>(toDeserialize);
        }

        // If deserializing an Item, use the custom method for deserialization
        return (T)(object)DeserializeItem(toDeserialize);
    }

    // Method to serialize item icon to JSON
    public static string SerializeItemIcon(Sprite icon)
    {
        // Create a dictionary to hold icon information
        Dictionary<string, string> iconData = new Dictionary<string, string>
        {
            { "IconName", icon ? icon.name : null }
        };

        // Serialize the icon data to JSON
        return JsonUtility.ToJson(iconData);
    }

    // Method to deserialize item icon from JSON
    public static Sprite DeserializeItemIcon(string itemIconData)
    {
        // Deserialize the icon data from JSON
        Dictionary<string, string> iconData = JsonUtility.FromJson<Dictionary<string, string>>(itemIconData);

        // Load the icon from Resources using the stored icon name
        if (iconData.TryGetValue("IconName", out string iconName) && !string.IsNullOrEmpty(iconName))
        {
            string iconPath = "Icons/" + iconName; // Assuming icons are stored in a folder named "Icons"
            return Resources.Load<Sprite>(iconPath);
        }

        return null;
    }

    // Method to serialize an Item to JSON
    private static string SerializeItem(Item item)
    {
        // Create a dictionary to hold item information
        Dictionary<string, string> itemData = new Dictionary<string, string>
        {
            { "Name", item.Name },
            { "Count", item.Count.ToString() },
            { "Weight", item.Weight.ToString() },
            { "Value", item.Value.ToString() },
            { "Rarity", ((int)item.Rarity).ToString() },
            { "IsConsumable", item.IsConsumable.ToString() },
            { "IconData", SerializeItemIcon(item.Icon) }
        };

        // Serialize the item data to JSON
        return JsonUtility.ToJson(itemData);
    }

    // Method to deserialize an Item from JSON
    private static Item DeserializeItem(string itemData)
    {
        // Deserialize the item data from JSON
        Dictionary<string, string> itemDict = JsonUtility.FromJson<Dictionary<string, string>>(itemData);

        // Create a new Item using the deserialized data
        Item item = new Item
        {
            Name = GetValueOrDefault(itemDict, "Name", ""),
            Count = GetValueOrDefault(itemDict, "Count", 0),
            Weight = GetValueOrDefault(itemDict, "Weight", 0.0f),
            Value = GetValueOrDefault(itemDict, "Value", 0),
            Rarity = (Rarity)GetValueOrDefault(itemDict, "Rarity", 0),
            IsConsumable = GetValueOrDefault(itemDict, "IsConsumable", false),
            Icon = DeserializeItemIcon(GetValueOrDefault(itemDict, "IconData", ""))
        };

        return item;
    }

    // Helper method to get a value from a dictionary or return a default value if not found
    private static T GetValueOrDefault<T>(Dictionary<string, string> dict, string key, T defaultValue)
    {
        if (dict.TryGetValue(key, out string value))
        {
            // Convert the string value to the desired type
            try
            {
                return (T)Convert.ChangeType(value, typeof(T));
            }
            catch (Exception)
            {
                // Conversion failed, return the default value
            }
        }
        return defaultValue;
    }
}
