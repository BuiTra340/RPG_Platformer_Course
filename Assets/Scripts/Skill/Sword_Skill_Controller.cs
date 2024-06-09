using System.Collections.Generic;
using UnityEngine;

public class Sword_Skill_Controller : MonoBehaviour
{
    Animator anim;
    Rigidbody2D rb;
    private CircleCollider2D collider2D;
    public bool canRotation = true;
    private float returnSpeed;
    private bool isReturning;
    Player player;

    [Header("Bounce info")]
    private bool isBoucing;
    private int targetIndex = 0;
    private float bounceSpeed;
    private int bounceAmount;
    private List<Transform> enemyTarget = new List<Transform>();

    [Header("Pierce info")]
    private int pierceAmount;

    [Header("Spin info")]
    private bool isSpining;
    private bool wasStoped;
    private float spinDuration;
    private float spinTimer;
    private float maxTravelDistance;

    private float hitTimer;
    private float spinDirection;
    private float freezeTimeDuration;
    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        collider2D = GetComponent<CircleCollider2D>();
    }
    private void Update()
    {
        if (canRotation)
            transform.right = rb.velocity;

        if (isReturning)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, returnSpeed * Time.deltaTime);
            if (Vector2.Distance(transform.position, player.transform.position) < 1f)
            {
                player.catchTheSword();
            }
        }

        bounceLogic();
        spinLogic();
    }

    private void spinLogic()
    {
        if (isSpining)
        {
            if (Vector2.Distance(transform.position, player.transform.position) > maxTravelDistance && !wasStoped)
            {
                stopWhenSpining();
            }
            if (wasStoped)
            {
                transform.position = Vector2.MoveTowards(transform.position,new Vector2(transform.position.x + spinDirection, transform.position.y), 1.5f * Time.deltaTime);
                spinTimer -= Time.deltaTime;
                if (spinTimer <= 0)
                {
                    isSpining = false;
                    isReturning = true;
                }

                hitTimer -= Time.deltaTime;
                if (hitTimer <= 0)
                {
                    Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 1);
                    foreach (var hit in colliders)
                    {
                        if (hit.GetComponent<Enemy>() != null)
                        {
                            swordSkillDamage(hit.GetComponent<Enemy>());
                        }
                    }
                }
            }
        }
    }

    private void bounceLogic()
    {
        if (isBoucing && enemyTarget.Count > 0)
        {
            transform.position = Vector2.MoveTowards(transform.position, enemyTarget[targetIndex].position, bounceSpeed * Time.deltaTime);
            if (Vector2.Distance(transform.position, enemyTarget[targetIndex].position) < .1f)
            {
                swordSkillDamage(enemyTarget[targetIndex].GetComponent<Enemy>());
                targetIndex++;
                bounceAmount--;
                if (targetIndex >= enemyTarget.Count) targetIndex = 0;
                if (bounceAmount <= 0)
                {
                    isBoucing = false;
                    isReturning = true;
                }
            }
        }
    }

    private void stopWhenSpining()
    {
        wasStoped = true;
        rb.constraints = RigidbodyConstraints2D.FreezePosition;
        spinTimer = spinDuration;
    }

    public void setUpBounce(bool _isBouncing,int _bounceAmount,float _bounceSpeed)
    {
        isBoucing = _isBouncing;
        bounceAmount = _bounceAmount;
        bounceSpeed = _bounceSpeed;
        enemyTarget = new List<Transform>();
    }
    public void setUpPierce(int _pierceAmount)
    {
        pierceAmount = _pierceAmount;
    }
    public void setUpSpin(bool _isSpining,float _spinDuration,float _maxTravelDistance,float _hitDuration)
    {
        isSpining= _isSpining;
        spinDuration= _spinDuration;
        maxTravelDistance= _maxTravelDistance;
        hitTimer= _hitDuration;
    }
    public void returnSword()
    {
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        //rb.isKinematic = false;
        transform.parent = null;
        canRotation = true;
        isReturning = true;
        anim.SetBool("Rotation", true);

    }
    private void destroyMe()
    {
        Destroy(gameObject);
    }
    public void setUpSword(Vector2 _dir, float _gravityScale, Player _player,float _freezeTimeDuration,float _returSpeed)
    {
        rb.velocity = _dir;
        rb.gravityScale = _gravityScale;
        player = _player;
        freezeTimeDuration = _freezeTimeDuration;
        returnSpeed = _returSpeed;
        
        if(pierceAmount <= 0)
        anim.SetBool("Rotation", true);
        spinDirection = Mathf.Clamp(rb.velocity.x, -1, 1);
        Invoke("destroyMe", 5);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isReturning) return;

        if(collision.GetComponent<Enemy>() != null)
        {
            Enemy enemy = collision.GetComponent<Enemy>();
            swordSkillDamage(enemy);
        }

        setUpTargetForBounce(collision);

        stuckInto(collision);
    }

    private void swordSkillDamage(Enemy enemy)
    {
        EnemyStats enemyStats = enemy.GetComponent<EnemyStats>();
        player.stats.doDamage(enemyStats);
        if(SkillManager.instance.sword.timeStopUnlocked)
            enemy.freezeTimeFor(freezeTimeDuration);

        if(SkillManager.instance.sword.vulnerableUnlocked)
            enemyStats.MakeVulnerableFor(freezeTimeDuration);
    }

    private void setUpTargetForBounce(Collider2D collision)
    {
        if (collision.GetComponent<Enemy>() != null)
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 10);
            foreach (var hit in colliders)
            {
                if (hit.GetComponent<Enemy>() != null)
                {
                    enemyTarget.Add(hit.transform);
                }
            }
        }
    }

    private void stuckInto(Collider2D collision)
    {
        if (pierceAmount > 0 && collision.GetComponent<Enemy>() != null)
        {
            pierceAmount--;
            return;
        }
        if (isSpining)
        {
            stopWhenSpining();
            return;
        }

        rb.isKinematic = true;
        collider2D.enabled = false;
        canRotation = false;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;

        if (isBoucing && enemyTarget.Count > 0) return;
        anim.SetBool("Rotation", false);
        transform.parent = collision.transform;
        GetComponentInChildren<ParticleSystem>().Play();
    }
}
