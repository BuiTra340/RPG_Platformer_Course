using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity
{
    [SerializeField] protected LayerMask whatIsPlayer;
    [Header("Stunned info")]
    public float stunDuration = 1;
    public Vector2 stunDirection = new Vector2(3,5);
    protected bool canBeStunned;
    [SerializeField] private GameObject counterImage;

    [Header("Move info")]
    public float moveSpeed = 1.5f;
    public float idleTime = 1f;
    public float battleTimer = 4f;
    private float defaultMoveSpeed;
    [Header("Attack info")]
    public float attackDistance = 2.15f;
    public float attackCooldown = 2f;
    public float agroDistance = 2;
    [HideInInspector]public float lastTimeAttacked;
    public EnemyStateMachine stateMachine { get; private set; }
    public string lastAnimBoolName { get; private set; }
    protected override void Awake()
    {
        base.Awake();
        stateMachine = new EnemyStateMachine();
    }
    protected override void Start()
    {
        base.Start();
        defaultMoveSpeed = moveSpeed;
    }

    protected override void Update()
    {
        base.Update();
        stateMachine.currentState.Update();
    }
    public virtual void animationFinishTrigger() => stateMachine.currentState.animationFinishTrigger();
    public virtual RaycastHit2D isPlayerDetected() => Physics2D.Raycast(wallCheck.position, Vector2.right * facingDir, attackDistance, whatIsPlayer);
    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x + attackDistance * facingDir, transform.position.y));
    }
    public virtual void createSpecialSkill() { }
    public virtual void openCounterAttackWindow()
    {
        canBeStunned = true;
        counterImage.SetActive(true);
    }
    public virtual void closeCounterAttackWindow()
    {
        canBeStunned=false;
        counterImage.SetActive(false);
    }
    public virtual bool CanBeStunned()
    {
        if (canBeStunned)
        {
            closeCounterAttackWindow();
            return true;
        }
        return false;
    }
    public virtual void freezeTime(bool _timeFrozen)
    {
        if (_timeFrozen)
        {
            moveSpeed = 0;
            anim.speed = 0;
        }
        else
        {
            moveSpeed = defaultMoveSpeed;
            anim.speed = 1;
        }
    }
    public virtual void freezeTimeFor(float _duration) => StartCoroutine(freezeTimeCoroutine(_duration));
    public IEnumerator freezeTimeCoroutine(float _duration)
    {
        freezeTime(true);
        yield return new WaitForSeconds(_duration);
        freezeTime(false);
    }
    public virtual void AsignAnimBoolName(string _lastAnimBoolName)
    {
        lastAnimBoolName = _lastAnimBoolName;
    }
    public override void Die()
    {
        base.Die();
    }
    public override void slowEntity(float _slowPercentage, float _slowDuration)
    {
        base.slowEntity(_slowPercentage, _slowDuration);
        moveSpeed *= (1 - _slowPercentage);
        anim.speed *= (1- _slowPercentage); 
    }
    protected override void returnSpeed()
    {
        moveSpeed = defaultMoveSpeed;
        anim.speed = 1;
    }
    public virtual void selfDestroy() => Destroy(gameObject);
}
