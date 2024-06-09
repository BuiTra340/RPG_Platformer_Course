using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow_Controller : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private string layerToAttack = "Player";
    private float xVelocity;
    private bool canMove = true;
    private bool flip;
    private int dir;
    private CharacterStat stat;

    void Update()
    {
        if (canMove)
            rb.velocity = new Vector2(xVelocity * dir, rb.velocity.y);
    }
    public void setUpArrow(float _speed,CharacterStat _stat,int _dir)
    {
        xVelocity = _speed;
        stat = _stat;
        dir = _dir;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer(layerToAttack))
        {
            stat.doDamage(collision.GetComponent<CharacterStat>());
            stuckInto(collision);
        }else if(collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
            stuckInto(collision);
    }

    private void stuckInto(Collider2D collision)
    {
        GetComponent<CapsuleCollider2D>().enabled = false;
        canMove = false;
        rb.isKinematic = true;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        transform.parent = collision.transform;
        Destroy(gameObject, Random.Range(3, 5));
    }

    public void flipArrow()
    {
        if (flip)
            return;
        flip = true;
        xVelocity *= -1;
        transform.Rotate(0, 180, 0);
        layerToAttack = "Enemy";
    }
}
