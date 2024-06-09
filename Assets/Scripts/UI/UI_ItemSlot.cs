using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class UI_ItemSlot : MonoBehaviour,IPointerDownHandler,IPointerEnterHandler,IPointerExitHandler
{
    [SerializeField] protected Image itemImage;
    [SerializeField] protected TextMeshProUGUI itemText;
    public InventoryItem item;
    protected UI ui;
    protected virtual void Start()
    {
        ui = GetComponentInParent<UI>();
    }
    public virtual void OnPointerDown(PointerEventData eventData)
    {
        if (item == null)
            return;

        if (Input.GetKey(KeyCode.P))
        {
            Inventory.instance.removeItem(item.data);
            return;
        }

        if(item.data.itemType == ItemType.Equiment)
            Inventory.instance.equipmentItem(item.data);
        ui.itemToolTip.hideToolTipText();
    }

    public void updateSlot(InventoryItem _newItem)
    {
        item = _newItem;
        itemImage.color = Color.white;
        if (item != null)
        {
            itemImage.sprite = item.data.icon;
            if (item.stackSize > 1)
            {
                itemText.text = item.stackSize.ToString();
            }
            else itemText.text = "";
        }
    }
    public void cleanUpSlot()
    {
        item = null;
        itemImage.sprite = null;
        itemImage.color = Color.clear;
        itemText.text = "";
    }


    public void OnPointerEnter(PointerEventData eventData)
    {
        if (item == null)
            return;
        ui.itemToolTip.showToolTipText(item.data as ItemData_Equipment);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        if (item == null)
            return;
        ui.itemToolTip.hideToolTipText();
    }
}
