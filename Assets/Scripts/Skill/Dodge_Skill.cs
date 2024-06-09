using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dodge_Skill : Skill
{
    [Header("Dodge")]
    [SerializeField] private UI_SkillTreeSlot dodgeUnlockButton;
    [SerializeField] private int dodgeAmount;
    public bool dodgeUnlocked { get; private set; }
    [Header("Dodge mirage")]
    [SerializeField] private UI_SkillTreeSlot dodgeMirageUnlockButton;
    public bool dodgeMirageUnlocked { get; private set; }
    protected override void Start()
    {
        base.Start();
        dodgeUnlockButton.GetComponent<Button>().onClick.AddListener(unlockDodge);
        dodgeMirageUnlockButton.GetComponent<Button>().onClick.AddListener(unlockDodgeMirage);
    }
    protected override void checkForUnlockSkill()
    {
        unlockDodge();
        unlockDodgeMirage();
    }
    #region Unlock Region
    private void unlockDodge()
    {
        if(dodgeUnlockButton.unlocked)
        {
            player.stats.evasion.addModifier(dodgeAmount);
            Inventory.instance.updateStatUI();
            dodgeUnlocked = true;
        }
    }
    private void unlockDodgeMirage()
    {
        if (dodgeMirageUnlockButton.unlocked)
            dodgeMirageUnlocked = true;
    }
    #endregion
    public void createMirageOnDodge(int _enemyDir)
    {
        if (dodgeMirageUnlocked)
            SkillManager.instance.clone.createClone(player.transform, new Vector3(2 * _enemyDir, 0));
    }
}
