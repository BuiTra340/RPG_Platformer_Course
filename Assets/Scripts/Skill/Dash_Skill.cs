using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dash_Skill : Skill
{
    [Header("Dash")]
    [SerializeField] private UI_SkillTreeSlot dashUnlockButton;
    public bool dashUnlock { get; private set; }

    [Header("Clone on dash")]
    [SerializeField] private UI_SkillTreeSlot cloneOnDashUnlockButton;
    public bool cloneOnDash { get; private set; }

    [Header("Clone on arrival")]
    [SerializeField] private UI_SkillTreeSlot cloneOnArrivalUnlockButton;
    public bool cloneOnArrival { get; private set; }
    protected override void Start()
    {
        base.Start();
        dashUnlockButton.GetComponent<Button>().onClick.AddListener(() => unlockDash());
        cloneOnDashUnlockButton.GetComponent<Button>().onClick.AddListener(() => unlockCloneOnDash());
        cloneOnArrivalUnlockButton.GetComponent<Button>().onClick.AddListener(() => unlockCloneOnArrival());
    }
    public override void useSkill()
    {
        base.useSkill();
    }
    protected override void checkForUnlockSkill()
    {
        unlockDash();
        unlockCloneOnDash();
        unlockCloneOnArrival();
    }
    #region Unlock Region
    public void unlockDash()
    {
        if(dashUnlockButton.unlocked == true)
            dashUnlock = true;
    }
    public void unlockCloneOnDash()
    {
        if (cloneOnDashUnlockButton.unlocked == true)
            cloneOnDash = true;
    }
    public void unlockCloneOnArrival()
    {
        if (cloneOnArrivalUnlockButton.unlocked == true)
            cloneOnArrival = true;
    }
    #endregion
    public void CreateCloneOnDash()
    {
        if (cloneOnDash)
        {
            SkillManager.instance.clone.createClone(player.transform, Vector3.zero);
        }
    }
    public void CreateCloneOnDashArrival()
    {
        if (cloneOnArrival)
        {
            SkillManager.instance.clone.createClone(player.transform, Vector3.zero);
        }
    }
}
