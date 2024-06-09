using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_MainMenu : MonoBehaviour
{
    [SerializeField] private string sceneName;
    [SerializeField] private GameObject continueButton;
    private void Start()
    {
        Invoke("canShowButtonContinue", .1f);
    }

    private void canShowButtonContinue()
    {
        if (SaveManager.instance.hasSaveData() == false)
            continueButton.gameObject.SetActive(false);
    }

    public void Continue()
    {
        SceneManager.LoadScene(sceneName);
    }
    public void newGame()
    {
        SaveManager.instance.deleteSavedData();
        SceneManager.LoadScene(sceneName);
    }
    public void exitGame()
    {
        Application.Quit();
    }
}
