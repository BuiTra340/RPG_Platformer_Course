using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_DeathBringer : Enemy
{
    [Header("DeathBringer Teleport")]
    [SerializeField] private GameObject spellPrefab;
    public int amoutOfSpell;
    public float spellCooldown;
    public bool bossFightBegun;
    public float spellStateCooldown;
    [HideInInspector]public float lastTimeSpellState;
    public int chanceToTeleport;
    public int defaultChanceToTeleport;
    [SerializeField] private BoxCollider2D arena;
    [SerializeField] private Vector2 surroundingCheckSize;
    #region States
    public DeathBringerIdleState idleState { get; private set; }
    public DeathBringerBattleState battleState { get; private set; }
    public DeathBringerAttackState attackState { get; private set; }
    public DeathBringerSpellCastState spellCastState { get; private set; }
    public DeathBringerTeleportState teleportState { get; private set; }
    public DeathBringerDeathState deathState { get; private set; }
    #endregion
    protected override void Awake()
    {
        base.Awake();
        idleState = new DeathBringerIdleState(stateMachine, this, "Idle", this);
        battleState = new DeathBringerBattleState(stateMachine, this, "Move", this);
        attackState = new DeathBringerAttackState(stateMachine, this, "Attack", this);
        spellCastState = new DeathBringerSpellCastState(stateMachine, this, "SpellCast", this);
        teleportState = new DeathBringerTeleportState(stateMachine, this, "Teleport", this);
        deathState = new DeathBringerDeathState(stateMachine, this, "Idle", this);
    }
    protected override void Start()
    {
        base.Start();
        stateMachine.Initialize(idleState);
        setUpDefaultDirection(-1);
        chanceToTeleport = defaultChanceToTeleport;
    }

    protected override void Update()
    {
        base.Update();
    }
    public override void Die()
    {
        base.Die();
        stateMachine.changeState(deathState);
    }
    public void findPosition()
    {
        Transform playerPos = PlayerManager.instance.player.transform;
        float x = Random.Range(arena.bounds.min.x + 3, arena.bounds.max.x - 3);
        float y = Random.Range(arena.bounds.min.y + 3, arena.bounds.max.y - 3);
        transform.position = new Vector3(x, y);
        transform.position = new Vector3(transform.position.x, transform.position.y - groundBelow().distance + (capsuleCollider2D.size.y / 2));
        if (playerPos.transform.position.x > transform.position.x)
            Flip();
        if(!groundBelow() || someThingIsAround())
        {
            Debug.Log("looking for Position");
            findPosition();
        }
    }
    private RaycastHit2D groundBelow() => Physics2D.Raycast(transform.position, Vector2.down, 100, whatIsGround);
    private bool someThingIsAround() => Physics2D.BoxCast(transform.position, surroundingCheckSize, 0, Vector2.zero, 0, whatIsGround);
    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x, transform.position.y - groundBelow().distance));
        Gizmos.DrawWireCube(transform.position,surroundingCheckSize);
    }
    public void createSpell()
    {
        Player player = PlayerManager.instance.player;
        float xOffSet = 0;
        if (player.rb.velocity.x != 0)
            xOffSet = player.facingDir * 3;
        Vector3 spellPos = new Vector3(player.transform.position.x + xOffSet, player.transform.position.y + 1.5f);
        GameObject newSpell = Instantiate(spellPrefab, spellPos , Quaternion.identity);
        newSpell.GetComponent<DeathBringerSpell_Controller>().setUpSpell(stats);
    }
}
