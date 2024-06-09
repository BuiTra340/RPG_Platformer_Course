using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathBringerIdleState : EnemyState
{
    Enemy_DeathBringer enemy;
    private Transform player;
    public DeathBringerIdleState(EnemyStateMachine _stateMachine, Enemy _enemyBase, string _stringBoolName, Enemy_DeathBringer _enemy) : base(_stateMachine, _enemyBase, _stringBoolName)
    {
        enemy = _enemy;
    }
    public override void Enter()
    {
        base.Enter();
        stateTimer = enemy.idleTime;
        player = PlayerManager.instance.player.transform;
    }

    public override void Exit()
    {
        base.Exit();
        //AudioManager.instance.PlaySFX(14, enemy.transform);
    }

    public override void Update()
    {
        base.Update();
        if (Vector2.Distance(enemy.transform.position, player.position) < 5)
            enemy.bossFightBegun = true;

        if (stateTimer < 0 && enemy.bossFightBegun) stateMachine.changeState(enemy.battleState);
    }
}
