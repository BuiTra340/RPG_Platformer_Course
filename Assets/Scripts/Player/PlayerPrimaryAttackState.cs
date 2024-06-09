using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPrimaryAttackState : PlayerState
{
    public int comboCounter { get; private set; }
    private float lastTimeAttacked;
    private float comboWindow = 1f;
    public PlayerPrimaryAttackState(PlayerStateMachine stateMachine, Player _player, string _animBoolName) : base(stateMachine, _player, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        xInput = 0; // fix bug for attack direction
        if (comboCounter > 2 || Time.time > lastTimeAttacked + comboWindow) 
            comboCounter = 0;
        player.anim.SetInteger("ComboCounter",comboCounter);
        stateTimer = .1f;

        float attackDir = player.facingDir;
        if(xInput != 0) attackDir = xInput;
        
        player.setVelocity(player.attackMovement[comboCounter].x * attackDir, player.attackMovement[comboCounter].y);
    }

    public override void Exit()
    {
        base.Exit();
        player.StartCoroutine(player.busyFor(.15f));
        comboCounter++;
        lastTimeAttacked = Time.time;
    }

    public override void Update()
    {
        base.Update();
        if(stateTimer<0) player.zeroVelocity();

        if (triggerCalled) stateMachine.ChangeState(player.idleState);
    }
}
