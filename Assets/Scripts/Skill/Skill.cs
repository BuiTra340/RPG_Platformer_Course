using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : MonoBehaviour
{
    public float cooldown;
    public float cooldownTimer;
    protected Player player;
    protected virtual void Start()
    {
        player = PlayerManager.instance.player;
        Invoke("checkForUnlockSkill", .1f);
    }
    protected virtual void Update()
    {
        cooldownTimer -=Time.deltaTime;
    }
    public virtual bool canUseSkill()
    {
        if(cooldownTimer <= 0)
        {
            useSkill();
            cooldownTimer = cooldown;
            return true;
        }
        player.fx.createPopUpText("Cooldown");
        return false;
    }
    public virtual void useSkill() { }
    protected virtual void checkForUnlockSkill() { }
    protected virtual Transform faceEnemyClosest(Transform _ckeckTransform)
    {
        float closestDistance = Mathf.Infinity;
        Transform enemyClosest = null;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(_ckeckTransform.position, 25);
        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null)
            {
                float distanceToEnemy = Vector2.Distance(_ckeckTransform.position, hit.transform.position);
                if (distanceToEnemy < closestDistance)
                {
                    closestDistance = distanceToEnemy;
                    enemyClosest = hit.transform;
                }
            }
        }
        return enemyClosest;
    }
}
