using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThunderStrike_Controller : MonoBehaviour
{
    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerStats player = PlayerManager.instance.player.GetComponent<PlayerStats>();
        if(collision.GetComponent<Enemy>() != null)
        {
            EnemyStats enemy = collision.GetComponent<EnemyStats>();
            player.doMagicalDamage(enemy);
        }
    }
}
