using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadyBattleState : EnemyState
{
    private Transform player;
    Enemy_Shady enemy;
    private int moveDir;
    private float defaultSpeed;
    public ShadyBattleState(EnemyStateMachine _stateMachine, Enemy _enemyBase, string _stringBoolName, Enemy_Shady _enemy) : base(_stateMachine, _enemyBase, _stringBoolName)
    {
        enemy = _enemy;
    }
    public override void Enter()
    {
        base.Enter();
        stateTimer = enemy.battleTimer;
        defaultSpeed = enemy.moveSpeed;
        enemy.moveSpeed = enemy.battleMoveSpeed;
        player = PlayerManager.instance.player.transform;
    }

    public override void Exit()
    {
        base.Exit();
        enemy.moveSpeed = defaultSpeed;
    }

    public override void Update()
    {
        base.Update();
        if (enemy.isPlayerDetected())
        {
            stateTimer = enemy.battleTimer;
            if (enemy.isPlayerDetected().distance < enemy.attackDistance)
            {
                stateMachine.changeState(enemy.deathState);
            }
        }
        else
        {
            if (stateTimer <= 0 || Vector2.Distance(enemy.transform.position, player.position) > 10)
                stateMachine.changeState(enemy.idleState);
        }

        if (player.position.x > enemy.transform.position.x)
            moveDir = 1;
        else if (player.position.x < enemy.transform.position.x)
            moveDir = -1;

        //if (Vector2.Distance(enemy.transform.position, player.position) < enemy.agroDistance) return;
        enemy.setVelocity(enemy.moveSpeed * moveDir, rb.velocity.y);
    }
}
