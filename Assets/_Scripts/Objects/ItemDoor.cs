using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.ProBuilder.Shapes;

public class ItemDoor : MonoBehaviour, IInteractable
{
    [Header("R�f�rences - Ne pas modifier")]
    public GameObject doorMesh;

    [System.Serializable]
    public struct statusData
    {
        public string objectName;
        public string actionName;
    }

    [System.Serializable]
    public enum LockStatus
    {
        NotLocked = 0,
        LockedByKey,
        LockedByCode,
        SwitchOnly
    }

    [Header("Param�tres")]
    public statusData[] statusList;

    public bool isClosedAtStart = true;
    public LockStatus isLocked = LockStatus.NotLocked;
    public int[] keyID;
    public string doorCode;
    /*
    public float closedAngle = 0.0f;
    public float openedAngle = 120.0f;
    */
    public Vector3 closedRotation = Vector3.zero;
    public Vector3 openedRotation = Vector3.zero;
    public float animationSpeedModifier = 1.0f;

    [Header("Audio")]
    public AudioClip characterVoiceOver;
    public bool shouldBeReplayed = false;
    public bool shouldStopOnClose = false;
    public AudioClip sfx_openingDoor;
    public AudioClip sfx_closingDoor;
    public AudioClip sfx_doorLockedByKey;
    public AudioClip sfx_doorUnlockedByKey;
    public AudioClip sfx_doorLockedByCode;
    public AudioClip sfx_doorUnlockedByCode;
    public AudioClip sfx_doorOpenedBySwitchOnly;
    public AudioClip sfx_doorOpenedBySwitchOnlyMoving;

    private int currentStatus = 0;
    private AudioSource sfx_audioSource;
    private Outline outline;
    private FPC_CustomAction playerReference;
    private bool isMoving;
    private bool isClosing;
    private float moveCursor;
    private Coroutine animationCoroutine;

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
        playerReference = fpc_ca;

        if (characterVoiceOver != null)
        {
            Audio_Manager.Instance.PlaySoundOnPlayerAudioSource(characterVoiceOver);
            fpc_ca.SetVoiceOverNeedToStop(shouldStopOnClose);

            if (!shouldBeReplayed)
            {
                characterVoiceOver = null;
            }
        }

