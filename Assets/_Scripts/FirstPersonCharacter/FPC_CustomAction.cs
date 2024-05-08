using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FPC_CustomAction : MonoBehaviour
{
    [Header("Références - Ne pas modifier")]
    public InputActionAsset playerActionAsset;

    public GameObject playerGO;
    public GameObject spawnPointForInspect3DMesh;
    public PlayerInventory playerInventory;

    [Header("Paramètres")]
    public float interactionMaxRange;

    private FirstPersonController firstPersonController;
    private StarterAssetsInputs playerInputs;
    private EPlayerStatus playerStatus;

    private Camera playerCam;
    private Vector2 screenCenterPoint;
    private GameObject targetedItem;
    private bool interactionTextNeedUpdate;
    private bool characterVoiceNeedToStop;
    private bool sfxNeedToStop;
    private GameObject itemToInspect;

    private InputAction interactAction;
    private InputAction useAction;
    private InputAction inventoryAction;

    public enum EPlayerStatus
    {
        Standard = 0,
        Inspect2D = 1,
        Inspect3D = 2,
        EnterCode = 3,
        CheckingInventory = 4,
        Blocked
    }

    private void Awake()
    {
        firstPersonController = playerGO.GetComponent<FirstPersonController>();
        playerInputs = playerGO.GetComponent<StarterAssetsInputs>();
        playerStatus = EPlayerStatus.Standard;
        playerCam = Camera.main;
        screenCenterPoint = new Vector2(Screen.width / 2, Screen.height / 2);
        targetedItem = null;
        interactionTextNeedUpdate = true;
        characterVoiceNeedToStop = false;
        sfxNeedToStop = false;

        interactAction = playerActionAsset.FindAction("Interact");
        useAction = playerActionAsset.FindAction("Use");
        inventoryAction = playerActionAsset.FindAction("Inventory");
    }

    void Start()
    {
        PlayerSwitchToStandardMode(EPlayerStatus.Standard);
        UI_Manager.Instance.SwitchToUI_Player_Standard();
    }

    void Update()
    {
        if (playerStatus == EPlayerStatus.Standard)
        {
            RaycastHit hit;

            if (Physics.Raycast(playerCam.ScreenPointToRay(screenCenterPoint), out hit, interactionMaxRange))
            {
                if (hit.collider.CompareTag("InteractableObject"))
                {
                    targetedItem = hit.collider.gameObject;

                    if (targetedItem.TryGetComponent(out IInteractable interactableObject))
                    {
                        interactableObject.ShowInteractionInfo();
                        interactionTextNeedUpdate = true;
                    }
                }
            }
            else
            {
                if (interactionTextNeedUpdate)
                {
                    if (targetedItem != null)
                    {
                        if (targetedItem.TryGetComponent(out IInteractable interactableObject))
                        {
                            interactableObject.HideInteractionInfo();
                        }
                    }
                    UI_Manager.Instance.GetUI_Player_Standard().UpdateInteractionText("");
                    interactionTextNeedUpdate = false;
                }
                targetedItem = null;
            }
            
            if (interactAction.WasPressedThisFrame() && targetedItem != null)
            {
                if (targetedItem.TryGetComponent(out IInteractable interactableObject))
                {
                    interactableObject.Interact(this);
                }
            }

            if (inventoryAction.WasPressedThisFrame())
            {
                PlayerSwitchToInventoryMode();
                UI_Manager.Instance.GetUI_Player_Inventory().UpdateInventoryVisual();
            }
        }
        else if (playerStatus == EPlayerStatus.Inspect2D)
        {
            if (interactAction.WasPressedThisFrame())
            {
                if (targetedItem.TryGetComponent(out IInteractable interactableObject))
                {
                    interactableObject.Interact(this);
                }

                PlayerSwitchToStandardMode(EPlayerStatus.Inspect2D);
            }
        }
        else if (playerStatus == EPlayerStatus.Inspect3D)
        {
            itemToInspect.transform.Rotate(playerInputs.inspect.y * 3, playerInputs.inspect.x * 3, 0);

            if (interactAction.WasPressedThisFrame())
            {
                if (targetedItem.TryGetComponent(out IInteractable interactableObject))
                {
                    interactableObject.Interact(this);
                }

                PlayerSwitchToStandardMode(EPlayerStatus.Inspect3D);
            }
        }
        else if (playerStatus == EPlayerStatus.CheckingInventory)
        {
            if (inventoryAction.WasPressedThisFrame())
            {
                PlayerSwitchToStandardMode(EPlayerStatus.CheckingInventory);
            }
        }
    }

    public void PlayerSwitchToStandardMode(EPlayerStatus previousStatus)
    {
        playerInputs.cursorInputForLook = true;
        playerInputs.SetCursorState(true);
        playerInputs.moveLocked = false;
        playerStatus = EPlayerStatus.Standard;
        playerInputs.MoveInput(Vector3.zero);
        playerInputs.LookInput(Vector3.zero);
        UI_Manager.Instance.SwitchToUI_Player_Standard();

        if (characterVoiceNeedToStop)
        {
            Audio_Manager.Instance.StopSoundOnPlayerAudioSource();
        }

        if (sfxNeedToStop)
        {
            Audio_Manager.Instance.StopSoundOnPlayerAudioSourceSFX();
        }

        switch (previousStatus)
        {
            case EPlayerStatus.Standard:

                break;

            case EPlayerStatus.Inspect2D:

                break;

            case EPlayerStatus.Inspect3D:
                Destroy(itemToInspect);
                break;

            case EPlayerStatus.EnterCode:

                break;
        }
    }

    public void PlayerSwitchToInspect2DMode()
    {
        playerInputs.cursorInputForLook = false;
        playerInputs.SetCursorState(false);
        playerInputs.moveLocked = true;
        playerStatus = EPlayerStatus.Inspect2D;
        playerInputs.MoveInput(Vector3.zero);
        playerInputs.LookInput(Vector3.zero);
        UI_Manager.Instance.SwitchToUI_Player_Inspect2D();
    }

    public void PlayerSwitchToInspect3DMode(GameObject p_itemToInspect)
    {
        playerInputs.cursorInputForLook = false;
        playerInputs.SetCursorState(true);
        playerInputs.moveLocked = true;
        playerStatus = EPlayerStatus.Inspect3D;
        playerInputs.MoveInput(Vector3.zero);
        playerInputs.LookInput(Vector3.zero);
        UI_Manager.Instance.SwitchToUI_Player_Inspect3D();

        itemToInspect = Instantiate(p_itemToInspect, spawnPointForInspect3DMesh.transform.position, Quaternion.identity);
    }

    public void PlayerSwitchToEnterCodeMode()
    {
        playerInputs.cursorInputForLook = false;
        playerInputs.SetCursorState(false);
        playerInputs.moveLocked = true;
        playerStatus = EPlayerStatus.EnterCode;
        playerInputs.MoveInput(Vector3.zero);
        playerInputs.LookInput(Vector3.zero);
        UI_Manager.Instance.SwitchToUI_Player_EnterCode();
    }

    public void PlayerSwitchToInventoryMode()
    {
        playerInputs.cursorInputForLook = false;
        playerInputs.SetCursorState(false);
        playerInputs.moveLocked = true;
        playerStatus = EPlayerStatus.CheckingInventory;
        playerInputs.MoveInput(Vector3.zero);
        playerInputs.LookInput(Vector3.zero);
        UI_Manager.Instance.SwitchToUI_Player_Inventory();
    }

    public void SetVoiceOverNeedToStop(bool value)
    {
        characterVoiceNeedToStop = value;
    }

    public void SetSFXNeedToStop(bool value)
    {
        sfxNeedToStop = value;
    }
}
