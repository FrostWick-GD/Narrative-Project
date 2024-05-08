using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemTriggerVolume : MonoBehaviour, IInteractable
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

    public bool isOneShotOnly = false;
    public bool isToggleInAndOut = false;

    [Header("Audio")]
    public AudioClip characterVoiceOver;
    public bool shouldBeReplayed = false;
    public bool shouldStopOnClose = false;
    public AudioClip sfx_interactStart;
    public bool shouldSfxInteractStartStopOnClose = false;
    public AudioClip sfx_interactEnd;

    private int currentStatus = 0;
    private AudioSource sfx_audioSource;
    private bool shouldBeActive = true;

    public void ShowInteractionInfo()
    {
        UI_Manager.Instance.GetUI_Player_Standard().UpdateInteractionText(statusList[currentStatus].actionName + " " + statusList[currentStatus].objectName);
    }

    public void HideInteractionInfo()
    {

    }

    public void Interact(FPC_CustomAction fpc_ca)
    {
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
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (shouldBeActive)
            {
                if (characterVoiceOver != null)
                {
                    Audio_Manager.Instance.PlaySoundOnPlayerAudioSource(characterVoiceOver);

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

                for (int i = 0; i < switchTargets.Length; i++)
                {
                    if (switchTargets[i].TryGetComponent(out IInteractable interactableObject))
                    {
                        interactableObject.Interact(null);
                    }
                }
            }

            if (isOneShotOnly)
            {
                shouldBeActive = false;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (characterVoiceOver != null && shouldStopOnClose)
            {
                Audio_Manager.Instance.StopSoundOnPlayerAudioSource();
            }

            if (sfx_interactStart != null && sfx_audioSource.isPlaying && shouldSfxInteractStartStopOnClose)
            {
                sfx_audioSource.Stop();
            }

            if (sfx_interactEnd != null)
            {
                sfx_audioSource.clip = sfx_interactEnd;
                sfx_audioSource.Play();
            }

            if (isToggleInAndOut)
            {
                if (shouldBeActive)
                {
                    for (int i = 0; i < switchTargets.Length; i++)
                    {
                        if (switchTargets[i].TryGetComponent(out IInteractable interactableObject))
                        {
                            interactableObject.Interact(null);
                        }
                    }
                }

                if (isOneShotOnly)
                {
                    shouldBeActive = false;
                }
            }
        }
    }
}
