using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDashState : PlayerState
{
    public PlayerDashState(PlayerStateMachine stateMachine, Player _player, string _animBoolName) : base(stateMachine, _player, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        dashTime = player.dashDuration;
        player.skill.dash.CreateCloneOnDash();
        player.stats.makeInvincible(true);
    }

    public override void Exit()
    {
        base.Exit();
        rb.velocity = new Vector2(0, rb.velocity.y);
        player.skill.dash.CreateCloneOnDashArrival();
        player.stats.makeInvincible(false);
    }

    public override void Update()
    {
        base.Update();
        if(player.isOnWallDetected()) stateMachine.ChangeState(player.airState);

        dashTime -= Time.deltaTime;
        rb.velocity = new Vector2(player.dashSpeed * player.dashDirection, 0);
        if (dashTime <= 0)
        {
            stateMachine.ChangeState(player.idleState);
        }
        player.fx.getAfterImageFromPool();
    }
}
