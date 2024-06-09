using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackHole_Skill_Controller : MonoBehaviour
{
    [SerializeField] private GameObject hotKeyPrefab;
    [SerializeField] private List<KeyCode> keyCodeLists;

    private bool canGrow = true;
    private float maxSize;
    private float growSpeed;
    private int numberOfAttack;
    private float cloneAttackCooldown;
    private float shrinkSpeed;
    private float clonetAttackTimer;
    private bool cloneAttackRelease;
    private bool canCreateHotKeys = true;
    private bool canShrink;
    private float growTimer;
    private bool playerCanDisapear = true;
    public bool playerCanExitState { get; private set; }

    public List<Transform> targets;
    private List<GameObject> createdHotKeys = new List<GameObject>();

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            cloneAttackAbility();
        }

        cloneAttackLogic();
        if (canGrow && !canShrink)
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(maxSize, maxSize), growSpeed * Time.deltaTime);

        if (canShrink)
        {
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(-1, -1), shrinkSpeed * Time.deltaTime);
            if (transform.localScale.x < 0)
            {
                Destroy(gameObject);
            }
        }

        growTimer -= Time.deltaTime;
        if(!cloneAttackRelease && growTimer <= 0)
        {
            Invoke("finishBlackHoleAbility", 1f);
        }
    }

    private void cloneAttackAbility()
    {
        if(targets.Count <= 0) return;
        growTimer = 0;
        destroyHotKeys();
        cloneAttackRelease = true;
        canCreateHotKeys = false;
        if(playerCanDisapear)
        {
            playerCanDisapear = false;
            PlayerManager.instance.player.fx.makeTransparent(true);
        }
    }

    public void setUpBlackHole(float _maxSize,float _growSpeed,int _numberOfAttack,float _shrinkSpeed,float _cloneAttackCooldown,float _growTimer)
    {
        maxSize = _maxSize;
        growSpeed = _growSpeed;
        numberOfAttack = _numberOfAttack;
        shrinkSpeed = _shrinkSpeed;
        cloneAttackCooldown = _cloneAttackCooldown;
        growTimer = _growTimer;

        if(SkillManager.instance.clone.crystalInsteadOfClone)
            playerCanDisapear = false;
    }

    private void cloneAttackLogic()
    {
        clonetAttackTimer -= Time.deltaTime;
        if (clonetAttackTimer <= 0 && cloneAttackRelease && numberOfAttack > 0)
        {
            clonetAttackTimer = cloneAttackCooldown;
            float xOffset;
            if (Random.Range(0, 100) > 50)
                xOffset = 2;
            else xOffset = -2;
            if (SkillManager.instance.clone.crystalInsteadOfClone)
            {
                SkillManager.instance.crystal.createCrystal();
                SkillManager.instance.crystal.currentCrystalChooseRandomEnemyTarget();
            }else
            {
                SkillManager.instance.clone.createClone(targets[Random.Range(0, targets.Count)], new Vector3(xOffset, 0));
            }
            numberOfAttack--;

            if (numberOfAttack <= 0)
            {
                Invoke("finishBlackHoleAbility", 1f);
            }
        }
    }

    private void finishBlackHoleAbility()
    {
        destroyHotKeys();
        cloneAttackRelease = false;
        canShrink = true;
        playerCanExitState = true;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<Enemy>() != null)
        {
            collision.GetComponent<Enemy>().freezeTime(false);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!canCreateHotKeys) return;

        if (collision.GetComponent<Enemy>() != null)
        {
            collision.GetComponent<Enemy>().freezeTime(true);

            createHotKeys(collision);
        }
    }

    private void createHotKeys(Collider2D collision)
    {
        if (keyCodeLists.Count <= 0)
        {
            Debug.Log("not enough hot key in list");
            return;
        }
        GameObject newHotKey = Instantiate(hotKeyPrefab, collision.transform.position + new Vector3(0, 2), Quaternion.identity);
        createdHotKeys.Add(newHotKey);
        KeyCode choosenKey = keyCodeLists[Random.Range(0, keyCodeLists.Count)];
        keyCodeLists.Remove(choosenKey);

        BlackHole_HotKey_Controller newHotKeyScript = newHotKey.GetComponent<BlackHole_HotKey_Controller>();
        newHotKeyScript.setUpHotKey(choosenKey, collision.transform, this);
    }

    public void addEnemyToList(Transform enemyTransform) => targets.Add(enemyTransform);
    private void destroyHotKeys()
    {
        if (createdHotKeys.Count <= 0) return;
        for (int i = 0; i < createdHotKeys.Count; i++)
        {
            Destroy(createdHotKeys[i]);
        }
    }
}
