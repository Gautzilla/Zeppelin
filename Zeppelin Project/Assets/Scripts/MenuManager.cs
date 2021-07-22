using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class MenuManager : MonoBehaviour
{
    #region Déclaration des variables

    public Canvas settings;
    public CanvasScaler settingsScaler;
    public GameObject settingsFirstButton;
    public GameObject menuCanvas;
    public GameObject title;
    static public bool displaySettings = false;
    public ButtonManager settingsButtonManager;

    #endregion

    #region Initialisation

    private void Start()
        {
            if (displaySettings)
            {
                Invoke("DisplaySettings", 0.01f);
                displaySettings = false;
            }
        }

    #endregion

    #region Affichage menu / paramètres

    public void Menu()
    {
        settings.enabled = false;
        settingsScaler.enabled = false;

        menuCanvas.SetActive(true);
        title.SetActive(true);
    }

    public void DisplaySettings()
    {
        settings.enabled = true;
        settingsScaler.enabled = true;
        EventSystem.current.SetSelectedGameObject(settingsFirstButton); // Pour navigation menu à la manette

        settingsButtonManager.ManageAnimator(true); // Animation d'entrée des bouttons

        menuCanvas.SetActive(false);
        title.SetActive(false);
    }

    #endregion

    #region Commandes du menu 

    public void StartGame()
    {
        SceneManager.LoadScene(Settings.levelToLoad);
    }

    

    public void Quit()
    {
        Application.Quit();
    }

    #endregion

    

    

    
}