        switch(isLocked)
        {
            case LockStatus.NotLocked:
                if (animationCoroutine != null)
                {
                    StopCoroutine(animationCoroutine);
                }

                animationCoroutine = StartCoroutine(MovingDoor());
                break;

            case LockStatus.LockedByKey:
                if (playerReference != null)
                {
                    bool keyTest = true;

                    for (int i = 0; i < keyID.Length; i++)
                    {
                        if (!playerReference.playerInventory.IsItemInInventory(keyID[i]))
                        {
                            keyTest = false;
                        }
                    }

                    if (keyTest)
                    {
                        UnlockDoorByKey();
                        Application.Quit();
                    }
                    else
                    {
                        if (sfx_doorLockedByKey != null)
                        {
                            sfx_audioSource.clip = sfx_doorLockedByKey;
                            sfx_audioSource.Play();
                        }
                    }
                }
                break;

            case LockStatus.LockedByCode:
                if (sfx_doorLockedByCode != null)
                {
                    sfx_audioSource.clip = sfx_doorLockedByCode;
                    sfx_audioSource.Play();
                }

                playerReference.PlayerSwitchToEnterCodeMode();
                UI_Manager.Instance.GetUI_Player_EnterCode().InitializeEnterCode(doorCode, this);
                break;

            case LockStatus.SwitchOnly:
                if (playerReference == null)
                {
                    if (animationCoroutine != null)
                    {
                        StopCoroutine(animationCoroutine);
                    }

                    animationCoroutine = StartCoroutine(MovingDoor());
                }
                else if (playerReference != null)
                {
                    if (sfx_doorOpenedBySwitchOnly != null)
                    {
                        sfx_audioSource.clip = sfx_doorOpenedBySwitchOnly;
                        sfx_audioSource.Play();
                    }
                }

                break;
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

    void Start()
    {
        moveCursor = 0;
        isMoving = false;
        isClosing = true;

        if (!isClosedAtStart)
        {
            animationCoroutine = StartCoroutine(MovingDoor());
        }

        if (isLocked == LockStatus.LockedByKey)
        {
            currentStatus = 2;
        }
        else if (isLocked == LockStatus.LockedByCode)
        {
            currentStatus = 4;
        }
        else if(isLocked == LockStatus.SwitchOnly)
        {
            currentStatus = 5;
        }
        else if (isClosedAtStart)
        {
            currentStatus = 0;
        }
        else
        {
            currentStatus = 1;
        }
    }

    IEnumerator MovingDoor()
    {
        if (isMoving)
        {
            moveCursor = 1 - moveCursor;
        }
        else
        {
            isMoving = true;
        }
        
        isClosing = !isClosing;

        if (isLocked != LockStatus.SwitchOnly)
        {
            int newStatus = isClosing ? 0 : 1;
            ChangeStatus(newStatus);

            if (isClosing)
            {
                if (sfx_closingDoor != null)
                {
                    sfx_audioSource.clip = sfx_closingDoor;
                    sfx_audioSource.Play();
                }
            }
            else
            {
                if (sfx_openingDoor != null)
                {
                    sfx_audioSource.clip = sfx_openingDoor;
                    sfx_audioSource.Play();
                }
            }
        }
        else
        {
            if (sfx_doorOpenedBySwitchOnlyMoving != null)
            {
                sfx_audioSource.clip = sfx_doorOpenedBySwitchOnlyMoving;
                sfx_audioSource.Play();
            }
        }

        while (moveCursor < 1)
        {
            moveCursor += Time.deltaTime * animationSpeedModifier;
            /*
            if (isClosing)
            {
                Vector3 newAngle = new Vector3(0, Mathf.LerpAngle(openedAngle, closedAngle, moveCursor), 0);
                Quaternion newRotation = Quaternion.Euler(newAngle);
                doorMesh.transform.SetLocalPositionAndRotation(doorMesh.transform.position, newRotation);
            }
            else
            {
                Vector3 newAngle = new Vector3(0, Mathf.LerpAngle(closedAngle, openedAngle, moveCursor), 0);
                Quaternion newRotation = Quaternion.Euler(newAngle);
                doorMesh.transform.SetLocalPositionAndRotation(doorMesh.transform.position, newRotation);
            }
            */
            if (isClosing)
            {
                Vector3 tmpRotation = Vector3.Lerp(openedRotation, closedRotation, moveCursor);
                Quaternion newRotation = Quaternion.Euler(tmpRotation);
                doorMesh.transform.SetLocalPositionAndRotation(doorMesh.transform.position, newRotation);
            }
            else
            {
                Vector3 tmpRotation = Vector3.Lerp(closedRotation, openedRotation, moveCursor);
                Quaternion newRotation = Quaternion.Euler(tmpRotation);
                doorMesh.transform.SetLocalPositionAndRotation(doorMesh.transform.position, newRotation);
            }

            yield return null;
        }

        isMoving = false;
        moveCursor = 0;
    }

    public void UnlockDoorByKey()
    {
        if (sfx_doorUnlockedByKey != null)
        {
            sfx_audioSource.clip = sfx_doorUnlockedByKey;
            sfx_audioSource.Play();
            Application.Quit();
        }

        isLocked = LockStatus.NotLocked;
        currentStatus = 0;
    }

    public void AbortEnterCode()
    {
        playerReference.PlayerSwitchToStandardMode(FPC_CustomAction.EPlayerStatus.EnterCode);
    }

    public void UnlockDoorByCode()
    {
        if (sfx_doorUnlockedByCode != null)
        {
            sfx_audioSource.clip = sfx_doorUnlockedByCode;
            sfx_audioSource.Play();
        }

        isLocked = LockStatus.NotLocked;
        currentStatus = 0;
        playerReference.PlayerSwitchToStandardMode(FPC_CustomAction.EPlayerStatus.EnterCode);
    }
}
