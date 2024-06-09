using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum StatType
{
    strength,
    agility,
    intelligence,
    vitality,
    armor,
    maxHealth,
    evasion,
    damage,
    critChance,
    critPower,
    magicResistance,
    fireDamage,
    iceDamage,
    lightingDamage
}
public class CharacterStat : MonoBehaviour
{
    public EntityFX fx { get; private set; }
    public Entity entity { get; private set; }
    [SerializeField] private float fxDuration = 2f;
    [Header("Major Stats")]
    public Stat strength; // 1 point increase damage by 1 and crit power by 1%
    public Stat agility;  // 1 point increase evasion by 1% and crit chance by 1%
    public Stat intelligence; // 1 point increase magic damage by 1 and magic resistance by 3
    public Stat vitality; // 1 point increase health by 5 points

    [Header("Defensive Stats")]
    public Stat armor;
    public Stat maxHealth;
    public Stat evasion;

    [Header("Offensive Stats")]
    public Stat damage;
    public Stat critChance;
    public Stat critPower; // default value 150%
    public Stat magicResistance;

    [Header("Magic Stats")]
    public Stat fireDamage;
    public Stat iceDamage;
    public Stat lightingDamage;

    public bool isIgnited; // does damage over time
    public bool isChilled; //  reduce armor by 20%
    public bool isShocked; // reduce accuracy by 20%

    private float ignitedTimer;
    private float chilledTimer;
    private float shockedTimer;

    private float igniteDamageCooldown = .3f;
    private float igniteDamageTimer;
    private int igniteDamage;
    private int shockDamage;
    [SerializeField] private GameObject shockStrikePrefab;

    public int currentHealth;
    public System.Action onHealthUpdated;
    public bool isDead { get; private set; }
    public bool isVulnerable { get; private set; }
    public bool isInvincible { get; private set; }
    protected virtual void Start()
    {
        fx = GetComponentInChildren<EntityFX>();
        entity = GetComponent<Entity>();
        critPower.setDefaultStat(150);
        currentHealth = getMaxHealthValue();
    }
    protected virtual void Update()
    {
        ignitedTimer -= Time.deltaTime;
        chilledTimer -= Time.deltaTime;
        shockedTimer -= Time.deltaTime;

        igniteDamageTimer -= Time.deltaTime;

        if (chilledTimer < 0) isChilled = false;
        if (shockedTimer < 0) isShocked = false;
        if (ignitedTimer < 0)
            isIgnited = false;

        if(isIgnited)
            applyIgniteAilment();
    }
    
    public virtual void doDamage(CharacterStat _target)
    {
        bool criticalFX = false;
        if (_target.isInvincible)
            return;

        if (targetCanAvoidAttack(_target)) return;
        _target.GetComponent<Entity>().setUpKnockBackDir(transform);
        int totalDamage = damage.getValue() + strength.getValue();
        if (canCrit())
        {
            totalDamage = calculateCriticalDamage(totalDamage);
            criticalFX = true;
        }
        fx.createHitFX(_target.transform, criticalFX);
        totalDamage = checkTargetArmor(totalDamage, _target);
        _target.takeDamge(totalDamage);
        doMagicalDamage(_target);
    }

