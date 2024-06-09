using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationTriggers : MonoBehaviour
{
    Player player => GetComponentInParent<Player>();
    public void animationTrigger()
    {
        player.animationTrigger();
    }
    private void attackTrigger()
    {
        AudioManager.instance.PlaySFX(2,null);
        Collider2D[] colliders = Physics2D.OverlapCircleAll(player.attackCheck.position, player.attackRadius);
        foreach(var hit in colliders)
        {
            if(hit.GetComponent<Enemy>() != null)
            {
                EnemyStats target = hit.GetComponent<EnemyStats>();
                if(target != null)
                    player.stats.doDamage(target);

                ItemData_Equipment weaponData = Inventory.instance.getEquipment(EquipmentType.Weapon);
                if (weaponData != null)
                    Inventory.instance.getEquipment(EquipmentType.Weapon).executeItemEffect(target.transform);
            }
        }
    }
    private void throwSword()
    {
        SkillManager.instance.sword.createSword();
    }
}
