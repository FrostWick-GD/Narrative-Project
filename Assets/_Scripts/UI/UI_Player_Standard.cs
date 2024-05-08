using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Player_Standard : MonoBehaviour
{
    [Header("Références - Ne pas modifier")]
    public Image playerCursor;
    public TMPro.TextMeshProUGUI interactionText;

    public void UpdateCursorState(bool p_state)
    {
        playerCursor.enabled = p_state;
    }

    public void UpdateInteractionText(string p_text)
    {
        interactionText.text = p_text;
    }
}
