using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UI_ItemToolTip : UI_ToolTip
{
    [SerializeField] private TextMeshProUGUI itemNameText;
    [SerializeField] private TextMeshProUGUI itemTypeText;
    [SerializeField] private TextMeshProUGUI itemDescriptionText;
    [SerializeField] private int defaultFontSize = 32;
    public void showToolTipText(ItemData_Equipment item)
    {
        if (item == null)
            return;
        itemNameText.text = item.itemName.ToString();
        itemTypeText.text = item.equipmentType.ToString();
        itemDescriptionText.text = item.getDescription();
        if(itemNameText.text.Length > 12)
            itemNameText.fontSize *= .7f;
        else 
            itemNameText.fontSize = defaultFontSize;
        gameObject.SetActive(true);
        adjustPosition();
    }
    public void hideToolTipText()
    {
        itemNameText.fontSize = defaultFontSize;
        gameObject.SetActive(false);
    }
}
