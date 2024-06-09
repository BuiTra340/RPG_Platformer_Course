using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAirState : PlayerState
{
    public PlayerAirState(PlayerStateMachine stateMachine, Player _player, string _animBoolName) : base(stateMachine, _player, _animBoolName)
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
        player.setVelocity(xInput * player.moveSpeed, rb.velocity.y);
        if (player.isOnWallDetected()) stateMachine.ChangeState(player.wallSlideState);

        if(player.isGroundDetected()) stateMachine.ChangeState(player.idleState);
    }
}
