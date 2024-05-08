using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Globalization;

public class UI_Player_Inspect2D : MonoBehaviour
{
    [Header("Références - Ne pas modifier")]
    public Image mainImage;
    public TMPro.TextMeshProUGUI txt_objectName;
    public TMPro.TextMeshProUGUI txt_objectDescription;
    public GameObject btn_previous;
    public GameObject btn_next;

    private Sprite[] sprites;
    private int spriteCursor;

    private void Awake()
    {
        spriteCursor = 0;
    }

    public void InitializeInspect2DView(Sprite[] p_sprites, string objectName, string objectDescription)
    {
        sprites = p_sprites;
        spriteCursor = 0;
        mainImage.sprite = sprites[spriteCursor];
        txt_objectName.text = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(objectName.ToLower());
        txt_objectDescription.text = objectDescription;

        if (sprites.Length > 1 )
        {
            btn_previous.SetActive(true);
            btn_next.SetActive(true);
        }
        else
        {
            btn_previous.SetActive(false);
            btn_next.SetActive(false);
        }
    }

    public void ShowPreviousSprite()
    {
        spriteCursor = Mathf.Max(spriteCursor - 1, 0);
        mainImage.sprite = sprites[spriteCursor];
    }

    public void ShowNextSprite()
    {
        spriteCursor = Mathf.Min(spriteCursor + 1, sprites.Length - 1);
        mainImage.sprite = sprites[spriteCursor];
    }
}
