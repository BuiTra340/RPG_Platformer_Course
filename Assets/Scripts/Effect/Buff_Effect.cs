using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Buff Effect", menuName = "Data/Item Effect/Buff Effect")]
public class Buff_Effect : ItemEffect
{
    private PlayerStats _stat;
    public StatType statType;
    [SerializeField] private int buffAmount;
    [SerializeField] private float buffDuration;
    public override void executeEffect(Transform _enemyTransform)
    {
        _stat = PlayerManager.instance.player.GetComponent<PlayerStats>();
        _stat.increaseStatBy(buffAmount,buffDuration ,_stat.getModify(statType));
    }
    
}
