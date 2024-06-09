using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeAttackState : EnemyState
{
    Enemy_Slime enemy;
    public SlimeAttackState(EnemyStateMachine _stateMachine, Enemy _enemyBase, string _stringBoolName, Enemy_Slime _enemy) : base(_stateMachine, _enemyBase, _stringBoolName)
    {
        enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
        enemy.lastTimeAttacked = Time.time;
    }

    public override void Update()
    {
        base.Update();
        enemy.zeroVelocity();
        if (triggerCalled) stateMachine.changeState(enemy.battleState);
    }
}
