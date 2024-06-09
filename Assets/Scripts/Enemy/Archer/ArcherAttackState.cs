using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherAttackState : EnemyState
{
    Enemy_Archer enemy;
    public ArcherAttackState(EnemyStateMachine _stateMachine, Enemy _enemyBase, string _stringBoolName, Enemy_Archer _enemy) : base(_stateMachine, _enemyBase, _stringBoolName)
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
        if (triggerCalled) stateMachine.changeState(enemy.battleState);
    }
}
