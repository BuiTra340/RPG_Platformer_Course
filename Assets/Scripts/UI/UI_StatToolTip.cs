using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UI_StatToolTip : UI_ToolTip
{
    [SerializeField] private TextMeshProUGUI description;
    public void showDescriptionToolTip(string _text)
    {
        description.text = _text;
        gameObject.SetActive(true);
        adjustPosition();
    }
    public void hideDescriptionToolTip() => gameObject.SetActive(false);
}
