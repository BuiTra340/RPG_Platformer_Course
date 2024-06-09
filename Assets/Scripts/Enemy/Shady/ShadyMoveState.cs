using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadyMoveState : ShadyGroundState
{
    Enemy_Shady enemy;
    public ShadyMoveState(EnemyStateMachine _stateMachine, Enemy _enemyBase, string _stringBoolName, Enemy_Shady _enemy) : base(_stateMachine, _enemyBase, _stringBoolName, _enemy)
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
