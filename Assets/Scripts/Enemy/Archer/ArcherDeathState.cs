using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherDeathState : EnemyState
{
    Enemy_Archer enemy;
    public ArcherDeathState(EnemyStateMachine _stateMachine, Enemy _enemyBase, string _stringBoolName, Enemy_Archer _enemy) : base(_stateMachine, _enemyBase, _stringBoolName)
    {
        enemy = _enemy;
    }
    public override void Enter()
    {
        base.Enter();
        enemy.anim.SetBool(enemy.lastAnimBoolName, true);
        enemy.anim.speed = 0;
        enemy.capsuleCollider2D.enabled = false;
        stateTimer = .15f;
    }

    public override void Update()
    {
        base.Update();
        if (stateTimer > 0)
            rb.velocity = new Vector2(0, 5);
    }
}
