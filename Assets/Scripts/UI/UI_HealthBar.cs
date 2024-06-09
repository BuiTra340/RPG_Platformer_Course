using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_HealthBar : MonoBehaviour
{
    private Entity entity;
    private RectTransform rectTransform;
    private CharacterStat myStat;
    private Slider mySlider;
    // Start is called before the first frame update
    void Start()
    {
        entity = GetComponentInParent<Entity>();
        rectTransform = GetComponent<RectTransform>();
        myStat = GetComponentInParent<CharacterStat>();
        mySlider = GetComponentInChildren<Slider>();
        entity.onFlipped += flipUI;
        myStat.onHealthUpdated += updateHealthBarUI;
        
    }
    private void OnDisable()
    {
        entity.onFlipped -= flipUI;
        myStat.onHealthUpdated -= updateHealthBarUI;
    }
    private void updateHealthBarUI()
    {
        mySlider.maxValue = myStat.getMaxHealthValue();
        mySlider.value = myStat.currentHealth;
    }
    
    private void flipUI() => rectTransform.Rotate(0, 180, 0);
}
