using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemLight : MonoBehaviour, IInteractable
{
    [System.Serializable]
    public struct statusData
    {
        public string objectName;
        public string actionName;
    }

    [Header("Paramètres")]
    public statusData[] statusList;
    public Light[] lights;
    public bool canBeDirectlyInteractedWith = true;
    public bool lightsInitialState = false;

    [Header("Audio")]
    public AudioClip characterVoiceOver;
    public bool shouldBeReplayed = false;
    public bool shouldStopOnClose = false;
    public AudioClip sfx_interactStart;

    private int currentStatus = 0;
    private AudioSource sfx_audioSource;
    private Outline outline;

    public void ShowInteractionInfo()
    {
        if (canBeDirectlyInteractedWith)
        {
            UI_Manager.Instance.GetUI_Player_Standard().UpdateInteractionText(statusList[currentStatus].actionName + " " + statusList[currentStatus].objectName);
            outline.OutlineWidth = 8;
        }
    }

    public void HideInteractionInfo()
    {
        outline.OutlineWidth = 0;
    }

    public void Interact(FPC_CustomAction fpc_ca)
    {
        if (lights.Length > 0)
        {
            if ((fpc_ca == null && !canBeDirectlyInteractedWith) || (fpc_ca != null && canBeDirectlyInteractedWith))
            {
                if (fpc_ca != null)
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
                        sfx_audioSource.clip = sfx_interactStart;
                        sfx_audioSource.Play();
                    }
                }

                for (int i = 0; i < lights.Length; i++)
                {
                    lights[i].enabled = currentStatus == 0 ? true : false;
                }

                if (currentStatus == 1)
                {
                    currentStatus = 0;
                }
                else
                {
                    currentStatus = 1;
                }
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
    }

    private void Start()
    {
        if (lights.Length > 0)
        {
            for (int i = 0; i < lights.Length; i++)
            {
                lights[i].enabled = lightsInitialState;
            }

            if (lightsInitialState)
            {
                currentStatus = 1;
            }
            else
            {
                currentStatus = 0;
            }
        }
    }
}
