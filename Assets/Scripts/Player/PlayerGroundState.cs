using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundState : PlayerState
{
    public PlayerGroundState(PlayerStateMachine stateMachine, Player _player, string _animBoolName) : base(stateMachine, _player, _animBoolName)
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
        if (Input.GetKeyDown(KeyCode.R) && player.skill.blackHole.blackHoleUnlocked)
        {
            if (player.skill.blackHole.cooldownTimer > 0)
            {
                player.fx.createPopUpText("Cooldown!");
                return;
            }

            stateMachine.ChangeState(player.blackHoleState);
        }

        if(Input.GetMouseButtonDown(1) && hasNoSword() && player.skill.sword.swordUnlocked) stateMachine.ChangeState(player.aimSwordState);

        if(Input.GetKeyDown(KeyCode.Q) && player.skill.parry.parryUnlocked) stateMachine.ChangeState(player.counterAttackState);

        if(Input.GetMouseButtonDown(0)) stateMachine.ChangeState(player.attackState);

        if(!player.isGroundDetected()) stateMachine.ChangeState(player.airState);
        
        if (Input.GetKeyDown(KeyCode.Space) && player.isGroundDetected()) stateMachine.ChangeState(player.jumpState);
    }
    private bool hasNoSword()
    {
        if(!player.sword)
        {
            return true;
        }
        player.sword.GetComponent<Sword_Skill_Controller>().returnSword();
        return false;
    }
}
