using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosive_Controller : MonoBehaviour
{
    Animator anim;
    private float maxSize;
    private float growSpeed;
    private bool canGrow = true;
    private float attackRadius;
    private CharacterStat myStat;
    public void setUpExplosive(float _maxSize,float _growSpeed,float _attackRadius,CharacterStat _myStat)
    {
        anim = GetComponent<Animator>();
        maxSize = _maxSize;
        growSpeed = _growSpeed;
        attackRadius = _attackRadius;
        myStat = _myStat;
    }

    // Update is called once per frame
    void Update()
    {
        if(canGrow)
            transform.localScale = Vector2.Lerp(transform.localScale,new Vector2(maxSize,maxSize),growSpeed*Time.deltaTime);
        if(maxSize - transform.localScale.x < .5f)
        {
            canGrow = false;
            anim.SetTrigger("Explosive");
        }
    }
    private void animationTrigger()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, attackRadius);
        foreach(var hit in colliders)
        {
            if(hit.GetComponent<Player>() != null)
            {
                myStat.doDamage(hit.GetComponent<CharacterStat>());
            }
        }
    }
    public void selfDestroy() => Destroy(gameObject);
}
