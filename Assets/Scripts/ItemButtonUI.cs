using UnityEngine;
using UnityEngine.UI;

public class ItemButtonUI : MonoBehaviour
{

    public Text itemNameText;
    public Text itemCountText;
    public Image itemIconImage;

    private string itemName;
    private int itemCount;


    private void Awake()
    {
        itemCountText = transform.Find("ItemCount").GetComponent<Text>();
    }

     public void SetItemInfo(string itemName, int itemCount, Sprite icon)
    {
        itemNameText.text = itemName;
        itemCountText.text = itemCount.ToString();
        itemIconImage.sprite = icon;
    }


    public string GetItemName()
    {
        return itemName;
    }

    public void SetItemCount(int count)
    {
        itemCount = count;
        UpdateUI();
    }

    private void UpdateUI()
    {
        itemNameText.text = itemName;
        itemCountText.text = itemCount.ToString();

        // Force canvas update to ensure changes are immediately reflected
        Canvas.ForceUpdateCanvases();
    }

}

