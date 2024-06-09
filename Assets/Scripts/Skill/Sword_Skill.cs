using UnityEngine;
using UnityEngine.UI;

public enum SwordTpye
{
    Regular,
    Bounce,
    Pierce,
    Spin
}
public class Sword_Skill : Skill
{
    public SwordTpye swordType = SwordTpye.Regular;
    [Header("Bounce info")]
    [SerializeField] private UI_SkillTreeSlot bounceUnlockButton;
    public bool bounceUnlocked { get; private set; }
    [SerializeField] private int bounceAmount;
    [SerializeField] private float bounceGravity;
    [SerializeField] private float bounceSpeed;

    [Header("Pierce info")]
    [SerializeField] private UI_SkillTreeSlot pierceUnlockButton;
    public bool pierceUnlocked { get; private set; }
    [SerializeField] private int pierceAmount;
    [SerializeField] private float pierceGravity;

    [Header("Spin info")]
    [SerializeField] private UI_SkillTreeSlot spinUnlockButton;
    public bool spinUnlocked { get; private set; }
    [SerializeField] private float spinDuration;
    [SerializeField] private float maxTravelDistance = 7;
    [SerializeField] private float spinGravity;
    [SerializeField] private float hitDuration = 0.35f;

    [Header("Skill info")]
    [SerializeField] private UI_SkillTreeSlot swordUnlockButton;
    public bool swordUnlocked { get; private set; }
    [SerializeField] private GameObject swordPrefab;
    [SerializeField] private Vector2 launchForce;
    [SerializeField] private float swordGravity;
    [SerializeField] private float freezeTimeDuration;
    [SerializeField] private float returnSpeed;

    [Header("Passive skills")]
    [SerializeField] private UI_SkillTreeSlot timeStopUnlockButton;
    public bool timeStopUnlocked { get; private set; }
    [SerializeField] private UI_SkillTreeSlot vulnerableUnlockButton;
    public bool vulnerableUnlocked { get;private set; }


    Vector2 finalDirection;

    [Header("Aim dots")]
    [SerializeField] private GameObject dotPrefab;
    [SerializeField] private int numberOfDots;
    [SerializeField] private float spaceBetweenDots;
    [SerializeField] private Transform dotParent;
    private GameObject[] dots;
    protected override void Start()
    {
        base.Start();
        generateDots();
        setUpGravity();
        timeStopUnlockButton.GetComponent<Button>().onClick.AddListener(unlockTimeStop);
        vulnerableUnlockButton.GetComponent<Button>().onClick.AddListener(unlockVulnerable);
        swordUnlockButton.GetComponent<Button>().onClick.AddListener(unlockSword);
        bounceUnlockButton.GetComponent<Button>().onClick.AddListener(unlockBounceSword);
        pierceUnlockButton.GetComponent<Button>().onClick.AddListener(unlockPierceSword);
        spinUnlockButton.GetComponent<Button>().onClick.AddListener(unlockSpinSword);
    }
    protected override void Update()
    {
        if (Input.GetMouseButtonUp(1))
            finalDirection = new Vector2(aimDirection().normalized.x * launchForce.x, aimDirection().normalized.y * launchForce.y);

        if (Input.GetMouseButton(1))
        {
            for (int i = 0; i < dots.Length; i++)
            {
                dots[i].transform.position = dotsPosition(i * spaceBetweenDots);
            }
        }
    }
    protected override void checkForUnlockSkill()
    {
        unlockTimeStop();
        unlockVulnerable();
        unlockSword();
        unlockBounceSword();
        unlockPierceSword();
        unlockSpinSword();
    }
    #region Unlock Sword Skills And Passive Skills
    private void unlockTimeStop()
    {
        if(timeStopUnlockButton.unlocked)
            timeStopUnlocked = true;
    }
    private void unlockVulnerable()
    {
        if(vulnerableUnlockButton.unlocked)
            vulnerableUnlocked = true;
    }
    private void unlockSword()
    {
        if (swordUnlockButton.unlocked)
        {
            swordType = SwordTpye.Regular;
            swordUnlocked = true;
        }
    }
    private void unlockBounceSword()
    {
        if (bounceUnlockButton.unlocked)
        {
            swordType = SwordTpye.Bounce;
            bounceUnlocked = true;
        }
    }
    private void unlockPierceSword()
    {
        if (pierceUnlockButton.unlocked)
        {
            swordType = SwordTpye.Pierce;
            pierceUnlocked = true;
        }
    }
    private void unlockSpinSword()
    {
        if (spinUnlockButton.unlocked)
        {
            swordType = SwordTpye.Spin;
            spinUnlocked = true;
        }
    }
    #endregion
    private void setUpGravity()
    {
        if (swordType == SwordTpye.Bounce)
            swordGravity = bounceGravity;
        else if (swordType == SwordTpye.Pierce)
            swordGravity = pierceGravity;
        else if (swordType == SwordTpye.Spin)
            swordGravity = spinGravity;
    }
    public void createSword()
    {
        GameObject newSword = Instantiate(swordPrefab, player.transform.position, transform.rotation);
        Sword_Skill_Controller newSwordScript = newSword.GetComponent<Sword_Skill_Controller>();
        if (swordType == SwordTpye.Bounce)
            newSwordScript.setUpBounce(true, bounceAmount, bounceSpeed);
        else if (swordType == SwordTpye.Pierce)
            newSwordScript.setUpPierce(pierceAmount);
        else if (swordType == SwordTpye.Spin)
            newSwordScript.setUpSpin(true, spinDuration, maxTravelDistance, hitDuration);

        newSwordScript.setUpSword(finalDirection, swordGravity, player, freezeTimeDuration, returnSpeed);
        player.assignNewSword(newSword);
        dotsActive(false);
    }
    #region Aim region
    public Vector2 aimDirection()
    {
        Vector2 playerPosition = player.transform.position;
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = mousePosition - playerPosition;
        return direction;
    }
    public void dotsActive(bool _isActive)
    {
        for (int i = 0; i < dots.Length; i++)
        {
            dots[i].SetActive(_isActive);
        }
    }
    private void generateDots()
    {
        dots = new GameObject[numberOfDots];
        for (int i = 0; i < numberOfDots; i++)
        {
            dots[i] = Instantiate(dotPrefab, player.transform.position, Quaternion.identity, dotParent);
            dots[i].SetActive(false);
        }
    }
    private Vector2 dotsPosition(float t)
    {
        Vector2 position = (Vector2)player.transform.position + new Vector2(
            aimDirection().normalized.x * launchForce.x,
            aimDirection().normalized.y * launchForce.y) * t + .5f * (Physics2D.gravity * swordGravity) * (t * t);
        return position;
    }
    #endregion
}
