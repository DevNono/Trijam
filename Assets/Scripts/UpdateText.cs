using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UpdateText : MonoBehaviour
{
    public TextMeshProUGUI textMeshPro;

    public void UpdateTextValue(string newText)
    {
        textMeshPro.text = newText;
    }
}