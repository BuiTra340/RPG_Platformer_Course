using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_SkillTreeSlot : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler,ISaveManager
{
    [SerializeField] private int skillCost;
    public bool unlocked;
    [SerializeField] private UI_SkillTreeSlot[] shouldBeUnlocked;
    [SerializeField] private UI_SkillTreeSlot[] shouldBeLocked;
    [SerializeField] private Image skillImage;
    [SerializeField] private Color lockedSkillColor;
    [SerializeField] private string skillName;
    [TextArea]
    [SerializeField] private string skillDescription;
    private UI ui;
    private void OnValidate()
    {
        gameObject.name = "SkillSlot_UI - " + skillName;
    }
    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(() => unlockSkillSlot());
    }
    void Start()
    {
        skillImage = GetComponent<Image>();
        skillImage.color = lockedSkillColor;
        ui = GetComponentInParent<UI>();
        if (unlocked)
            skillImage.color = Color.white;
    }
    public void unlockSkillSlot()
    {
        if (!PlayerManager.instance.haveEnoughMoney(skillCost))
            return;
        for (int i=0;i< shouldBeUnlocked.Length;i++)
        {
            if(shouldBeUnlocked[i].unlocked == false)
            {
                Debug.Log("cannot unlock skill");
                return;
            }
        }
        for(int i=0;i< shouldBeLocked.Length;i++)
        {
            if (shouldBeLocked[i].unlocked == true)
            {
                Debug.Log("cannot unlock skill");
                return;
            }
        }
        unlocked = true;
        skillImage.color = Color.white;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        ui.skillToolTip.showSkillToolTip(skillName, skillDescription,skillCost);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ui.skillToolTip.hideSkillToolTip();
    }

    public void saveData(ref GameData _data)
    {
        _data.skillTree.Remove(skillName);
        _data.skillTree.Add(skillName, unlocked);
    }

    public void loadData(GameData _data)
    {
        if(_data.skillTree.TryGetValue(skillName, out bool value))
        {
            unlocked = value;
        }
    }
}
