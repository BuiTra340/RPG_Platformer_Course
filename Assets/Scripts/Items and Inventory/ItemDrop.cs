using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDrop : MonoBehaviour
{
    [SerializeField] private GameObject itemDropPrefab;
    [SerializeField] private ItemData[] possibleDrop;
    private List<ItemData> dropList = new List<ItemData>();
    public virtual void generateDrop()
    {
        if(possibleDrop.Length <= 0)
            return;

        for (int i = 0; i < possibleDrop.Length;i++)
        {
            if(Random.Range(0,100) < possibleDrop[i].chanceDrop)
            {
                dropList.Add(possibleDrop[i]);
            }
        }
        
        for (int i = 0;i< dropList.Count;i++)
        {
            ItemData randomItem = dropList[Random.Range(0,dropList.Count - 1)];
            dropList.Remove(randomItem);
            dropItem(randomItem);
        }
    }
    protected void dropItem(ItemData _itemData)
    {
        GameObject newDrop = Instantiate(itemDropPrefab, transform.position,Quaternion.identity);
        ItemObject itemObject = newDrop.GetComponent<ItemObject>();
        itemObject.setUpItem(_itemData, new Vector2(Random.Range(-5,5),Random.Range(15,20)));
    }
}
