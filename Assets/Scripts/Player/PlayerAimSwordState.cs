using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAimSwordState : PlayerState
{
    public PlayerAimSwordState(PlayerStateMachine stateMachine, Player _player, string _animBoolName) : base(stateMachine, _player, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        SkillManager.instance.sword.dotsActive(true);
    }

    public override void Exit()
    {
        base.Exit();
        player.StartCoroutine(player.busyFor(.15f));
    }

    public override void Update()
    {
        base.Update();
        player.zeroVelocity();

        if (Input.GetMouseButtonUp(1)) stateMachine.ChangeState(player.idleState);

        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (player.transform.position.x > mousePosition.x && player.facingDir == 1)
            player.Flip();
        else if (player.transform.position.x < mousePosition.x && player.facingDir == -1)
            player.Flip();
    }
}
