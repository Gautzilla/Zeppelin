using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameResults : MonoBehaviour
{

    #region Déclaration des variables

    public Text winnerName;
    public GameManager gameManager;
    public GameObject players;

    #endregion

    #region Initialisation
    void Start()
    {
        if (gameManager.deadPlayers == Settings.numberOfPlayers)
        {
            winnerName.GetComponent<Text>().text = "Nobody won";
        } else
        {
            int winner = players.transform.GetChild(0).GetComponent<PlayerMovement>().playerIdentifier; // Identifiant du seul joueur encore présent

            // Modification du nom et de la couleur à afficher

            winnerName.GetComponent<Text>().text = GameManager.playerNames[winner] + " won";
            Color textColor = Settings.configurableColors[Settings.playerColors[winner]];
            textColor.a = 0f;
            winnerName.GetComponent<Text>().color = textColor;

            // Le reste se fait dans l'animation du texte
        }
    }

    #endregion

    #region Commandes de fin de partie

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Menu()
    {
        SceneManager.LoadScene("Menu");
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void GoToSettings()
    {
        MenuManager.displaySettings = true;
        SceneManager.LoadScene("Menu");
    }

    #endregion

}
