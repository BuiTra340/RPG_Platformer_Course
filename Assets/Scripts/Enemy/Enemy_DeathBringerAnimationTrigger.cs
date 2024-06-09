using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_DeathBringerAnimationTrigger : Enemy_AnimationTriggers
{
    Enemy_DeathBringer enemy => GetComponentInParent<Enemy_DeathBringer>();
    private void findPosition() => enemy.findPosition();
    private void makeInvincible() => enemy.stats?.makeInvincible(true);
    private void makeTransparentOn() => enemy.fx.makeTransparent(true);
    private void makeTransparentOff() => enemy.fx.makeTransparent(false);
}
