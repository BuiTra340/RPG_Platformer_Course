using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeIdleState : SlimeGroundState
{
    public SlimeIdleState(EnemyStateMachine _stateMachine, Enemy _enemyBase, string _stringBoolName, Enemy_Slime _enemy) : base(_stateMachine, _enemyBase, _stringBoolName, _enemy)
    {
    }
    public override void Enter()
    {
        base.Enter();
        stateTimer = enemy.idleTime;
    }

    public override void Exit()
    {
        base.Exit();
        //AudioManager.instance.PlaySFX(14, enemy.transform);
    }

    public override void Update()
    {
        base.Update();
        if (stateTimer < 0) stateMachine.changeState(enemy.moveState);
    }
}
