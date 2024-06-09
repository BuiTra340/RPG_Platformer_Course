using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : CharacterStat
{
    Player player;
    protected override void Start()
    {
        base.Start();
        player = GetComponent<Player>();
    }
    public override void takeDamge(int _damage)
    {
        base.takeDamge(_damage);
        if (_damage > maxHealth.getValue() * .3f)
        {
            player.fx.createShakeScreen(player.fx.shakeHighDamage);
            GetComponent<Entity>().setUpKnockBackPower(new Vector2(10, 6));
        }
        int soundRandom = Random.Range(34, 36);
        AudioManager.instance.PlaySFX(soundRandom, null);
        ItemData_Equipment itemData = Inventory.instance.getEquipment(EquipmentType.Armor);
        if (itemData != null)
            itemData.executeItemEffect(player.transform);
    }
    protected override void Die()
    {
        base.Die();
        player.Die();
        GetComponent<PlayerItemDrop>()?.generateDrop();
        GameManager.instance.lostCurrencyAmount = PlayerManager.instance.currentCurrency();
        PlayerManager.instance.currency = 0;
    }
    protected override void onEvasion(int _enemyDir)
    {
        SkillManager.instance.dodge.createMirageOnDodge(_enemyDir);
    }
    public void cloneDoDamage(CharacterStat _target,float _attackMultiplier)
    {
        if (targetCanAvoidAttack(_target)) return;
        int totalDamage = damage.getValue() + strength.getValue();
        if(_attackMultiplier > 0)
            totalDamage = Mathf.RoundToInt(totalDamage * _attackMultiplier);
        if (canCrit())
        {
            totalDamage = calculateCriticalDamage(totalDamage);
        }
        totalDamage = checkTargetArmor(totalDamage, _target);
        _target.takeDamge(totalDamage);
        doMagicalDamage(_target);
    }
}
