using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    [Header("Knockback info")]
    [SerializeField] protected Vector2 knockbackPower = new Vector2(10,5);
    [SerializeField] protected float knockbackDuration = 0.07f;
    protected bool isKnocked;
    [Header("Collision info")]
    public Transform attackCheck;
    public float attackRadius = 1.2f;
    [SerializeField] protected float groundCheckDistance = 1.7f;
    [SerializeField] protected Transform groundCheck;
    [SerializeField] protected float wallCheckDistance = 0.7f;
    [SerializeField] protected Transform wallCheck;
    [SerializeField] protected LayerMask whatIsGround;

    public int facingDir { get; private set; } = 1;
    protected bool facingRight = true;

    public EntityFX fx { get; private set; }
    public CapsuleCollider2D capsuleCollider2D { get; private set; }
    public SpriteRenderer sprite { get; private set; }
    public CharacterStat stats { get; private set; }
    public System.Action onFlipped;
    public int knockbackDir { get; private set; }
    #region Components
    public Animator anim { get; private set; }
    public Rigidbody2D rb { get; private set; }
    #endregion

    protected virtual void Awake() { }
    protected virtual void Start()
    {
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        fx = GetComponentInChildren<EntityFX>();
        sprite = GetComponentInChildren<SpriteRenderer>();
        stats = GetComponent<CharacterStat>();
        capsuleCollider2D = GetComponent<CapsuleCollider2D>();
    }

    
    protected virtual void Update() { }
    public virtual void damageImpact() => StartCoroutine("hitKnockback");
    public virtual void slowEntity(float _slowPercentage,float _slowDuration)
    {
        Invoke("returnSpeed", _slowDuration);
    }
        
    protected virtual void returnSpeed() { }
    public virtual void setUpKnockBackDir(Transform _targetDoDamage)
    {
        if (_targetDoDamage.position.x < transform.position.x)
            knockbackDir = 1;
        else if(_targetDoDamage.position.x > transform.position.x)
            knockbackDir = -1;
    }
    public void setUpKnockBackPower(Vector2 _newKnockBack) => knockbackPower = _newKnockBack; // for player
    public virtual IEnumerator hitKnockback()
    {
        isKnocked = true;
        rb.velocity = new Vector2(knockbackPower.x * knockbackDir, knockbackPower.y);
        yield return new WaitForSeconds(knockbackDuration);
        isKnocked = false;
        setUpZeroKnockBackPower();
    }
    public virtual void setUpZeroKnockBackPower() { }
    public virtual void setUpDefaultDirection(int _dir)
    {
        facingDir = _dir;
        if (_dir == -1)
            facingRight = false;
    }
    public virtual void Die() { }
    #region Velocity
    public virtual void setVelocity(float xVelocity, float yVelocity)
    {
        if (isKnocked) return;
        rb.velocity = new Vector2(xVelocity, yVelocity);
        flipController(xVelocity);
    }
    public virtual void zeroVelocity()
    {
        if(isKnocked) return;
        rb.velocity = Vector2.zero;
    }
    #endregion
    #region Check Collision
    public virtual bool isGroundDetected() => Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, whatIsGround);
    public virtual bool isOnWallDetected() => Physics2D.Raycast(wallCheck.position, Vector2.right * facingDir, wallCheckDistance, whatIsGround);
    protected virtual void OnDrawGizmos()
    {
        Gizmos.DrawLine(groundCheck.position, new Vector3(groundCheck.position.x, transform.position.y - groundCheckDistance));
        Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x + wallCheckDistance, wallCheck.position.y));
        Gizmos.DrawWireSphere(attackCheck.position, attackRadius);
    }
    #endregion
    #region Flip
    public virtual void Flip()
    {
        facingDir = facingDir * -1;
        facingRight = !facingRight;
        transform.Rotate(0, 180, 0);
        if(onFlipped != null)
        onFlipped();    
    }
    public virtual void flipController(float _xVelocity)
    {
        if (_xVelocity < 0 && facingRight)
        {
            Flip();
        }
        else if (_xVelocity > 0 && !facingRight) Flip();
    }
    #endregion
}
