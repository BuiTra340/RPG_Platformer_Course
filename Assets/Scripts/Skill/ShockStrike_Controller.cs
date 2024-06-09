using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShockStrike_Controller : MonoBehaviour
{
    private CharacterStat targetStat;
    [SerializeField] private float moveSpeed;
    private int damage;
    Animator anim;
    private bool isDamaged;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!targetStat) return;
        if (isDamaged) return;

        transform.right = targetStat.transform.position - transform.position;
        transform.position = Vector2.MoveTowards(transform.position,targetStat.transform.position,moveSpeed * Time.deltaTime);
        if(Vector2.Distance(transform.position,targetStat.transform.position) < .1f)
        {
            transform.localRotation = Quaternion.identity;
            anim.transform.localRotation = Quaternion.identity;
            anim.transform.localPosition = new Vector3(0, .5f);
            transform.localScale = new Vector3(3, 3, 3);
            Invoke("damageAndSelfDistroy", .2f);
            isDamaged = true;
            anim.SetTrigger("Hit");
        }
    }

    private void damageAndSelfDistroy()
    {
        targetStat.applyShock(true);
        targetStat.takeDamge(damage);
        Destroy(gameObject, .4f);
    }

    public void setUpShockStrike(int _damage,CharacterStat _targetStat)
    {
        damage = _damage;
        targetStat = _targetStat;
    }
}
