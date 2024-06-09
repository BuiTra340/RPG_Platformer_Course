using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathBringerTeleportState : EnemyState
{
    Enemy_DeathBringer enemy;
    public DeathBringerTeleportState(EnemyStateMachine _stateMachine, Enemy _enemyBase, string _stringBoolName, Enemy_DeathBringer _enemy) : base(_stateMachine, _enemyBase, _stringBoolName)
    {
        enemy = _enemy;
    }
    public override void Enter()
    {
        base.Enter();
        Debug.Log("In teleport State");
        stateTimer = 1;
    }
    public override void Update()
    {
        base.Update();
        if(stateTimer < 0)
            stateMachine.changeState(enemy.spellCastState);
    }
}
