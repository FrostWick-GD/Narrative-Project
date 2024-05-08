using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public interface IInteractable
{
    void ShowInteractionInfo();

    void HideInteractionInfo();

    void Interact(FPC_CustomAction fpc_ca);

    void Use();

    void ChangeStatus(int newStatus);
}
