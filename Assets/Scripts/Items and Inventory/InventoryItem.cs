using System;

[Serializable]
public class InventoryItem 
{
    public ItemData data;
    public int stackSize;
    public InventoryItem(ItemData _newData)
    {
        data = _newData;
        addStack();
    }
    public void addStack() => stackSize++;
    public void removeStack() => stackSize--;

}
