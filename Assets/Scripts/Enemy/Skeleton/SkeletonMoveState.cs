using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonMoveState : SkeletonGroundState
{
    public SkeletonMoveState(EnemyStateMachine _stateMachine, Enemy _enemyBase, string _stringBoolName, Enemy_Skeleton _enemy) : base(_stateMachine, _enemyBase, _stringBoolName, _enemy)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        enemy.setVelocity(enemy.moveSpeed * enemy.facingDir, enemy.rb.velocity.y);
        if (!enemy.isGroundDetected() || enemy.isOnWallDetected())
        {
            enemy.Flip();
            stateMachine.changeState(enemy.idleState);
        }
    }
}
