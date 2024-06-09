using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyState 
{
    protected EnemyStateMachine stateMachine;
    protected Enemy enemyBase;
    private string stringBoolName;

    protected bool triggerCalled;
    protected float stateTimer;
    protected Rigidbody2D rb;

    public EnemyState(EnemyStateMachine _stateMachine,Enemy _enemyBase,string _stringBoolName)
    {
        this.stateMachine = _stateMachine;
        this.enemyBase = _enemyBase;
        this.stringBoolName = _stringBoolName;
    }
    public virtual void Enter()
    {
        rb = enemyBase.rb;
        enemyBase.anim.SetBool(stringBoolName, true);   
        triggerCalled = false;
    }
    public virtual void Update()
    {
        stateTimer -= Time.deltaTime;
    }
    public virtual void Exit()
    {
        enemyBase.anim.SetBool(stringBoolName, false);
        enemyBase.AsignAnimBoolName(stringBoolName);
    }
    public virtual void animationFinishTrigger()
    {
        triggerCalled = true;   
    }
}
