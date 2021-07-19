using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class MenuManager : MonoBehaviour
{
    public Canvas settings;
    public CanvasScaler settingsScaler;
    public GameObject settingsFirstButton;
    public GameObject menuCanvas;
    public GameObject title;
    static public bool displaySettings = false;
    public ButtonManager settingsButtonManager;

    private void Start()
    {
        if (displaySettings)
        {
            Invoke("DisplaySettings", 0.01f);
            displaySettings = false;
        }
    }

    public void StartGame()
    {
        SceneManager.LoadScene(Settings.levelToLoad);
    }

    public void DisplaySettings()
    {
        settings.enabled = true;
        settingsScaler.enabled = true;
        EventSystem.current.SetSelectedGameObject(settingsFirstButton);

        settingsButtonManager.ManageAnimator(true);

        menuCanvas.SetActive(false);

        title.SetActive(false);
    }

    public void Menu()
    {
        settings.enabled = false;
        settingsScaler.enabled = false;

        menuCanvas.SetActive(true);

        title.SetActive(true);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
