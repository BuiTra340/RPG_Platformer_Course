using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Shady : Enemy
{
    [Header("Shady info")]
    [SerializeField] private GameObject explosivePrafab;
    public float battleMoveSpeed;
    [SerializeField] private float maxSize = 7;
    [SerializeField] private float growSpeed = 3f;
    #region States
    public ShadyIdleState idleState { get; private set; }
    public ShadyMoveState moveState { get; private set; }
    public ShadyBattleState battleState { get; private set; }
    public ShadyStunnedState stunnedState { get; private set; }
    public ShadyDeathState deathState { get; private set; }
    #endregion
    protected override void Awake()
    {
        base.Awake();
        idleState = new ShadyIdleState(stateMachine, this, "Idle", this);
        moveState = new ShadyMoveState(stateMachine, this, "Move", this);
        battleState = new ShadyBattleState(stateMachine, this, "MoveFast", this);
        stunnedState = new ShadyStunnedState(stateMachine, this, "Stunned", this);
        deathState = new ShadyDeathState(stateMachine, this, "Dead", this);
    }
    protected override void Start()
    {
        base.Start();
        stateMachine.Initialize(idleState);
    }

    protected override void Update()
    {
        base.Update();
    }
    public override bool CanBeStunned()
    {
        if (base.CanBeStunned())
        {
            stateMachine.changeState(stunnedState);
            return true;
        }
        return false;
    }
    public override void Die()
    {
        base.Die();
        stateMachine.changeState(deathState);
    }
    public override void createSpecialSkill()
    {
        GameObject newExplosive = Instantiate(explosivePrafab, transform.position, Quaternion.identity);
        newExplosive.GetComponent<Explosive_Controller>().setUpExplosive(maxSize, growSpeed, attackRadius, stats);
    }
}
