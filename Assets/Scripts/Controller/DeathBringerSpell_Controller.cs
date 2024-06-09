using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathBringerSpell_Controller : MonoBehaviour
{
    [SerializeField] private Vector2 boxSize;
    [SerializeField] private Transform Check;
    private CharacterStat myStat;
    public void setUpSpell(CharacterStat _stat) => myStat = _stat;
    private void animationAttackTrigger()
    {
        Collider2D[] colliders = Physics2D.OverlapBoxAll(Check.position,boxSize,0);
        foreach(var hit in colliders)
        {
            if(hit.GetComponent<Player>() != null)
            {
                myStat.doDamage(hit.GetComponent<CharacterStat>());
            }
        }
    }
    private void selfDestroy() => Destroy(gameObject);
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(Check.position,boxSize);
    }
}
