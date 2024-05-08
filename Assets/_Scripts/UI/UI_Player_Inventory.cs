using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static PlayerInventory;

public class UI_Player_Inventory : MonoBehaviour
{
    [Header("Références - Ne pas modifier")]
    public PlayerInventory playerInventory;
    public TextMeshProUGUI txt_itemName;
    public TextMeshProUGUI txt_itemDescription;
    public Button btn_playAudio;
    public UI_ItemInInventory[] itemSlots;

    public void UpdateInventoryVisual()
    {
        txt_itemName.text = "";
        txt_itemDescription.text = "";
        btn_playAudio.gameObject.SetActive(false);

        for (int i = 0; i < itemSlots.Length; i++)
        {
            if (i < playerInventory.inventory.Count)
            {
                itemSlots[i].img_itemVisual.sprite = playerInventory.inventory[i].itemImage;
                itemSlots[i].txt_itemQuantity.text = playerInventory.inventory[i].itemQuantityInInventory + "";

                if (playerInventory.inventory[i].itemQuantityInInventory > 0)
                {
                    itemSlots[i].gameObject.SetActive(true);
                }
                else
                {
                    itemSlots[i].gameObject.SetActive(false);
                }
            }
            else
            {
                itemSlots[i].gameObject.SetActive(false);
            }
        }
    }

    public void UpdateItemVisualData(int slotID)
    {
        InventoryItem tmpItem = playerInventory.GetInventoryItem(slotID);
        txt_itemName.text = tmpItem.itemName;
        txt_itemDescription.text = tmpItem.itemDescription;
        btn_playAudio.gameObject.SetActive(tmpItem.hasAudioFile);
        Audio_Manager.Instance.playerAudioSource.clip = tmpItem.itemAudioClip;
    }

    public void ItemPlayAudio()
    {
        Audio_Manager.Instance.PlaySoundOnPlayerAudioSource();
    }
}
