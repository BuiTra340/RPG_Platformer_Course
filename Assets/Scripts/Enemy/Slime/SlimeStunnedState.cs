using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeStunnedState : EnemyState
{
    Enemy_Slime enemy;
    public SlimeStunnedState(EnemyStateMachine _stateMachine, Enemy _enemyBase, string _stringBoolName, Enemy_Slime _enemy) : base(_stateMachine, _enemyBase, _stringBoolName)
    {
        enemy = _enemy;
    }
    public override void Enter()
    {
        base.Enter();
        stateTimer = enemy.stunDuration;
        rb.velocity = new Vector2(enemy.stunDirection.x * -enemy.facingDir, enemy.stunDirection.y);
        enemy.fx.InvokeRepeating("redColorBlink", 0, 0.1f);
    }

    public override void Exit()
    {
        base.Exit();
        enemy.stats.makeInvincible(false);
    }

    public override void Update()
    {
        base.Update();
        if(enemy.rb.velocity.y < .1f && enemy.isGroundDetected())
        {
            enemy.fx.Invoke("cancelColorChange", 0);
            enemy.anim.SetTrigger("StunFold");
            enemy.stats.makeInvincible(true);
        }
        if (stateTimer <= 0) stateMachine.changeState(enemy.idleState);
    }
}
