using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_AnimationTriggers : MonoBehaviour
{
    private Enemy enemy => gameObject.GetComponentInParent<Enemy>();
    private void animationTrigger()
    {
        enemy.animationFinishTrigger();
    }
    private void attackTrigger()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(enemy.attackCheck.position, enemy.attackRadius);
        foreach(var hit in colliders)
        {
            if(hit.GetComponent<Player>() != null)
            {
                PlayerStats target = hit.GetComponent<PlayerStats>();
                enemy.stats.doDamage(target);
            }
        }
    }
    private void selfDestroy() => enemy.selfDestroy();
    private void specialSkillTrigger()
    {
        enemy.createSpecialSkill();
    }
    private void openCounterWindow()
    {
        enemy.openCounterAttackWindow();
    }
    private void closeCounterWindow()
    {
        enemy.closeCounterAttackWindow();
    }
}
