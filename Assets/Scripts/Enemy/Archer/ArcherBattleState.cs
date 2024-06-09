using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherBattleState : EnemyState
{
    Enemy_Archer enemy;
    private Transform player;
    public ArcherBattleState(EnemyStateMachine _stateMachine, Enemy _enemyBase, string _stringBoolName, Enemy_Archer _enemy) : base(_stateMachine, _enemyBase, _stringBoolName)
    {
        enemy = _enemy;
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
        if (enemy.isPlayerDetected())
        {
            stateTimer = enemy.battleTimer;
            if(Vector2.Distance(enemy.transform.position,player.transform.position) < enemy.safeDistance)
            {
                if(Time.time > enemy.jumpCooldown + enemy.lastTimeUsedJump && enemy.checkGroundBehind() && !enemy.checkWallBehind())
                    stateMachine.changeState(enemy.jumpState);
            }
            if (enemy.isPlayerDetected().distance < enemy.attackDistance)
            {
                if (canAttack())
                    stateMachine.changeState(enemy.attackState);
            }
        }
        else if (stateTimer <= 0 || Vector2.Distance(enemy.transform.position, player.position) > 10)
            stateMachine.changeState(enemy.idleState);
        battleFlipController();
    }

    private void battleFlipController()
    {
        if (player.position.x > enemy.transform.position.x && enemy.facingDir == -1)
            enemy.Flip();
        else if (player.position.x < enemy.transform.position.x && enemy.facingDir == 1)
            enemy.Flip();
    }

    private bool canAttack()
    {
        if (Time.time >= enemy.lastTimeAttacked + enemy.attackCooldown)
        {
            enemy.lastTimeAttacked = Time.time;
            return true;
        }
        return false;
    }
}
