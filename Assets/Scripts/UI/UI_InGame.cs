using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_InGame : MonoBehaviour
{
    [SerializeField] private PlayerStats playerStats;
    [SerializeField] private Slider slider;
    [SerializeField] private Image dashCooldownImage;
    [SerializeField] private Image swordCooldownImage;
    [SerializeField] private Image blackHoleCooldownImage;
    [SerializeField] private Image flaskCooldownImage;
    [SerializeField] private Image crystalCooldownImage;
    [SerializeField] private Image parryCooldownImage;

    [Header("Souls info")]
    [SerializeField] private TextMeshProUGUI currencyText;
    [SerializeField]private float soulAmount;
    [SerializeField] private float increaseRate = 100;
    private SkillManager skill;
    private void Start()
    {
        if (playerStats != null)
            playerStats.onHealthUpdated += updateHealthBarUI;
        skill = SkillManager.instance;
    }
    private void Update()
    {
        updateSoulUI();

        if (Input.GetKeyDown(KeyCode.LeftShift) && skill.dash.dashUnlock)
            setCooldownOf(dashCooldownImage);

        if (Input.GetMouseButtonUp(1) && skill.sword.swordUnlocked && !PlayerManager.instance.player.sword)
            setCooldownOf(swordCooldownImage);

        if (Input.GetKeyDown(KeyCode.R) && skill.blackHole.blackHoleUnlocked)
            setCooldownOf(blackHoleCooldownImage);

        if (Input.GetKeyDown(KeyCode.K) && skill.crystal.crystalUnlocked)
            setCooldownOf(crystalCooldownImage);

        if (Input.GetKeyDown(KeyCode.Q) && skill.parry.parryUnlocked)
            setCooldownOf(parryCooldownImage);

        if (Input.GetKeyDown(KeyCode.Alpha1) && Inventory.instance.getEquipment(EquipmentType.Flask) != null)
            setCooldownOf(flaskCooldownImage);

        checkCooldownOf(dashCooldownImage, skill.dash.cooldown);
        checkCooldownOf(swordCooldownImage, skill.sword.cooldown);
        checkCooldownOf(blackHoleCooldownImage, skill.blackHole.cooldown);
        checkCooldownOf(crystalCooldownImage, skill.crystal.cooldown);
        checkCooldownOf(parryCooldownImage, skill.parry.cooldown);
        checkCooldownOf(flaskCooldownImage, Inventory.instance.flaskCooldown);
    }

    private void updateSoulUI()
    {
        if (soulAmount < PlayerManager.instance.currentCurrency())
        {
            soulAmount += increaseRate * Time.deltaTime;
        }
        else soulAmount = PlayerManager.instance.currency;
        currencyText.text = ((int)soulAmount).ToString();
    }

    private void updateHealthBarUI()
    {
        slider.maxValue = playerStats.getMaxHealthValue();
        slider.value = playerStats.currentHealth;
    }
    private void setCooldownOf(Image _cooldownImage)
    {
        if (_cooldownImage.fillAmount <= 0)
            _cooldownImage.fillAmount = 1;
    }
    private void checkCooldownOf(Image _cooldownImage, float _cooldown)
    {
        if (_cooldownImage.fillAmount > 0)
            _cooldownImage.fillAmount -= 1 / _cooldown * Time.deltaTime;
    }
}
