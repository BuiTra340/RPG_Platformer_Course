using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadyDeathState : EnemyState
{
    Enemy_Shady enemy;
    public ShadyDeathState(EnemyStateMachine _stateMachine, Enemy _enemyBase, string _stringBoolName, Enemy_Shady _enemy) : base(_stateMachine, _enemyBase, _stringBoolName)
    {
        enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();
    }
    public override void Exit()
    {
        base.Exit();
        enemy.anim.SetBool(enemy.lastAnimBoolName, true);
        enemy.anim.speed = 0;
        enemy.capsuleCollider2D.enabled = false;
    }

    public override void Update()
    {
        base.Update();
        
    }
}
