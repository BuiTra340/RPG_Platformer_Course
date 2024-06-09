using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBlackHoleState : PlayerState
{
    private float flyTimer = .4f;
    private bool skillUsed;
    private float defaultGravity;
    public PlayerBlackHoleState(PlayerStateMachine stateMachine, Player _player, string _animBoolName) : base(stateMachine, _player, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        defaultGravity = rb.gravityScale;
        rb.gravityScale = 0;
        stateTimer = flyTimer;
        skillUsed = false;
    }

    public override void Exit()
    {
        base.Exit();
        rb.gravityScale = defaultGravity;
        player.fx.makeTransparent(false);
    }

    public override void Update()
    {
        base.Update();
        if(stateTimer > 0)
            rb.velocity = new Vector2(0, 15);
        if(stateTimer < 0)
        {
            rb.velocity = new Vector2(0, -.1f);
            if(!skillUsed)
            {
                player.skill.blackHole.canUseSkill();
                skillUsed = true;
            }
        }
        if (SkillManager.instance.blackHole.blackHoleFinished())
            stateMachine.ChangeState(player.airState);
    }
}
