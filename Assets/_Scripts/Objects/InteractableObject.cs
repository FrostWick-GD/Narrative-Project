using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractableObject : MonoBehaviour, IInteractable
{
    [System.Serializable]
    public struct statusData
    {
        public string objectName;
        public string actionName;
    }

    public statusData[] statusList;

    private int currentStatus;

    public void ShowInteractionInfo()
    {
        UI_Manager.Instance.GetUI_Player_Standard().UpdateInteractionText(statusList[currentStatus].actionName + " " + statusList[currentStatus].objectName);
    }

    public void HideInteractionInfo()
    {
        
    }

    public void Interact(FPC_CustomAction fpc_ca)
    {
        
    }

    public void Use()
    {

    }

    public void ChangeStatus(int newStatus)
    {
        currentStatus = newStatus;
    }
}
