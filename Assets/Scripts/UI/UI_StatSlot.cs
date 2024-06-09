using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class UI_StatSlot : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler
{
    [SerializeField] private string statName;
    [SerializeField]public StatType statType;
    [SerializeField] private TextMeshProUGUI statValueText;
    [SerializeField] private TextMeshProUGUI statNameText;

    [TextArea]
    [SerializeField] private string statDescription;
    private UI ui;
    private void OnValidate()
    {
        gameObject.name = "Stat - " + statName;
        if(statNameText != null)
            statNameText.text = statName;
    }
    private void Start()
    {
        updateStatValueUI();
        ui = GetComponentInParent<UI>();
    }
    public void updateStatValueUI()
    {
        PlayerStats playerStats = PlayerManager.instance.player.GetComponent<PlayerStats>();
        if (playerStats != null)
        {
            statValueText.text = playerStats.getModify(statType).getValue().ToString();

            if (statType == StatType.maxHealth)
                statValueText.text = (playerStats.getMaxHealthValue() + playerStats.vitality.getValue() * 5).ToString();

            if (statType == StatType.damage)
                statValueText.text = (playerStats.damage.getValue() + playerStats.strength.getValue()).ToString();

            if (statType == StatType.critPower)
                statValueText.text = (playerStats.critPower.getValue() + playerStats.strength.getValue()).ToString();

            if (statType == StatType.evasion)
                statValueText.text = (playerStats.evasion.getValue() + playerStats.agility.getValue()).ToString();

            if (statType == StatType.critChance)
                statValueText.text = (playerStats.critChance.getValue() + playerStats.agility.getValue()).ToString();

            if (statType == StatType.magicResistance)
                statValueText.text = (playerStats.magicResistance.getValue() + playerStats.intelligence.getValue() * 3).ToString();
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ui.statToolTip.hideDescriptionToolTip();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        ui.statToolTip.showDescriptionToolTip(statDescription);
        
    }
}
