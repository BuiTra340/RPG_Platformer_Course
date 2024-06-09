using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState
{
    protected PlayerStateMachine stateMachine;
    protected Player player;
    private string animBoolName;
    protected float xInput;
    protected float yInput;
    protected Rigidbody2D rb;
    protected float dashTime;
    protected float stateTimer;
    protected bool triggerCalled;

    public PlayerState(PlayerStateMachine stateMachine,Player _player,string _animBoolName)
    {
        this.stateMachine = stateMachine;
        this.player = _player;
        this.animBoolName = _animBoolName;
    }
    public virtual void Enter()
    {
        player.anim.SetBool(animBoolName,true);
        rb = player.rb;
        triggerCalled = false;
    }
    public virtual void Update()
    {
        xInput = Input.GetAxisRaw("Horizontal");
        yInput = Input.GetAxisRaw("Vertical");
        player.anim.SetFloat("yVelocity", rb.velocity.y);
        stateTimer -= Time.deltaTime;
    }
    public virtual void Exit()
    {
        player.anim.SetBool(animBoolName, false);
    }
    public virtual void animationFinishTrigger()
    {
        triggerCalled = true;
    }
}