    public virtual void takeDamge(int _damage)
    {
        if (isInvincible)
            return;

        if (isVulnerable)
            _damage = Mathf.RoundToInt(_damage * 1.1f);

        currentHealth -= _damage;
        if(_damage > 0) 
            fx.createPopUpText(_damage.ToString());

        GetComponent<Entity>().damageImpact();
        fx.StartCoroutine("FlashFX");
        if (currentHealth <= 0 && !isDead)
        {
            isDead = true;
            Die();
        }
        onHealthUpdated();
    }
    public virtual void increaseHealBy(int _amount)
    {
        currentHealth += _amount;
        currentHealth = Mathf.Clamp(currentHealth,0,getMaxHealthValue());
        if (onHealthUpdated != null)
            onHealthUpdated();
    }
    public virtual void increaseStatBy(int _modifiers,float _duration,Stat _stat)
    {
        StartCoroutine(statModCoroutine(_modifiers, _duration, _stat));
    }
    private IEnumerator statModCoroutine(int _modifiers, float _duration, Stat _stat)
    {
        _stat.addModifier(_modifiers);
        yield return new WaitForSeconds(_duration);
        _stat.removeModifier(_modifiers);

    }
    public void killEntity() => Die();
    protected virtual void Die() { }
    public virtual void MakeVulnerableFor(float _duration) => StartCoroutine(vulnerableCorountine(_duration));
    private IEnumerator vulnerableCorountine(float _duration)
    {
        isVulnerable = true;
        yield return new WaitForSeconds(_duration);
        isVulnerable = false;
    }
    #region Stat calculations
    protected virtual bool targetCanAvoidAttack(CharacterStat _target)
    {
        int totalEvasion = _target.evasion.getValue() + _target.agility.getValue();
        if (isShocked)
            totalEvasion += 20;
        if (Random.Range(0, 100) < totalEvasion)
        {
            int facingDir = -entity.facingDir;
            _target.onEvasion(facingDir);
            return true;
        }
        return false;
    }
    protected virtual void onEvasion(int _facingDir) { }
    protected virtual int checkTargetArmor(int totalDamage, CharacterStat _target)
    {
        if (_target.isChilled)
        {
            totalDamage -= Mathf.RoundToInt(_target.armor.getValue() * .8f);
        }
        else totalDamage -= _target.armor.getValue();
        totalDamage = Mathf.Clamp(totalDamage, 0, int.MaxValue);
        return totalDamage;
    }
    protected virtual bool canCrit()
    {
        int totalCriticalChance = critChance.getValue() + strength.getValue();
        if (Random.Range(0, 100) <= totalCriticalChance)
        {
            return true;
        }
        return false;
    }
    protected virtual int calculateCriticalDamage(int _damage)
    {
        float totalCritPower = (critPower.getValue() + strength.getValue()) * .01f;
        float totalDamage = _damage * totalCritPower;
        return Mathf.RoundToInt(totalDamage);
    }
    #endregion
    #region Magical Damage and Ailments
    public virtual void doMagicalDamage(CharacterStat _target)
    {
        int _fireDamage = fireDamage.getValue();
        int _iceDamage = iceDamage.getValue();
        int _lightlingDamage = lightingDamage.getValue();
        int totalMagicalDamage = _fireDamage + _iceDamage + _lightlingDamage + intelligence.getValue();

        totalMagicalDamage -= checkMagicalResistanceTarget(_target);

        totalMagicalDamage = Mathf.Clamp(totalMagicalDamage, 0, int.MaxValue);
        _target.takeDamge(totalMagicalDamage);

        if (Mathf.Max(_fireDamage, _iceDamage, _lightlingDamage) <= 0)
            return;
        attempToApplyAilments(_target, _fireDamage, _iceDamage, _lightlingDamage);
    }

    private static void attempToApplyAilments(CharacterStat _target, int _fireDamage, int _iceDamage, int _lightlingDamage)
    {
        bool canApplyIgnite = _fireDamage > _iceDamage && _fireDamage > _lightlingDamage;
        bool canApplyChill = _iceDamage > _fireDamage && _iceDamage > _lightlingDamage;
        bool canApplyShock = _lightlingDamage > _fireDamage && _lightlingDamage > _iceDamage;
        while (!canApplyIgnite && !canApplyChill && !canApplyShock)
        {
            if (Random.value < .3f && _fireDamage > 0)
            {
                canApplyIgnite = true;
                _target.applyAilment(canApplyIgnite, canApplyChill, canApplyShock);
                return;
            }
            if (Random.value < .3f && _iceDamage > 0)
            {
                canApplyChill = true;
                _target.applyAilment(canApplyIgnite, canApplyChill, canApplyShock);
                return;
            }
            if (Random.value < .3f && _lightlingDamage > 0)
            {
                canApplyShock = true;
                _target.applyAilment(canApplyIgnite, canApplyChill, canApplyShock);
                return;
            }
        }
        if (canApplyIgnite) _target.setUpIgniteDamage(Mathf.RoundToInt(_fireDamage * .2f));

        if (canApplyShock) _target.setUpShockStrikeDamage(Mathf.RoundToInt(_lightlingDamage * .1f));

        _target.applyAilment(canApplyIgnite, canApplyChill, canApplyShock);
    }

