using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_CraftList : MonoBehaviour,IPointerDownHandler
{
    [SerializeField] private Transform craftSlotParent;
    [SerializeField] private GameObject craftSlotPrefab;
    [SerializeField] private List<ItemData_Equipment> equipmentList;
    private void Start()
    {
        transform.parent.GetChild(0).GetComponent<UI_CraftList>().setUpCraftList();
        setUpDefaultCraftWindow();
    }
    public void setUpCraftList()
    {
        for(int i=0;i< craftSlotParent.childCount;i++)
        {
            Destroy(craftSlotParent.GetChild(i).gameObject);
        }
        for(int i=0;i<equipmentList.Count;i++)
        {
            GameObject newCraft = Instantiate(craftSlotPrefab, craftSlotParent);
            newCraft.GetComponent<UI_CraftSlot>().setUpCraftSlot(equipmentList[i]);
        }
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        setUpCraftList();
    }
    public void setUpDefaultCraftWindow()
    {
        GetComponentInParent<UI>().craftWindow.setUpCraftWindow(equipmentList[0]);
    }
}
