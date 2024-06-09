using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Thunder Strike Effect", menuName = "Data/Item Effect/Thunder effect")]
public class ThunderStrike_Effect : ItemEffect
{
    [SerializeField] private GameObject thunderStrikePrefab;
    public override void executeEffect(Transform _enemyTransform)
    {
        GameObject newThunder = Instantiate(thunderStrikePrefab,_enemyTransform.position,Quaternion.identity);
        Destroy(newThunder,1); 
    }
}