    private int checkMagicalResistanceTarget(CharacterStat _target)
    {
        return _target.magicResistance.getValue() + (_target.intelligence.getValue() * 3);
    }

    public void applyAilment(bool _isIgnited, bool _isChilled, bool _isShocked)
    {
        //if (isIgnited || isChilled || isShocked) return;
        bool canApplyIgnite = !isIgnited && !isChilled && !isShocked;
        bool canApplyChill = !isIgnited && !isChilled && !isShocked;
        bool canApplyShock = !isIgnited && !isChilled;

        float slowPercentage = .3f;
        if (_isIgnited && canApplyIgnite)
        {
            isIgnited = _isIgnited;
            ignitedTimer = fxDuration;
            fx.igniteFxFor(fxDuration);
        }
        if (_isChilled && canApplyChill)
        {
            isChilled = _isChilled;
            chilledTimer = fxDuration;
            fx.chillFxFor(fxDuration);
            GetComponent<Entity>().slowEntity(slowPercentage, fxDuration);
        }
        if (_isShocked && canApplyShock)
        {
            if (!isShocked)
            {
                applyShock(_isShocked);
            }
            else
            {
                if (GetComponent<Player>() != null) return;
                hitNearestTargetWithShockStrike();
            }
        }
    }

    public void applyShock(bool _isShocked)
    {
        if (isShocked) return;

        isShocked = _isShocked;
        shockedTimer = fxDuration;
        fx.lightingFxFor(fxDuration);
    }
    private void applyIgniteAilment()
    {
        if (igniteDamageTimer < 0)
        {
            currentHealth -= igniteDamage;
            if (currentHealth <= 0 && !isDead)
            {
                isDead = true;
                Die();
            }
            igniteDamageTimer = igniteDamageCooldown;
        }
    }

    private void hitNearestTargetWithShockStrike()
    {
        float closestDistance = Mathf.Infinity;
        Transform enemyClosest = null;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 25);
        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null && Vector2.Distance(transform.position, hit.transform.position) > 1)
            {
                float distanceToEnemy = Vector2.Distance(transform.position, hit.transform.position);
                if (distanceToEnemy < closestDistance)
                {
                    enemyClosest = hit.transform;
                    closestDistance = distanceToEnemy;
                }
            }
        }

        if (enemyClosest == null)
        {
            enemyClosest = transform;
        }
        GameObject newShockStrike = Instantiate(shockStrikePrefab, transform.position, Quaternion.identity);
        newShockStrike.GetComponent<ShockStrike_Controller>().setUpShockStrike(shockDamage, enemyClosest.GetComponent<CharacterStat>());
    }
    public void setUpIgniteDamage(int _damage) => igniteDamage = _damage;
    public void setUpShockStrikeDamage(int _damage) => shockDamage = _damage;
    #endregion
    public int getMaxHealthValue() => maxHealth.getValue() + vitality.getValue() * 5;
    public virtual Stat getModify(StatType _statType)
    {
        if (_statType == StatType.strength) return strength;
        else if (_statType == StatType.agility) return agility;
        else if (_statType == StatType.intelligence) return intelligence;
        else if (_statType == StatType.vitality) return vitality;
        else if (_statType == StatType.armor) return armor;
        else if (_statType == StatType.maxHealth) return maxHealth;
        else if (_statType == StatType.evasion) return evasion;
        else if (_statType == StatType.damage) return damage;
        else if (_statType == StatType.critChance) return critChance;
        else if (_statType == StatType.critPower) return critPower;
        else if (_statType == StatType.magicResistance) return magicResistance;
        else if (_statType == StatType.fireDamage) return fireDamage;
        else if (_statType == StatType.iceDamage) return iceDamage;
        else if (_statType == StatType.lightingDamage) return lightingDamage;

        return null;
    }
    public void makeInvincible(bool _value) => isInvincible = _value;

}
