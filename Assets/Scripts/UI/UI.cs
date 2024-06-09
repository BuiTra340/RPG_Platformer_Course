using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI : MonoBehaviour, ISaveManager
{
    [SerializeField] private GameObject characterUI;
    [SerializeField] private GameObject skillTreeUI;
    [SerializeField] private GameObject craftUI;
    [SerializeField] private GameObject optionUI;
    [SerializeField] private GameObject inGameUI;
    public FadeScene fadeScene;
    [SerializeField] private TextMeshProUGUI endText;
    [SerializeField] private Button playAgainButton;
    public UI_ItemToolTip itemToolTip;
    public UI_StatToolTip statToolTip;
    public UI_CraftWindow craftWindow;
    public UI_SkillToolTip skillToolTip;

    [SerializeField] private UI_VolumeSlider[] volumeSettings;
    private void Awake()
    {
        switchTo(skillTreeUI);// call this to assign events in skill tree slots before assign events on skill
    }
    private void Start()
    {
        switchTo(inGameUI);

        itemToolTip.gameObject.SetActive(false);
        statToolTip.gameObject.SetActive(false);
        skillToolTip.gameObject.SetActive(false);
        fadeScene.gameObject.SetActive(true);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
            switchWithKeyTo(characterUI);

        if (Input.GetKeyDown(KeyCode.B))
            switchWithKeyTo(skillTreeUI);

        if (Input.GetKeyDown(KeyCode.O))
            switchWithKeyTo(craftUI);

        if (Input.GetKeyDown(KeyCode.L))
            switchWithKeyTo(optionUI);
    }
    public void switchTo(GameObject _menu)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }
        if (_menu != null)
        {
            _menu.SetActive(true);
            if (_menu != inGameUI && !_menu.activeSelf)
            {
                _menu.GetComponent<UI_Zoom>().zoomIn();
            }
        }
        AudioManager.instance.PlaySFX(7, null);
        checkForPauseGame(_menu);
    }

    private void checkForPauseGame(GameObject _menu)
    {
        if (_menu == inGameUI)
            GameManager.instance.pauseGame(false);
        else GameManager.instance.pauseGame(true);
    }

    private void switchWithKeyTo(GameObject _menu)
    {
        if (_menu != null && _menu.activeSelf)
        {
            _menu.GetComponent<UI_Zoom>().zoomOut();
            return;
        }
        
        switchTo(_menu);
    }

    public void checkForZoomOutUI(GameObject _menu)
    {
        _menu.SetActive(false);
        checkForIngameUI();
    }

    private void checkForIngameUI()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).gameObject.activeSelf)
            {
                return;
            }
        }
        switchTo(inGameUI);
    }
    public void showBannerDie()
    {
        StartCoroutine(disPlayEndScene());
    }
    private IEnumerator disPlayEndScene()
    {
        yield return new WaitForSeconds(1);
        fadeScene.fadeOut();
        yield return new WaitForSeconds(1.5f);
        endText.gameObject.SetActive(true);
        playAgainButton.gameObject.SetActive(true);

    }

    public void saveData(ref GameData _data)
    {
        _data.sliderVolume.Clear();
        foreach (var item in volumeSettings)
        {
            _data.sliderVolume.Add(item.parameter, item.slider.value);
        }
    }

    public void loadData(GameData _data)
    {
        foreach (var pair in _data.sliderVolume)
        {
            foreach (var item in volumeSettings)
            {
                if (item.parameter == pair.Key)
                {
                    item.loadVolume(pair.Value);
                }
            }
        }
    }
}
