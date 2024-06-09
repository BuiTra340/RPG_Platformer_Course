using UnityEngine;
[CreateAssetMenu(fileName = "Health Effect", menuName = "Data/Item Effect/Health Effect")]
public class Health_Effect : ItemEffect
{
    [Range(0f, 1f)]
    [SerializeField] private float healthPercent;
    public override void executeEffect(Transform _enemyTransform)
    {
        PlayerStats player = PlayerManager.instance.player.GetComponent<PlayerStats>();
        int healAmount = Mathf.RoundToInt(player.getMaxHealthValue() * healthPercent);
        player.increaseHealBy(healAmount);
    }
}
