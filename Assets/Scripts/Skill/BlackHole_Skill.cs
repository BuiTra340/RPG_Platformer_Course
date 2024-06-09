using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlackHole_Skill : Skill
{
    [SerializeField] private UI_SkillTreeSlot blackHoleUnlockButton;
    public bool blackHoleUnlocked { get; private set; }
    [SerializeField] private GameObject blackHolePrefab;
    [SerializeField] private float maxSize;
    [SerializeField] private float growTimer;
    [SerializeField] private float cloneAttackCooldown;
    [SerializeField] private float growSpeed;
    [SerializeField] private float shrinkSpeed;
    [SerializeField] private int numberOfAttack;
    BlackHole_Skill_Controller currentBlackHole;
    public override void useSkill()
    {
        base.useSkill();
        GameObject newBlackHole = Instantiate(blackHolePrefab,player.transform.position,Quaternion.identity);
        currentBlackHole = newBlackHole.GetComponent<BlackHole_Skill_Controller>();
        currentBlackHole.setUpBlackHole(maxSize,growSpeed,numberOfAttack,shrinkSpeed,cloneAttackCooldown, growTimer);
        AudioManager.instance.PlaySFX(3,player.transform);
        AudioManager.instance.PlaySFX(6,player.transform);
    }

    protected override void Start()
    {
        base.Start();
        blackHoleUnlockButton.GetComponent<Button>().onClick.AddListener(unlockBlackHole);
    }

    protected override void Update()
    {
        base.Update();
    }
    protected override void checkForUnlockSkill()
    {
        unlockBlackHole();
    }
    private void unlockBlackHole()
    {
        if (blackHoleUnlockButton.unlocked)
            blackHoleUnlocked = true;
    }
    public bool blackHoleFinished()
    {
        if (!currentBlackHole) return false;
        if(currentBlackHole.playerCanExitState)
        {
            currentBlackHole = null;
            return true;
        }
        return false;
    }
    public float getRadiusBlackHole() => maxSize / 2;
}
