using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.ProBuilder.Shapes;

public class UI_Player_Inspect3D : MonoBehaviour
{
    [Header("Références - Ne pas modifier")]
    public TMPro.TextMeshProUGUI txt_objectName;
    public TMPro.TextMeshProUGUI txt_objectDescription;

    public void InitializeInspect3DView(string objectName, string objectDescription)
    {
        txt_objectName.text = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(objectName.ToLower());
        txt_objectDescription.text = objectDescription;
    }
}
