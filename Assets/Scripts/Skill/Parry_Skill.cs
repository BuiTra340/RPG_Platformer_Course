using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Parry_Skill : Skill
{
    [Header("Parry")]
    [SerializeField] private UI_SkillTreeSlot parryUnlockButton;
    public bool parryUnlocked { get; private set; }
    [Header("Parry restore")]
    [SerializeField] private UI_SkillTreeSlot parryRestoreUnlockButton;
    public bool parryRestoreUnlocked { get; private set; }
    [Range(0f, 1f)]
    [SerializeField] private float restorePercentage;
    [Header("Parry with mirage")]
    [SerializeField] private UI_SkillTreeSlot parryWithMirageUnlockButton;
    public bool parryWithMirageUnlocked { get;private set; }
    public override void useSkill()
    {
        base.useSkill();
        if(parryRestoreUnlocked)
        {
            int amountRestore = Mathf.RoundToInt(player.stats.getMaxHealthValue() * restorePercentage);
            player.stats.increaseHealBy(amountRestore);
        }
    }
    protected override void Start()
    {
        base.Start();
        parryUnlockButton.GetComponent<Button>().onClick.AddListener(() => unlockParry());
        parryRestoreUnlockButton.GetComponent<Button>().onClick.AddListener(() => unlockParryRestore());
        parryWithMirageUnlockButton.GetComponent<Button>().onClick.AddListener(() => unlockParryWithMirage());
    }
    protected override void checkForUnlockSkill()
    {
        unlockParry();
        unlockParryRestore();
        unlockParryWithMirage();
    }
    public void unlockParry()
    {
        if (parryUnlockButton.unlocked)
            parryUnlocked = true;
    }
    public void unlockParryRestore()
    {
        if (parryRestoreUnlockButton.unlocked)
            parryRestoreUnlocked = true;
    }
    public void unlockParryWithMirage()
    {
        if (parryWithMirageUnlockButton.unlocked)
            parryWithMirageUnlocked = true;
    }
    public void makeMirageOnParry(Transform _enemyTransform)
    {
        if(parryWithMirageUnlocked)
            SkillManager.instance.clone.createCloneWithDelay(_enemyTransform);
    }
}
