using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Clone_Skill : Skill
{
    [Header("Clone info")]
    [SerializeField] private float attackMultiplier;
    [SerializeField] private GameObject clonePrefab;
    [SerializeField] private float cloneDuration;
    [Space]

    [Header("Clone attack")]
    [SerializeField] private float cloneAttackMultiplier;
    [SerializeField] private UI_SkillTreeSlot cloneUnlockButton;
    [SerializeField] private bool canAttack;

    [Header("Aggresive Clone")]
    [SerializeField] private float aggresiveCloneAttackMultiplier;
    [SerializeField] private UI_SkillTreeSlot aggresiveCloneButton;
    public bool aggresiveCloneUnlocked { get; private set; }

    [Header("Multi Clone")]
    [SerializeField] private float multiCloneAttackMultiplier;
    [SerializeField] private UI_SkillTreeSlot multipleCloneUnlockButton;
    [SerializeField] private bool canDuplicateClone;
    [SerializeField] private int chanceToDuplicate;

    [Header("Crystal instead of clone")]
    [SerializeField] private UI_SkillTreeSlot crystalInsteadCloneUnlockButton;
    public bool crystalInsteadOfClone;
    protected override void Start()
    {
        base.Start();
        cloneUnlockButton.GetComponent<Button>().onClick.AddListener(unlockClone);
        aggresiveCloneButton.GetComponent<Button>().onClick.AddListener(unlockAggresiveClone);
        multipleCloneUnlockButton.GetComponent<Button>().onClick.AddListener(unlockMultipleClone);
        crystalInsteadCloneUnlockButton.GetComponent<Button>().onClick.AddListener(unlockCrystalInsteadClone);
    }
    protected override void checkForUnlockSkill()
    {
        unlockClone();
        unlockAggresiveClone();
        unlockMultipleClone();
        unlockCrystalInsteadClone();
    }
    #region Unlock region
    private void unlockClone()
    {
        if(cloneUnlockButton.unlocked)
        {
            canAttack = true;
            attackMultiplier = cloneAttackMultiplier;
        }

    }
    private void unlockAggresiveClone()
    {
        if(aggresiveCloneButton.unlocked)
        {
            aggresiveCloneUnlocked = true;
            attackMultiplier = aggresiveCloneAttackMultiplier;
        }
    }
    private void unlockMultipleClone()
    {
        if(multipleCloneUnlockButton.unlocked)
        {
            canDuplicateClone = true;
            attackMultiplier = multiCloneAttackMultiplier;
        }
    }
    private void unlockCrystalInsteadClone()
    {
        if(crystalInsteadCloneUnlockButton.unlocked)
        {
            crystalInsteadOfClone = true;
        }
    }
    #endregion

    public void createClone(Transform _newClone,Vector3 offSet)
    {
        if(crystalInsteadOfClone)
        {
            SkillManager.instance.crystal.createCrystal();
            return;
        }
        GameObject newClone = Instantiate(clonePrefab);
        newClone.GetComponent<Clone_Skill_Controller>().setUpCloneSkill(_newClone, cloneDuration, canAttack,offSet,faceEnemyClosest(newClone.transform),canDuplicateClone,chanceToDuplicate,player, attackMultiplier);
        int randomSound = Random.Range(37, 41);
        AudioManager.instance.PlaySFX(randomSound,null);
    }
    public void createCloneWithDelay(Transform _enemyTransform)
    {
        StartCoroutine(createCloneCorountine(_enemyTransform, .4f, new Vector3(2 * player.facingDir, 0)));
    }
    private IEnumerator createCloneCorountine(Transform _transform,float _timeDelay, Vector3 _offSet)
    {
        yield return new WaitForSeconds(_timeDelay);
        createClone(_transform, _offSet);
    }
}
