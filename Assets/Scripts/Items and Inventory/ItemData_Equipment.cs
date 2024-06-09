using UnityEngine;
using System.Collections.Generic;
public enum EquipmentType
{
    Weapon,
    Armor,
    Amulet,
    Flask
}
[CreateAssetMenu(fileName = "New Item Data", menuName = "Data/Equipment")]
public class ItemData_Equipment : ItemData
{
    public EquipmentType equipmentType;
    public ItemEffect[] itemEffects;
    public float itemCooldown;
    private int desCriptionLength;

    [Header("Major Stats")]
    public Stat strength; 
    public Stat agility;  
    public Stat intelligence; 
    public Stat vitality; 

    [Header("Defensive Stats")]
    public Stat armor;
    public Stat maxHealth;
    public Stat evasion; 

    [Header("Offensive Stats")]
    public Stat damage;
    public Stat critChance;
    public Stat critPower; // default value 150%
    public Stat magicResistance;

    [Header("Magic Stats")]
    public Stat fireDamage;
    public Stat iceDamage;
    public Stat lightingDamage;

    [Header("Craft requirements")]
    public List<InventoryItem> craftingMaterials;
    public void addModifiers()
    {
        PlayerStats player = PlayerManager.instance.player.GetComponent<PlayerStats>();
        player.strength.addModifier(strength.getValue());
        player.agility.addModifier(agility.getValue());
        player.intelligence.addModifier(intelligence.getValue());
        player.vitality.addModifier(vitality.getValue());

        player.armor.addModifier(armor.getValue());
        player.maxHealth.addModifier(maxHealth.getValue());
        player.evasion.addModifier(evasion.getValue());

        player.damage.addModifier(damage.getValue());
        player.critChance.addModifier(critChance.getValue());
        player.critPower.addModifier(critPower.getValue());
        player.magicResistance.addModifier(magicResistance.getValue());

        player.fireDamage.addModifier(fireDamage.getValue());
        player.iceDamage.addModifier(iceDamage.getValue());
        player.lightingDamage.addModifier(lightingDamage.getValue());
    }
    public void removeModifiers()
    {
        PlayerStats player = PlayerManager.instance.player.GetComponent<PlayerStats>();
        player.strength.removeModifier(strength.getValue());
        player.agility.removeModifier(agility.getValue());
        player.intelligence.removeModifier(intelligence.getValue());
        player.vitality.removeModifier(vitality.getValue());

        player.armor.removeModifier(armor.getValue());
        player.maxHealth.removeModifier(maxHealth.getValue());
        player.evasion.removeModifier(evasion.getValue());

        player.damage.removeModifier(damage.getValue());
        player.critChance.removeModifier(critChance.getValue());
        player.critPower.removeModifier(critPower.getValue());
        player.magicResistance.removeModifier(magicResistance.getValue());

        player.fireDamage.removeModifier(fireDamage.getValue());
        player.iceDamage.removeModifier(iceDamage.getValue());
        player.lightingDamage.removeModifier(lightingDamage.getValue());
    }
    public void executeItemEffect(Transform _enemyTransform)
    {
        foreach(var effect in itemEffects)
        {
            effect.executeEffect(_enemyTransform);
        }
    }
    public override string getDescription()
    {
        sb.Length = 0;
        desCriptionLength = 0;
        addItemDescription(strength.getValue(), "Strength");
        addItemDescription(agility.getValue(), "Agility");
        addItemDescription(intelligence.getValue(), "Intelligence");
        addItemDescription(vitality.getValue(), "Vitality");

        addItemDescription(armor.getValue(), "Armor");
        addItemDescription(maxHealth.getValue(), "Max Health");
        addItemDescription(evasion.getValue(), "Evasion");

        addItemDescription(damage.getValue(), "Damage");
        addItemDescription(critChance.getValue(), "Crit.Chance");
        addItemDescription(critPower.getValue(), "Crit.Power");
        addItemDescription(magicResistance.getValue(), "Magic Resist.");

        addItemDescription(fireDamage.getValue(), "Fire Damage");
        addItemDescription(iceDamage.getValue(), "Ice Damage");
        addItemDescription(lightingDamage.getValue(), "Lighting Damage");

        if(desCriptionLength < 5)
        {
            for (int i = 0; i < 5 - desCriptionLength; i++)
            {
                sb.AppendLine();
                sb.Append("");
            }
        }
        if(itemEffects.Length > 0)
        {
            for (int i = 0; i < itemEffects.Length; i++)
            {
                sb.AppendLine();
                sb.Append(itemEffects[i].description);
            }
        }
        
        return sb.ToString();
    }
    private void addItemDescription(int _value,string _name)
    {
        if(_value != 0)
        {
            if (sb.Length > 0)
                sb.AppendLine();
            if (_value > 0)
                sb.Append("+" +_value +" " +_name);
            desCriptionLength++;
        }
    }
}
