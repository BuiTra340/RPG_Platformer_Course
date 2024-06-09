using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCounterAttackState : PlayerState
{
    private bool cloneCreated;
    public PlayerCounterAttackState(PlayerStateMachine stateMachine, Player _player, string _animBoolName) : base(stateMachine, _player, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        stateTimer = player.counterDuration;
        player.anim.SetBool("SuccessfulCounterAttack", false);
        cloneCreated = true;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        player.zeroVelocity();
        Collider2D[] colliders = Physics2D.OverlapCircleAll(player.transform.position, player.attackRadius);
        foreach(var hit in colliders)
        {
            if (hit.GetComponent<Arrow_Controller>() != null)
            {
                hit.GetComponent<Arrow_Controller>().flipArrow();
                counterSuccessful();
            }

            if (hit.GetComponent<Enemy>() != null)
            {
                if(hit.GetComponent<Enemy>().CanBeStunned())
                {
                    player.skill.parry.useSkill();
                    counterSuccessful();

                    if (cloneCreated)
                    {
                        player.skill.parry.makeMirageOnParry(hit.transform);
                        cloneCreated = false;
                    }
                }
            }
        }
        if(stateTimer <= 0 || triggerCalled)
            stateMachine.ChangeState(player.idleState);
    }

    private void counterSuccessful()
    {
        stateTimer = 10; // any value bigger than 1 for implement anim SuccessfulCounterAttack
        player.anim.SetBool("SuccessfulCounterAttack", true);
    }
}
