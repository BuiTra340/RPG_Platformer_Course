using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeathState : PlayerState
{
    public PlayerDeathState(PlayerStateMachine stateMachine, Player _player, string _animBoolName) : base(stateMachine, _player, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        GameObject.Find("Canvas").GetComponent<UI>().showBannerDie();
        AudioManager.instance.PlaySFX(36, null);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        player.zeroVelocity();
    }
}
