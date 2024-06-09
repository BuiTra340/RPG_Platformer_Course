using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemObject : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private ItemData itemData;

    private void setUpVisuals()
    {
        if (itemData == null) return;
        GetComponent<SpriteRenderer>().sprite = itemData.icon;
        gameObject.name = "Item objet - " + itemData.name;
    }
    public void setUpItem(ItemData _itemData,Vector2 _velocity)
    {
        itemData = _itemData;
        rb.velocity = _velocity;
        setUpVisuals();
    }
    public void pickUpItem()
    {
        if (!Inventory.instance.canAddItem(itemData as ItemData_Equipment) && itemData.itemType == ItemType.Equiment)
        {
            rb.velocity = new Vector2(0, 7);
            return;
        }
        Inventory.instance.addItem(itemData);
        AudioManager.instance.PlaySFX(18, transform);
        Destroy(gameObject);
    }
}
