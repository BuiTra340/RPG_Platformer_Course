using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherJumpState : EnemyState
{
    Enemy_Archer enemy;
    public ArcherJumpState(EnemyStateMachine _stateMachine, Enemy _enemyBase, string _stringBoolName, Enemy_Archer _enemy) : base(_stateMachine, _enemyBase, _stringBoolName)
    {
        enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();
        rb.velocity = new Vector2(enemy.jumpDirection.x * -enemy.facingDir, enemy.jumpDirection.y);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        enemy.anim.SetFloat("yVelocity", rb.velocity.y);
        if(rb.velocity.y <= 0 && enemy.isGroundDetected())
            stateMachine.changeState(enemy.battleState);
    }
}
