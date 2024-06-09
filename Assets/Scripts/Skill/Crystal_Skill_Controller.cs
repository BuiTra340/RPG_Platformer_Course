using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crystal_Skill_Controller : MonoBehaviour
{
    private CircleCollider2D circleCollider2D => GetComponent<CircleCollider2D>();
    private Animator anim => GetComponent<Animator>();
    private float crystalTimer;
    private bool canExplode;
    private float moveSpeed;
    private bool canMoveToEnemy;
    private Transform enemyTransform;

    private bool canGrow;
    [SerializeField] private float growSpeed = 1.5f;
    [SerializeField] private LayerMask whatIsEnemy;
    private Player player;
    // Update is called once per frame
    void Update()
    {
        crystalTimer -= Time.deltaTime;
        if (crystalTimer <= 0)
        {
            finishCrystal();
        }
        if (canGrow)
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector3(3, 3), growSpeed * Time.deltaTime);
        if(canMoveToEnemy)
        {
            if (enemyTransform == null) return;

            transform.position = Vector2.MoveTowards(transform.position, enemyTransform.position,moveSpeed * Time.deltaTime);
            if(Vector2.Distance(transform.position, enemyTransform.position) < 1)
            {
                canMoveToEnemy = false;
                finishCrystal();
            }
        }
    }
    public void setUpCrystal(float _crystalDuration,bool _canExplode, bool _canMoveToEnemy, float _moveSpeed,Transform _enemyTransform,Player _player)
    {
        crystalTimer = _crystalDuration;
        canExplode = _canExplode;
        moveSpeed = _moveSpeed;
        canMoveToEnemy = _canMoveToEnemy;
        enemyTransform = _enemyTransform;
        player = _player;
    }
    public void animationExplodeEvent()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, circleCollider2D.radius);
        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null)
            {
                player.stats.doDamage(hit.GetComponent<CharacterStat>());

                ItemData_Equipment itemEquipment = Inventory.instance.getEquipment(EquipmentType.Amulet);
                if (itemEquipment != null)
                    itemEquipment.executeItemEffect(hit.transform);
            }
        }
    }
    public void finishCrystal()
    {
        if (canExplode)
        {
            canGrow = true;
            anim.SetTrigger("Explode");
        }
        else selfDestroy();
    }
    public void selfDestroy() => Destroy(gameObject);
    public void chooseRandomEnemy()
    {
        float radius = SkillManager.instance.blackHole.getRadiusBlackHole();
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, radius, whatIsEnemy);
        if(colliders.Length > 0)
            enemyTransform = colliders[Random.Range(0,colliders.Length)].transform;
    }
}
