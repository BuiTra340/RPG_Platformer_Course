using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerItemDrop : ItemDrop
{
    [Header("Player's drop")]
    [SerializeField] private int chanceToLooseItems;
    [SerializeField] private int chanceToLooseMaterials;
    public override void generateDrop()
    {
        Inventory inventory = Inventory.instance;
        List<InventoryItem> itemToUnequip = new List<InventoryItem>();
        List<InventoryItem> materialToLoose = new List<InventoryItem>();

        foreach(var item in inventory.getEquipmentList())
        {
            if (Random.Range(0, 100) < chanceToLooseItems)
            {
                dropItem(item.data);
                itemToUnequip.Add(item);
            }
        }
        for(int i = 0;i < itemToUnequip.Count;i++)
        {
            inventory.unEquipItem(itemToUnequip[i].data as ItemData_Equipment);
        }

        foreach (var item in inventory.getMaterialList())
        {
            if (Random.Range(0, 100) < chanceToLooseMaterials)
            {
                dropItem(item.data);
                materialToLoose.Add(item);
            }
        }
        for (int i = 0; i < materialToLoose.Count; i++)
        {
            inventory.removeItem(materialToLoose[i].data);
        }
    }
}
