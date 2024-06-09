using UnityEngine;
[CreateAssetMenu(fileName = "Fire and Ice effect", menuName = "Data/Item Effect/Fire and Ice")]
public class FireAndIce_Effect : ItemEffect
{
    [SerializeField] private GameObject fireAndIcePrefab;
    [SerializeField] private float xVelocity;
    public override void executeEffect(Transform _enemyTransform)
    {
        Player player = PlayerManager.instance.player;
        bool thirdAttack = player.attackState.comboCounter == 2;
        if (thirdAttack)
        {
            GameObject newFireAndIce = Instantiate(fireAndIcePrefab, _enemyTransform.position, player.transform.rotation);
            newFireAndIce.GetComponent<Rigidbody2D>().velocity = new Vector2(xVelocity * player.facingDir, 0);
            Destroy(newFireAndIce,3f);
        }
    }
}
