using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder.Shapes;

public class ItemInspect3D : MonoBehaviour, IInteractable
{
    [Header("Références - Ne pas modifier")]
    public MeshRenderer meshRenderer;

    [System.Serializable]
    public struct statusData
    {
        public string objectName;
        public string actionName;
    }

    [Header("Paramètres")]
    public statusData[] statusList;
    public string description;
    public GameObject itemToInspect;

    [Header("Audio")]
    public AudioClip characterVoiceOver;
    public bool shouldBeReplayed = false;
    public bool shouldStopOnClose = false;
    public AudioClip sfx_interactStart;
    public bool shouldSfxInteractStartStopOnClose = false;
    public AudioClip sfx_interactEnd;

    private int currentStatus = 0;
    private AudioSource sfx_audioSource;
    private Outline outline;
    private bool isCurrentlyInteractedWith = false;

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
        if (isCurrentlyInteractedWith)
        {
            meshRenderer.enabled = true;

            if (sfx_interactStart != null && sfx_audioSource.isPlaying && shouldSfxInteractStartStopOnClose)
            {
                sfx_audioSource.Stop();
            }

            if (sfx_interactEnd != null)
            {
                sfx_audioSource.clip = sfx_interactEnd;
                sfx_audioSource.Play();
            }

            isCurrentlyInteractedWith = false;
        }
        else
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

            fpc_ca.PlayerSwitchToInspect3DMode(itemToInspect);
            UI_Manager.Instance.GetUI_Player_Inspect3D().InitializeInspect3DView(statusList[currentStatus].objectName, description);

            meshRenderer.enabled = false;

            if (sfx_interactStart != null)
            {
                sfx_audioSource.clip = sfx_interactStart;
                sfx_audioSource.Play();
            }

            isCurrentlyInteractedWith = true;
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
        isCurrentlyInteractedWith = false;
    }
}
