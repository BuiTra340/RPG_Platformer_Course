using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathBringerBattleState : EnemyState
{
    Enemy_DeathBringer enemy;
    private Player player;
    private int moveDir;
    public DeathBringerBattleState(EnemyStateMachine _stateMachine, Enemy _enemyBase, string _stringBoolName, Enemy_DeathBringer _enemy) : base(_stateMachine, _enemyBase, _stringBoolName)
    {
        enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();
        player = PlayerManager.instance.player;
    }

    public override void Exit()
    {
        base.Exit();
        enemy.bossFightBegun = false;
    }

    public override void Update()
    {
        base.Update();
        if (player.stats.isDead)
            return;
        if (Vector2.Distance(enemy.transform.position, player.transform.position) > 15)
            stateMachine.changeState(enemy.idleState);

        if (enemy.isPlayerDetected())
        {
            if (enemy.isPlayerDetected().distance < enemy.attackDistance)
            {
                if (canSpellCast())
                    stateMachine.changeState(enemy.teleportState);
                else if (canAttack())
                    stateMachine.changeState(enemy.attackState);
            }
        }

        if (player.transform.position.x > enemy.transform.position.x)
            moveDir = 1;
        else if (player.transform.position.x < enemy.transform.position.x)
            moveDir = -1;

        if (Vector2.Distance(enemy.transform.position, player.transform.position) < enemy.agroDistance) return;
        enemy.setVelocity(enemy.moveSpeed * moveDir, rb.velocity.y);
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
    private bool canSpellCast()
    {
        if (Time.time > enemy.spellStateCooldown + enemy.lastTimeSpellState)
        {
            if (Random.Range(0, 100) < enemy.chanceToTeleport)
            {
                enemy.chanceToTeleport = enemy.defaultChanceToTeleport;
                return true;
            }
        }
        return false;
    }
}
