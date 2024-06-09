using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_CraftWindow : MonoBehaviour
{
    [SerializeField] private Image itemImage;
    [SerializeField] private TextMeshProUGUI itemName;
    [SerializeField] private TextMeshProUGUI itemDescription;
    [SerializeField] private Image[] materialsImage;
    [SerializeField] private Button craftButton;
    public void setUpCraftWindow(ItemData_Equipment _item)
    {
        craftButton.onClick.RemoveAllListeners();
        for(int i=0;i<materialsImage.Length;i++)
        {
            materialsImage[i].color = Color.clear;
            materialsImage[i].GetComponentInChildren<TextMeshProUGUI>().color = Color.clear;
        }
        for(int i=0;i<_item.craftingMaterials.Count;i++)
        {
            if (_item.craftingMaterials.Count > materialsImage.Length)
                Debug.LogWarning("have more materials amout than you have materials slot in craft window");

            materialsImage[i].color = Color.white;
            materialsImage[i].sprite = _item.craftingMaterials[i].data.icon;

            TextMeshProUGUI materialSlotText = materialsImage[i].GetComponentInChildren<TextMeshProUGUI>();
            materialSlotText.color = Color.white;
            materialSlotText.text = _item.craftingMaterials[i].stackSize.ToString();
        }
        itemImage.sprite = _item.icon;
        itemName.text = _item.name;
        itemDescription.text = _item.getDescription();
        craftButton.onClick.AddListener(() => Inventory.instance.canCraft(_item,_item.craftingMaterials));
    }
}
