using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Crystal_Skill : Skill
{
    [Header("Crystal simple")]
    [SerializeField] private GameObject crystalPrefab;
    [SerializeField] private float crystalDuration;
    [SerializeField] private UI_SkillTreeSlot crystalUnlockButton;
    public bool crystalUnlocked { get; private set; }

    [Header("Explosive crystal")]
    [SerializeField] private UI_SkillTreeSlot explosiveUnlockButton;
    public bool canExplosive { get; private set; }

    [Header("Crystal mirage")]
    [SerializeField] private UI_SkillTreeSlot crystalMirageUnlockButton;
    public bool cloneInsteadOfCrystal { get; private set; }

    [Header("Moving crystal")]
    [SerializeField] private UI_SkillTreeSlot crystalMoveToEnemyButton;
    public bool canMoveToEnemy { get; private set; }
    [SerializeField] private float moveSpeed;
    private GameObject currentCrystal;

    [Header("Multi Stack Crystal")]
    [SerializeField] private UI_SkillTreeSlot multiStacksCrystalButton;
    public bool canUseMultiStacks { get; private set; }
    [SerializeField] private int amountOfStacks;
    [SerializeField] private float multiStackCooldown;
    [SerializeField] private float useTimeWindow;
    [SerializeField] private List<GameObject> crystalLeft = new List<GameObject>();
    protected override void Start()
    {
        base.Start();
        crystalUnlockButton.GetComponent<Button>().onClick.AddListener(unlockCrystalSimple);
        explosiveUnlockButton.GetComponent<Button>().onClick.AddListener(unlockCrystalExplosive);
        crystalMirageUnlockButton.GetComponent<Button>().onClick.AddListener(unlockCrystalMirage);
        crystalMoveToEnemyButton.GetComponent<Button>().onClick.AddListener(unlockCrystalMovingToEnemy);
        multiStacksCrystalButton.GetComponent<Button>().onClick.AddListener(unlockMultiStacksCrystal);
    }
    protected override void checkForUnlockSkill()
    {
        unlockCrystalSimple();
        unlockCrystalExplosive();
        unlockCrystalMirage();
        unlockCrystalMovingToEnemy();
        unlockMultiStacksCrystal();
    }
    #region Unlock Crystal Skills
    private void unlockCrystalSimple()
    {
        if (crystalUnlockButton.unlocked)
            crystalUnlocked = true;
    }
    private void unlockCrystalExplosive()
    {
        if (explosiveUnlockButton.unlocked)
            canExplosive = true;
    }
    private void unlockCrystalMirage()
    {
        if (crystalMirageUnlockButton.unlocked)
            cloneInsteadOfCrystal = true;
    }
    private void unlockCrystalMovingToEnemy()
    {
        if (crystalMoveToEnemyButton.unlocked)
            canMoveToEnemy = true;
    }
    private void unlockMultiStacksCrystal()
    {
        if (multiStacksCrystalButton.unlocked)
            canUseMultiStacks = true;
    }
    #endregion
    public override void useSkill()
    {
        base.useSkill();

        if (CanUseMultiCrystal()) return;

        if(currentCrystal == null)
        {
            currentCrystal = Instantiate(crystalPrefab, player.transform.position, Quaternion.identity);
            Crystal_Skill_Controller currentCrystalScript = currentCrystal.GetComponent<Crystal_Skill_Controller>();
            currentCrystalScript.setUpCrystal(crystalDuration,canExplosive,canMoveToEnemy,moveSpeed, faceEnemyClosest(currentCrystal.transform),player);
        }
        else
        {
            if (canMoveToEnemy) return;

            Vector3 playerPos = player.transform.position;
            player.transform.position = currentCrystal.transform.position;
            currentCrystal.transform.position = playerPos;
            if (cloneInsteadOfCrystal)
            {
                SkillManager.instance.clone.createClone(currentCrystal.transform, Vector3.zero);
                Destroy(currentCrystal);
            }

            currentCrystal.GetComponent<Crystal_Skill_Controller>()?.finishCrystal();
        }
    }
    public void createCrystal()
    {
        currentCrystal = Instantiate(crystalPrefab, player.transform.position, Quaternion.identity);
        Crystal_Skill_Controller currentCrystalScript = currentCrystal.GetComponent<Crystal_Skill_Controller>();
        currentCrystalScript.setUpCrystal(crystalDuration, canExplosive, canMoveToEnemy, moveSpeed, faceEnemyClosest(currentCrystal.transform),player);
    }
    public override bool canUseSkill()
    {
        return base.canUseSkill();
    }
    protected override void Update()
    {
        base.Update();
    }
    private void refilCrystal()
    {
        int amountToAdd = amountOfStacks - crystalLeft.Count;
        for (int i = 0;i< amountToAdd; i++)
        {
            crystalLeft.Add(crystalPrefab);
        }    
    }
    private bool CanUseMultiCrystal()
    {
        if(canUseMultiStacks)
        {
            if(crystalLeft.Count > 0)
            {
                if (crystalLeft.Count == amountOfStacks)
                    Invoke("resetAbility", useTimeWindow);
                cooldown = 0;
                
                GameObject crystalToSpawn = crystalLeft[crystalLeft.Count - 1];
                GameObject newCrystal = Instantiate(crystalToSpawn, player.transform.position, Quaternion.identity);
                crystalLeft.Remove(crystalToSpawn);
                newCrystal.GetComponent<Crystal_Skill_Controller>().setUpCrystal(crystalDuration, canExplosive, canMoveToEnemy, moveSpeed, faceEnemyClosest(newCrystal.transform),player);

                if(crystalLeft.Count == 0)
                {
                    cooldown = multiStackCooldown;
                    refilCrystal();
                }
                return true;
            }
        }
        return false;
    }
    private void resetAbility()
    {
        if (cooldownTimer > 0) return;
        cooldownTimer = multiStackCooldown;
        refilCrystal();
    }
    public void currentCrystalChooseRandomEnemyTarget() 
        => currentCrystal.GetComponent<Crystal_Skill_Controller>().chooseRandomEnemy();
}
