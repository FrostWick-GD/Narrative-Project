using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    [System.Serializable]
    public struct InventoryItem
    {
        public string itemName;
        public int itemID;
        public Sprite itemImage;
        public string itemDescription;
        public bool hasAudioFile;
        public AudioClip itemAudioClip;
        public int itemQuantityInInventory;
    }

    public List<InventoryItem> inventory = new List<InventoryItem>();

    public void AddNewItem(int itemID)
    {
        for (int i = 0; i < inventory.Count; i++)
        {
            if (inventory[i].itemID == itemID)
            {
                InventoryItem tmpItem = inventory[i];
                tmpItem.itemQuantityInInventory = inventory[i].itemQuantityInInventory + 1;
                inventory[i] = tmpItem;
            }
        }
    }

    public void RemoveItem(int itemID)
    {
        for (int i = 0; i < inventory.Count; i++)
        {
            if (inventory[i].itemID == itemID && inventory[i].itemQuantityInInventory > 0)
            {
                InventoryItem tmpItem = inventory[i];
                tmpItem.itemQuantityInInventory = inventory[i].itemQuantityInInventory - 1;
                inventory[i] = tmpItem;
            }
        }
    }

    public bool IsItemInInventory(int itemID)
    {
        for (int i = 0; i < inventory.Count; i++)
        {
            if (inventory[i].itemID == itemID && inventory[i].itemQuantityInInventory > 0)
                return true;
        }

        return false;
    }

    public InventoryItem GetInventoryItem(int itemSlotID)
    {
        return inventory[itemSlotID];
    }
}
