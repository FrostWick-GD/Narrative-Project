using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemSwitch : MonoBehaviour, IInteractable
{
    [System.Serializable]
    public struct statusData
    {
        public string objectName;
        public string actionName;
    }

    [Header("Paramètres")]
    public statusData[] statusList;

    public GameObject[] switchTargets;

    [Header("Audio")]
    public AudioClip characterVoiceOver;
    public bool shouldBeReplayed = false;
    public AudioClip[] sfx_interact;

    private int currentStatus = 0;
    private AudioSource sfx_audioSource;
    private Outline outline;
    private int sfx_cursor;

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

            if (!shouldBeReplayed)
            {
                characterVoiceOver = null;
            }
        }

        if (sfx_interact.Length > 0)
        {
            sfx_audioSource.clip = sfx_interact[sfx_cursor];
            sfx_audioSource.Play();
            sfx_cursor = (sfx_cursor + 1) % sfx_interact.Length;
        }

        for (int i = 0; i < switchTargets.Length; i++)
        {
            if (switchTargets[i].TryGetComponent(out IInteractable interactableObject))
            {
                interactableObject.Interact(null);
            }
        }
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
        sfx_audioSource = GetComponent<AudioSource>();
        outline = GetComponent<Outline>();
        sfx_cursor = 0;
    }
}
