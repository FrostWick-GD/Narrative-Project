using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_Player_EnterCode : MonoBehaviour
{
    [Header("Références - Ne pas modifier")]
    public TMPro.TMP_InputField txt_currentCode;
    [SerializeField]
    private string currentCode;
    private int codeSize;
    private string codeValue;
    private ItemDoor doorReference;

    public void InitializeEnterCode(string code, ItemDoor door)
    {
        txt_currentCode.text = "";
        currentCode = "";
        codeSize = code.Length;
        codeValue = code;
        doorReference = door;
    }

    public void AddDigit(string newCharacter)
    {
        if (currentCode.Length < codeSize)
        {
            currentCode = currentCode + newCharacter;
        }
        txt_currentCode.text = currentCode;
    }

    public void RemoveDigit()
    {
        if (currentCode.Length > 0)
        {
            //currentCode = currentCode.Remove(0);
            currentCode = currentCode.Remove(currentCode.Length - 1);
        }
        txt_currentCode.text = currentCode;
    }

    public void AbortEnterCode()
    {
        doorReference.AbortEnterCode();
    }

    public void TryToValidateCode()
    {
        if (currentCode == codeValue)
        {
            print("Code OK");
            doorReference.UnlockDoorByCode();
        }
        else
        {
            print("Code NOK");
        }
    }
}
