using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonBattleState : EnemyState
{
    Enemy_Skeleton enemy;
    private Transform player;
    private int moveDir;
    public SkeletonBattleState(EnemyStateMachine _stateMachine, Enemy _enemyBase, string _stringBoolName, Enemy_Skeleton _enemy) : base(_stateMachine, _enemyBase, _stringBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();
        player = PlayerManager.instance.player.transform;
        if (player.GetComponent<PlayerStats>().isDead)
            stateMachine.changeState(enemy.moveState);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        if(enemy.isPlayerDetected())
        {
            stateTimer = enemy.battleTimer;
            if (enemy.isPlayerDetected().distance < enemy.attackDistance)
            {
                if(canAttack())
                    stateMachine.changeState(enemy.attackState);
            }
        }else if(stateTimer <=0 || Vector2.Distance(enemy.transform.position,player.position) > 10) 
            stateMachine.changeState(enemy.idleState);

        if (player.position.x > enemy.transform.position.x)
            moveDir = 1;
        else if (player.position.x < enemy.transform.position.x)
            moveDir = -1;

        if (Vector2.Distance(enemy.transform.position, player.position) < 1.5f) return;
        enemy.setVelocity(enemy.moveSpeed * moveDir, rb.velocity.y);
    }
    private bool canAttack()
    {
        if(Time.time >= enemy.lastTimeAttacked + enemy.attackCooldown)
        {
            enemy.lastTimeAttacked = Time.time;
            return true;
        }
        return false;
    }
}
