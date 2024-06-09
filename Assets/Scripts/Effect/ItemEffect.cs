using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item Data", menuName = "Data/Item Effect")]
public class ItemEffect : ScriptableObject
{
    [TextArea]
    public string description;
    public virtual void executeEffect(Transform _enemyTransform)
    {
        Debug.Log("executed effect");
    }
}
