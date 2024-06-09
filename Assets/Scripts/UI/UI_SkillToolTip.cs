using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UI_SkillToolTip : UI_ToolTip
{
    [SerializeField] private TextMeshProUGUI skillNameText;
    [SerializeField] private TextMeshProUGUI skillDescriptionText;
    [SerializeField] private TextMeshProUGUI skillCostText;

    public void showSkillToolTip(string _skillName,string _skillDescription,int _skillCost)
    {
        skillNameText.text = _skillName;
        skillDescriptionText.text = _skillDescription;
        skillCostText.text = "Cost : " + _skillCost.ToString();
        gameObject.SetActive(true);
        adjustPosition();
    }
    public void hideSkillToolTip() => gameObject.SetActive(false);
}
