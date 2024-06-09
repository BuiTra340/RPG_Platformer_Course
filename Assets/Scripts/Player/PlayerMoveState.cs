using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveState : PlayerGroundState
{
    public PlayerMoveState(PlayerStateMachine stateMachine, Player _player, string _animBoolName) : base(stateMachine, _player, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        AudioManager.instance.PlaySFX(14, null);
    }

    public override void Exit()
    {
        base.Exit();
        AudioManager.instance.StopSFX(14);
    }
    public override void Update()
    {
        base.Update();
        player.setVelocity(xInput * player.moveSpeed, rb.velocity.y);
        if (xInput == 0) stateMachine.ChangeState(player.idleState);
    }
}
