using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathBringerSpellCastState : EnemyState
{
    Enemy_DeathBringer enemy;
    private int amountOfSpell;
    public DeathBringerSpellCastState(EnemyStateMachine _stateMachine, Enemy _enemyBase, string _stringBoolName, Enemy_DeathBringer _enemy) : base(_stateMachine, _enemyBase, _stringBoolName)
    {
        enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();
        amountOfSpell = enemy.amoutOfSpell;
        stateTimer = enemy.spellCooldown;
    }

    public override void Exit()
    {
        base.Exit();
        enemy.lastTimeSpellState = Time.time;
    }

    public override void Update()
    {
        base.Update();
        if(canSpell())
            enemy.createSpell();
        else if(amountOfSpell <= 0)
            stateMachine.changeState(enemy.battleState);
    }
    private bool canSpell()
    {
        if(stateTimer < 0 && amountOfSpell > 0)
        {
            amountOfSpell--;
            stateTimer = enemy.spellCooldown;
            return true;
        }
        return false;
    }
}
