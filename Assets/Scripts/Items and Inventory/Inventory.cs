using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Inventory : MonoBehaviour, ISaveManager
{
    public static Inventory instance;

    public List<InventoryItem> inventory;
    public Dictionary<ItemData, InventoryItem> inventoryDictionary;

    public List<InventoryItem> stash;
    public Dictionary<ItemData, InventoryItem> stashDictionary;

    public List<InventoryItem> equipment;
    public Dictionary<ItemData_Equipment, InventoryItem> equipmentDictionary;

    public List<ItemData> startingEquipment;

    [Header("Inventory UI")]
    [SerializeField] private Transform inventorySlotParent;
    [SerializeField] private Transform stashSlotParent;
    [SerializeField] private Transform equipmentSlotParent;
    [SerializeField] private Transform statSlotParent;
    private UI_ItemSlot[] inventoryItemSlot;
    private UI_ItemSlot[] inventoryStashSlot;
    private UI_EquipmentSlot[] equipmentSlot;
    private UI_StatSlot[] statSlot;
    [Header("Items cooldown")]
    private float lastTimeUsedFlask;
    private float lastTimeUsedArmor;

    public float flaskCooldown { get; private set; }
    private float armorCooldown;
    [Header("Data base")]
    public List<ItemData> itemDatabase;
    public List<InventoryItem> loadedItems;
    public List<ItemData_Equipment> loadedEquipments;

    void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }
    private void Start()
    {
        inventory = new List<InventoryItem>();
        stash = new List<InventoryItem>();
        equipment = new List<InventoryItem>();
        inventoryDictionary = new Dictionary<ItemData, InventoryItem>();
        stashDictionary = new Dictionary<ItemData, InventoryItem>();
        equipmentDictionary = new Dictionary<ItemData_Equipment, InventoryItem>();
        inventoryItemSlot = inventorySlotParent.GetComponentsInChildren<UI_ItemSlot>();
        inventoryStashSlot = stashSlotParent.GetComponentsInChildren<UI_ItemSlot>();
        equipmentSlot = equipmentSlotParent.GetComponentsInChildren<UI_EquipmentSlot>();
        statSlot = statSlotParent.GetComponentsInChildren<UI_StatSlot>();
        Invoke("addStartingItems", .1f);
    }
    private void addStartingItems()
    {
        foreach(ItemData_Equipment equipment in loadedEquipments)
        {
            equipmentItem(equipment);
        }

        if(loadedItems.Count > 0)
        {
            foreach(InventoryItem item in loadedItems)
            {
                for(int i= 0;i<item.stackSize;i++)
                {
                    addItem(item.data);
                }
            }
            return;
        }
        for (int i = 0; i < startingEquipment.Count; i++)
        {
            addItem(startingEquipment[i]);
        }
    }
    public void addItem(ItemData _item)
    {
        if (_item.itemType == ItemType.Equiment && canAddItem(_item as ItemData_Equipment))
            addToInventory(_item);
        else if (_item.itemType == ItemType.Material)
            addToStash(_item);
        updateSlotUI();
    }

    private void addToStash(ItemData _item)
    {
        if (stashDictionary.TryGetValue(_item, out InventoryItem value))
        {
            value.addStack();
        }
        else
        {
            InventoryItem newItem = new InventoryItem(_item);
            stash.Add(newItem);
            stashDictionary.Add(_item, newItem);
        }
    }

    private void addToInventory(ItemData _item)
    {
        if (inventoryDictionary.TryGetValue(_item, out InventoryItem value))
        {
            value.addStack();
        }
        else
        {
            InventoryItem newItem = new InventoryItem(_item);
            inventory.Add(newItem);
            inventoryDictionary.Add(_item, newItem);
        }
    }

    public void removeItem(ItemData _item)
    {
        if (inventoryDictionary.TryGetValue(_item, out InventoryItem value))
        {
            if (value.stackSize <= 1)
            {
                inventory.Remove(value);
                inventoryDictionary.Remove(_item);
            }
            else
            {
                value.removeStack();
            }
        }

        if (stashDictionary.TryGetValue(_item, out InventoryItem stashValue))
        {
            if (stashValue.stackSize <= 1)
            {
                stash.Remove(stashValue);
                stashDictionary.Remove(_item);
            }
            else
            {
                stashValue.removeStack();
            }
        }
        updateSlotUI();
    }
    public void updateSlotUI()
    {
        for (int i = 0; i < equipmentSlot.Length; i++)
        {
            foreach (KeyValuePair<ItemData_Equipment, InventoryItem> item in equipmentDictionary)
            {
                if (item.Key.equipmentType == equipmentSlot[i].slotType)
                {
                    equipmentSlot[i].updateSlot(item.Value);
                }
            }
        }


        for (int i = 0; i < inventoryItemSlot.Length; i++)
        {
            inventoryItemSlot[i].cleanUpSlot();
        }
        for (int i = 0; i < inventoryStashSlot.Length; i++)
        {
            inventoryStashSlot[i].cleanUpSlot();
        }

        for (int i = 0; i < inventory.Count; i++)
        {
            inventoryItemSlot[i].updateSlot(inventory[i]);
        }
        for (int i = 0; i < stash.Count; i++)
        {
            inventoryStashSlot[i].updateSlot(stash[i]);
        }

        updateStatUI();
    }

    public void updateStatUI()
    {
        for (int i = 0; i < statSlot.Length; i++) // update stat UI
        {
            statSlot[i].updateStatValueUI();
        }
    }

    public bool canAddItem(ItemData_Equipment _item)
    {
        foreach (var item in inventoryDictionary)
        {
            if (item.Key == _item) return true;
        }
        if (inventory.Count >= inventoryItemSlot.Length)
        {
            Debug.Log("inventory's full");
            return false;
        }
        return true;
    }
    public void equipmentItem(ItemData _item)
    {
        ItemData_Equipment newEquipment = _item as ItemData_Equipment;
        InventoryItem newItem = new InventoryItem(newEquipment);
        ItemData_Equipment oldEquipment = null;
        foreach (KeyValuePair<ItemData_Equipment, InventoryItem> item in equipmentDictionary)
        {
            if (item.Key.equipmentType == newEquipment.equipmentType)
            {
                oldEquipment = item.Key;
            }
        }

        if (oldEquipment != null)
        {
            unEquipItem(oldEquipment);
            addItem(oldEquipment);
        }

        equipment.Add(newItem);
        equipmentDictionary.Add(newEquipment, newItem);
        newEquipment.addModifiers();
        removeItem(_item);
        updateSlotUI();
    }

    public void unEquipItem(ItemData_Equipment oldItem)
    {
        if (equipmentDictionary.TryGetValue(oldItem, out InventoryItem value))
        {
            equipment.Remove(value);
            equipmentDictionary.Remove(oldItem);
            oldItem.removeModifiers();
        }
    }
    public bool canCraft(ItemData_Equipment _itemToCraft, List<InventoryItem> _requiredMaterials)
    {
        List<InventoryItem> materialsToRemove = new List<InventoryItem>();
        for (int i = 0; i < _requiredMaterials.Count; i++)
        {
            if (stashDictionary.TryGetValue(_requiredMaterials[i].data, out InventoryItem stashValue))
            {
                if (stashValue.stackSize < _requiredMaterials[i].stackSize)
                {
                    Debug.Log("not enought material");
                    return false;
                }
                else
                {
                    materialsToRemove.Add(stashValue);
                }
            }
            else
            {
                Debug.Log("not enought material");
                return false;
            }
        }

        for (int i = 0; i < materialsToRemove.Count; i++)
        {
            removeItem(materialsToRemove[i].data);
        }
        addItem(_itemToCraft);
        Debug.Log("craft successfully - " + _itemToCraft.name);
        return true;
    }
    public List<InventoryItem> getEquipmentList() => equipment;
    public List<InventoryItem> getMaterialList() => stash;
    public ItemData_Equipment getEquipment(EquipmentType _type)
    {
        ItemData_Equipment itemData = null;
        foreach (var item in equipmentDictionary)
        {
            if (item.Key.equipmentType == _type)
                itemData = item.Key;
        }
        return itemData;
    }
    public void useFlask()
    {
        ItemData_Equipment currentFlask = getEquipment(EquipmentType.Flask);
        if (currentFlask == null)
            return;
        bool canUseFlask = Time.time > lastTimeUsedFlask + flaskCooldown;
        if (canUseFlask)
        {
            flaskCooldown = currentFlask.itemCooldown;
            currentFlask.executeItemEffect(null);
            lastTimeUsedFlask = Time.time;
        }
        else
        {
            Debug.Log("Flask on cooldown");
        }
    }
    public bool canUseArmor()
    {
        ItemData_Equipment currentArmor = getEquipment(EquipmentType.Armor);
        if (Time.time > lastTimeUsedArmor + armorCooldown)
        {
            armorCooldown = currentArmor.itemCooldown;
            lastTimeUsedArmor = Time.time;
            return true;
        }
        Debug.Log("Armor on cooldown");
        return false;
    }

    public void saveData(ref GameData _data)
    {
        _data.inventory.Clear();
        _data.equipmentId.Clear();
        foreach (var item in inventoryDictionary)
        {
            _data.inventory.Add(item.Key.itemId, item.Value.stackSize);
        }

        foreach(var stash in stashDictionary)
        {
            _data.inventory.Add(stash.Key.itemId, stash.Value.stackSize);
        }

        foreach(var equiment in equipmentDictionary)
        {
            _data.equipmentId.Add(equiment.Key.itemId);
        }
    }

    public void loadData(GameData _data)
    {
        foreach(var pair in _data.inventory)
        {
            foreach(var item in itemDatabase)
            {
                if(item != null && item.itemId == pair.Key)
                {
                    InventoryItem itemToLoad = new InventoryItem(item);
                    itemToLoad.stackSize = pair.Value;
                    loadedItems.Add(itemToLoad);
                }
            }
        }
        foreach(string itemId in _data.equipmentId)
        {
            foreach(var item in itemDatabase)
            {
                if(item != null && itemId == item.itemId)
                {
                    loadedEquipments.Add(item as ItemData_Equipment);
                }
            }
        }
    }

#if UNITY_EDITOR
    [ContextMenu("FillUp Item Database")]
    public void fillUpItemDatabase() => itemDatabase = new List<ItemData>(getItemDataBase());
    private List<ItemData> getItemDataBase()
    {
        List<ItemData> itemDatabse = new List<ItemData>();
        string[] assestNames = AssetDatabase.FindAssets("", new[] { "Assets/Data/Items" });
        foreach(string SOName in assestNames)
        {
            var SOPath = AssetDatabase.GUIDToAssetPath(SOName);
            var itemData = AssetDatabase.LoadAssetAtPath<ItemData>(SOPath);
            if(itemData != null)
                itemDatabse.Add(itemData);
        }
        return itemDatabse;
    }
#endif
}
