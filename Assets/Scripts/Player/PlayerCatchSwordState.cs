using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCatchSwordState : PlayerState
{
    public PlayerCatchSwordState(PlayerStateMachine stateMachine, Player _player, string _animBoolName) : base(stateMachine, _player, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        if (player.transform.position.x > player.sword.transform.position.x && player.facingDir == 1)
            player.Flip();
        else if (player.transform.position.x < player.sword.transform.position.x && player.facingDir == -1)
            player.Flip();

        player.fx.createShakeScreen(player.fx.shakeSwordImpact);
        rb.velocity = new Vector2(player.returnSwordImpact * -player.facingDir, rb.velocity.y);
        player.fx.createDust();
    }

    public override void Exit()
    {
        base.Exit();
        player.zeroVelocity();
        player.StartCoroutine(player.busyFor(.15f));
    }

    public override void Update()
    {
        base.Update();
        if(triggerCalled) stateMachine.ChangeState(player.idleState);
    }
}
