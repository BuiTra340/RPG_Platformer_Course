using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum SlimeType { Big,Medium,Small}
public class Enemy_Slime : Enemy
{
    [Header("Slime type")]
    public SlimeType slimeSpecific;
    [SerializeField] private GameObject slimePrefab;
    [SerializeField] private int createsToSlime;
    [SerializeField] private float minVelocityX;
    [SerializeField] private float maxVelocityX;

    #region States
    public SlimeIdleState idleState { get; private set; }
    public SlimeMoveState moveState { get; private set; }
    public SlimeBattleState battleState { get; private set; }
    public SlimeAttackState attackState { get; private set; }
    public SlimeStunnedState stunnedState { get; private set; }
    public SlimeDeathState deathState { get; private set; }
    #endregion
    protected override void Awake()
    {
        base.Awake();
        setUpDefaultDirection(-1);
        idleState = new SlimeIdleState(stateMachine, this, "Idle", this);
        moveState = new SlimeMoveState(stateMachine, this, "Move", this);
        battleState = new SlimeBattleState(stateMachine, this, "Move", this);
        attackState = new SlimeAttackState(stateMachine, this, "Attack", this);
        stunnedState = new SlimeStunnedState(stateMachine, this, "Stunned", this);
        deathState = new SlimeDeathState(stateMachine, this, "Idle", this);
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

        if (slimeSpecific == SlimeType.Small)
            return;
        createSlime();
    }
    public void createSlime()
    {
        for(int i=0;i< createsToSlime;i++)
        {
            float xVelocity = Random.Range(minVelocityX, maxVelocityX) * 1.5f;
            GameObject newSlime = Instantiate(slimePrefab, transform.position, Quaternion.identity);
            newSlime.GetComponent<Rigidbody2D>().velocity = new Vector2(xVelocity, Random.Range(1,3));
        }
    }
}
