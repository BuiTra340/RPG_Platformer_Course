using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : CharacterStat
{
    Enemy enemy;
    public Stat amoutSoul;
    [SerializeField] private int level = 1;

    [Range(0f, 1f)]
    [SerializeField] private float percentageModifier = .4f;
    private ItemDrop myDrop;
    protected override void Start()
    {
        amoutSoul.setDefaultStat(100);
        applyLevelModifiers();
        base.Start();
        enemy = GetComponent<Enemy>();
        myDrop = GetComponent<ItemDrop>();
    }

    private void applyLevelModifiers()
    {
        Modify(strength);
        Modify(agility);
        Modify(intelligence);
        Modify(vitality);

        Modify(armor);
        Modify(maxHealth);
        Modify(evasion);

        Modify(damage);
        Modify(critChance);
        Modify(critPower);
        Modify(magicResistance);

        Modify(fireDamage);
        Modify(iceDamage);
        Modify(lightingDamage);

        Modify(amoutSoul);
    }

    private void Modify(Stat _stat)
    {
        for(int i=1;i<level;i++)
        {
            float modifier = _stat.getValue() * percentageModifier;
            _stat.addModifier(Mathf.RoundToInt(modifier));
        }
    }
    public override void takeDamge(int _damage)
    {
        base.takeDamge(_damage);
    }
    protected override void Die()
    {
        base.Die();
        enemy.Die();
        PlayerManager.instance.currency += amoutSoul.getValue();
        myDrop.generateDrop();
        Destroy(gameObject, 5f);
    }
}
