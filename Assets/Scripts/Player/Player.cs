using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity
{
    [Header("Attack details")]
    public Vector2[] attackMovement;
    public float counterDuration;
    public bool isBusy { get; private set; }
    [Header("Move info")]
    public float moveSpeed;
    public float jumpForce;
    public float returnSwordImpact;
    private float defaultMoveSpeed;
    private float defaultJumpForce;

    [Header("Dash info")]
    public float dashSpeed;
    public float dashDuration;
    private float defaultDashSpeed;
    public float dashDirection { get; private set; } 
    public SkillManager skill { get; private set; }
    public GameObject sword { get; private set; }

    #region States
    public PlayerStateMachine stateMachine { get; private set; }
    public PlayerIdleState idleState { get; private set; }
    public PlayerMoveState moveState { get; private set; }
    public PlayerJumpState jumpState { get; private set; }
    public PlayerAirState airState { get; private set; }
    public PlayerDashState dashState { get; private set; }
    public PlayerWallSlideState wallSlideState { get; private set; }
    public PlayerWallJumpState wallJumpState { get; private set; }
    public PlayerPrimaryAttackState attackState { get; private set; }
    public PlayerCounterAttackState counterAttackState { get; private set; }
    public PlayerAimSwordState aimSwordState { get;private set;}
    public PlayerCatchSwordState catchSwordState { get; private set;}
    public PlayerBlackHoleState blackHoleState { get; private set; }
    public PlayerDeathState dieState { get; private set; }
    #endregion
    protected override void Awake()
    {
        base.Awake();
        stateMachine = new PlayerStateMachine();
        idleState = new PlayerIdleState(stateMachine, this, "Idle");
        moveState = new PlayerMoveState(stateMachine, this, "Move");
        jumpState = new PlayerJumpState(stateMachine, this, "Jump");
        airState = new PlayerAirState(stateMachine, this, "Jump");
        dashState = new PlayerDashState(stateMachine, this, "Dash");
        wallSlideState = new PlayerWallSlideState(stateMachine, this, "WallSlide");
        wallJumpState = new PlayerWallJumpState(stateMachine, this, "Jump");
        attackState = new PlayerPrimaryAttackState(stateMachine, this, "Attack");
        counterAttackState = new PlayerCounterAttackState(stateMachine, this, "CounterAttack");
        aimSwordState = new PlayerAimSwordState(stateMachine, this, "AimSword");
        catchSwordState = new PlayerCatchSwordState(stateMachine, this, "CatchSword");
        blackHoleState = new PlayerBlackHoleState(stateMachine, this, "Jump");
        dieState = new PlayerDeathState(stateMachine, this, "Die");
    }
    protected override void Start()
    {
        base.Start();
        stateMachine.Initialize(idleState);
        skill = SkillManager.instance;
        defaultMoveSpeed = moveSpeed;
        defaultJumpForce = jumpForce;
        defaultDashSpeed = dashSpeed;
    }
    protected override void Update()
    {
        if (Time.timeScale == 0) return;

        base.Update();
        stateMachine.currentState.Update();
        checkForDashInput();
        if (Input.GetKeyDown(KeyCode.K) && skill.crystal.crystalUnlocked)
            skill.crystal.canUseSkill();

        if (Input.GetKeyDown(KeyCode.Alpha1))
            Inventory.instance.useFlask();
    }
    private void checkForDashInput()
    {
        if (isOnWallDetected()) return;
        if (!skill.dash.dashUnlock)
            return;

        if(Input.GetKeyDown(KeyCode.LeftShift) && SkillManager.instance.dash.canUseSkill())
        {
            dashDirection = Input.GetAxisRaw("Horizontal");
            if(dashDirection != 0)
            {
                stateMachine.ChangeState(dashState);
            }
        }
    }
    public void assignNewSword(GameObject _newSword)
    {
        sword = _newSword;
    }
    public void catchTheSword()
    {
        stateMachine.ChangeState(catchSwordState);
        Destroy(sword);
    }
    public void animationTrigger() => stateMachine.currentState.animationFinishTrigger();
    public IEnumerator busyFor(float _time)
    {
        isBusy = true;
        yield return new WaitForSeconds(_time);
        isBusy = false;
    }
    public override void Die()
    {
        base.Die();
        stateMachine.ChangeState(dieState);
    }
    public override void slowEntity(float _slowPercentage, float _slowDuration)
    {
        base.slowEntity(_slowPercentage, _slowDuration);
        moveSpeed *= (1 - _slowPercentage);
        jumpForce *= (1 - _slowPercentage);
        dashSpeed *= (1 - _slowPercentage);
        anim.speed *= (1- _slowPercentage);
    }
    protected override void returnSpeed()
    {
        moveSpeed = defaultMoveSpeed;
        jumpForce= defaultJumpForce;
        dashSpeed = defaultDashSpeed;
        anim.speed = 1;
    }
    public override void setUpZeroKnockBackPower()
    {
        knockbackPower = Vector2.zero;
        zeroVelocity();
    }
}
