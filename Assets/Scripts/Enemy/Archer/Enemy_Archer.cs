using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Archer : Enemy
{
    [Header("Archer info")]
    [SerializeField] private GameObject arrowPrefab;
    [SerializeField] private float arrowSpeed;
    public float safeDistance;
    public Vector2 jumpDirection;
    public float jumpCooldown;
    [HideInInspector] public float lastTimeUsedJump;

    [Header("Additional collision check")]
    [SerializeField] private Transform groundBehindCheck;
    [SerializeField] private Vector2 groundBehindCheckSize;
    [SerializeField] private float wallBehindDistance;
    #region States
    public ArcherIdleState idleState { get; private set; }
    public ArcherMoveState moveState { get; private set; }
    public ArcherBattleState battleState { get; private set; }
    public ArcherAttackState attackState { get; private set; }
    public ArcherStunnedState stunnedState { get; private set; }
    public ArcherDeathState deathState { get; private set; }
    public ArcherJumpState jumpState { get; private set; }
    #endregion
    protected override void Awake()
    {
        base.Awake();
        idleState = new ArcherIdleState(stateMachine, this, "Idle", this);
        moveState = new ArcherMoveState(stateMachine, this, "Move", this);
        battleState = new ArcherBattleState(stateMachine, this, "Idle", this);
        attackState = new ArcherAttackState(stateMachine, this, "Attack", this);
        stunnedState = new ArcherStunnedState(stateMachine, this, "Stunned", this);
        deathState = new ArcherDeathState(stateMachine, this, "Idle", this);
        jumpState = new ArcherJumpState(stateMachine, this, "Jump",this);
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
    public bool checkGroundBehind() => Physics2D.BoxCast(groundBehindCheck.position, groundBehindCheckSize, 0, Vector2.zero, 0, whatIsGround);
    public bool checkWallBehind() => Physics2D.Raycast(wallCheck.position, Vector2.right * -facingDir, wallBehindDistance, whatIsGround);
    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(groundBehindCheck.position,groundBehindCheckSize);
    }
    public override void createSpecialSkill()
    {
        GameObject newArrow = Instantiate(arrowPrefab,attackCheck.position, Quaternion.identity);
        newArrow.GetComponent<Arrow_Controller>().setUpArrow(arrowSpeed,stats,facingDir);
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
}
