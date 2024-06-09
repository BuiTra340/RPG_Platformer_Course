using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_CraftSlot : UI_ItemSlot
{
    protected override void Start()
    {
        base.Start();
    }
    public override void OnPointerDown(PointerEventData eventData)
    {
        if (item == null)
            return;
        ui.craftWindow.setUpCraftWindow(item.data as ItemData_Equipment);
    }
    public void setUpCraftSlot(ItemData_Equipment _item)
    {
        if (_item == null)
            return;
        item.data = _item;
        itemImage.sprite = _item.icon;
        itemText.text = _item.name;
    }
}
