using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UI_Manager : MonoBehaviour
{
    public static UI_Manager Instance { get; private set; }

    [Header("Références - Ne pas modifier")]
    public UI_Player_Standard ui_player_standard;
    public UI_Player_Inspect2D ui_player_inspect2D;
    public UI_Player_Inspect3D ui_player_inspect3D;
    public UI_Player_EnterCode ui_player_enterCode;
    public UI_Player_Inventory ui_player_inventory;


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    public void SwitchToUI_Player_Standard()
    {
        ui_player_standard.gameObject.SetActive(true);
        ui_player_inspect2D.gameObject.SetActive(false);
        ui_player_inspect3D.gameObject.SetActive(false);
        ui_player_enterCode.gameObject.SetActive(false);
        ui_player_inventory.gameObject.SetActive(false);
    }

    public UI_Player_Standard GetUI_Player_Standard()
    {
        return ui_player_standard;
    }

    public void SwitchToUI_Player_Inspect2D()
    {
        ui_player_standard.gameObject.SetActive(false);
        ui_player_inspect2D.gameObject.SetActive(true);
        ui_player_inspect3D.gameObject.SetActive(false);
        ui_player_enterCode.gameObject.SetActive(false);
        ui_player_inventory.gameObject.SetActive(false);
    }

    public UI_Player_Inspect2D GetUI_Player_Inspect2D()
    {
        return ui_player_inspect2D;
    }

    public void SwitchToUI_Player_Inspect3D()
    {
        ui_player_standard.gameObject.SetActive(false);
        ui_player_inspect2D.gameObject.SetActive(false);
        ui_player_inspect3D.gameObject.SetActive(true);
        ui_player_enterCode.gameObject.SetActive(false);
        ui_player_inventory.gameObject.SetActive(false);
    }

    public UI_Player_Inspect3D GetUI_Player_Inspect3D()
    {
        return ui_player_inspect3D;
    }

    public void SwitchToUI_Player_EnterCode()
    {
        ui_player_standard.gameObject.SetActive(false);
        ui_player_inspect2D.gameObject.SetActive(false);
        ui_player_inspect3D.gameObject.SetActive(false);
        ui_player_enterCode.gameObject.SetActive(true);
        ui_player_inventory.gameObject.SetActive(false);
    }

    public UI_Player_EnterCode GetUI_Player_EnterCode()
    {
        return ui_player_enterCode;
    }

    public void SwitchToUI_Player_Inventory()
    {
        ui_player_standard.gameObject.SetActive(false);
        ui_player_inspect2D.gameObject.SetActive(false);
        ui_player_inspect3D.gameObject.SetActive(false);
        ui_player_enterCode.gameObject.SetActive(false);
        ui_player_inventory.gameObject.SetActive(true);
    }

    public UI_Player_Inventory GetUI_Player_Inventory()
    {
        return ui_player_inventory;
    }
}
