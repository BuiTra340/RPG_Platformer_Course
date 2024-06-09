using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeGroundState : EnemyState
{
    protected Enemy_Slime enemy;
    private Transform player;
    public SlimeGroundState(EnemyStateMachine _stateMachine, Enemy _enemyBase, string _stringBoolName,Enemy_Slime _enemy) : base(_stateMachine, _enemyBase, _stringBoolName)
    {
        enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();
        player = GameObject.Find("Player").transform;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        if (enemy.isPlayerDetected() || Vector2.Distance(enemy.transform.position, player.position) < enemy.agroDistance)
            stateMachine.changeState(enemy.battleState);
    }
}
