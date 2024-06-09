using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadyStunnedState : EnemyState
{
    Enemy_Shady enemy;
    public ShadyStunnedState(EnemyStateMachine _stateMachine, Enemy _enemyBase, string _stringBoolName, Enemy_Shady _enemy) : base(_stateMachine, _enemyBase, _stringBoolName)
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
        enemy.fx.Invoke("cancelColorChange", 0);
    }

    public override void Update()
    {
        base.Update();
        if (stateTimer <= 0) stateMachine.changeState(enemy.idleState);
    }
}
