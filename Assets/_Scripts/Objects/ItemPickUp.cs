using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickUp : MonoBehaviour, IInteractable
{
    [System.Serializable]
    public struct statusData
    {
        public string objectName;
        public string actionName;
    }

    [Header("Paramètres")]
    public statusData[] statusList;

    public int itemID;

    [Header("Audio")]
    public AudioClip characterVoiceOver;
    public bool shouldBeReplayed = false;
    public bool shouldStopOnClose = false;
    public AudioClip sfx_interactStart;

    private int currentStatus = 0;
    private Outline outline;

    public void ShowInteractionInfo()
    {
        UI_Manager.Instance.GetUI_Player_Standard().UpdateInteractionText(statusList[currentStatus].actionName + " " + statusList[currentStatus].objectName);
        outline.OutlineWidth = 8;
    }

    public void HideInteractionInfo()
    {
        outline.OutlineWidth = 0;
    }

    public void Interact(FPC_CustomAction fpc_ca)
    {
        if (characterVoiceOver != null)
        {
            Audio_Manager.Instance.PlaySoundOnPlayerAudioSource(characterVoiceOver);
            fpc_ca.SetVoiceOverNeedToStop(shouldStopOnClose);

            if (!shouldBeReplayed)
            {
                characterVoiceOver = null;
            }
        }

        if (sfx_interactStart != null)
        {
            Audio_Manager.Instance.PlaySoundOnPlayerAudioSourceSFX(sfx_interactStart);
        }

        fpc_ca.playerInventory.AddNewItem(itemID);
        this.gameObject.SetActive(false);
    }

    public void Use()
    {

    }

    public void ChangeStatus(int newStatus)
    {
        currentStatus = newStatus;
    }

    private void Awake()
    {
        outline = GetComponent<Outline>();
    }
}
