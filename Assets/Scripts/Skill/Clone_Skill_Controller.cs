using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clone_Skill_Controller : MonoBehaviour
{
    [Header("Attack info")]
    [SerializeField] private Transform attackCheck;
    [SerializeField] private float attackRadius = .8f;
    private SpriteRenderer sprite;
    [SerializeField] private float colorLosingSpeed;
    private float cloneTimer;
    Animator anim;
    private Transform enemyClosest;
    private bool canDuplicateClone;
    private int chanceToDuplicate;
    private int facingDir = 1;
    private Player player;
    private float attackMultiplier;
    private void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }
    void Update()
    {
        cloneTimer -= Time.deltaTime;
        if (cloneTimer < 0)
        {
            sprite.color = new Color(1f, 1f, 1f, sprite.color.a - (Time.deltaTime * colorLosingSpeed));
        }
        if (sprite.color.a <= 0) Destroy(gameObject);
    }
    public void setUpCloneSkill(Transform _newClone, float cloneDuration, bool _canAttack, Vector3 offSet, Transform _enemyClosest, bool _canDuplicate,int _chanceToDuplicate,Player _player, float _attackMultiplier)
    {
        cloneTimer = cloneDuration;
        transform.position = _newClone.position + offSet;
        enemyClosest = _enemyClosest;
        canDuplicateClone = _canDuplicate;
        chanceToDuplicate = _chanceToDuplicate;
        player = _player;
        attackMultiplier = _attackMultiplier;
        faceClosestTarget();
        if (_canAttack)
        {
            anim.SetInteger("AttackNumber", Random.Range(1, 4));
        }
    }
    private void animationTrigger()
    {
        cloneTimer = .1f;
    }
    private void attackTrigger()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(attackCheck.position, attackRadius);
        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null)
            {
                //player.stats.doDamage(hit.GetComponent<CharacterStat>());
                PlayerStats playerStat = player.GetComponent<PlayerStats>();
                EnemyStats enemyStat = hit.GetComponent<EnemyStats>();
                playerStat.cloneDoDamage(enemyStat, attackMultiplier);

                if(SkillManager.instance.clone.aggresiveCloneUnlocked)
                {
                    ItemData_Equipment weaponEquipment = Inventory.instance.getEquipment(EquipmentType.Weapon);
                    if (weaponEquipment != null)
                        weaponEquipment.executeItemEffect(hit.transform);
                }
                if (canDuplicateClone)
                {
                    if(Random.Range(0,100) < chanceToDuplicate)
                    {
                        SkillManager.instance.clone.createClone(hit.transform, new Vector3(.5f * facingDir, 0));
                    }
                }
            }
        }
    }
    private void faceClosestTarget()
    {
        if (enemyClosest != null)
        {
            if (transform.position.x > enemyClosest.position.x)
            {
                facingDir = -1;
                transform.Rotate(0, 180, 0);
            }
        }
    }
}
