using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Freeze Effect", menuName = "Data/Item Effect/Freeze Effect")]
public class FreezeEnemies_Effect : ItemEffect
{
    [SerializeField] private float freezeTime;
    public override void executeEffect(Transform _transform)
    {
        PlayerStats playerStats = PlayerManager.instance.player.GetComponent<PlayerStats>();
        if (playerStats.currentHealth > playerStats.getMaxHealthValue() * .1f)
            return;

        if (!Inventory.instance.canUseArmor())
            return;

        Collider2D[] colliders = Physics2D.OverlapCircleAll(_transform.position, 2);
        foreach(var hit in colliders)
        {
            hit.GetComponent<Enemy>()?.freezeTimeFor(freezeTime);
        }
    }
}
