using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonIdleState : SkeletonGroundState
{
    public SkeletonIdleState(EnemyStateMachine _stateMachine, Enemy _enemyBase, string _stringBoolName, Enemy_Skeleton _enemy) : base(_stateMachine, _enemyBase, _stringBoolName, _enemy)
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
        AudioManager.instance.PlaySFX(14, enemy.transform);
    }

    public override void Update()
    {
        base.Update();
        if (stateTimer < 0) stateMachine.changeState(enemy.moveState);
    }
}
