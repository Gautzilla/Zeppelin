using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class Settings : MonoBehaviour
{
    #region Déclaration des variables

    [Header("Players")]
    public int playerToConfigure = 0; // Indice du joueur dont on change les paramètres
    public static int numberOfPlayers = 2;
    public Image[] numberOfPlayersButtons;
    public Image[] playerConfigurationButtons;
    public Image[] playerSettingsColor;
    public InputField playerName;
    public GameObject playerSettingsButtonToSelect;

    static public float playerHeight = 3f; // Hauteur des joueurs par rapport au sol

    [Header("Player Colors")]
    public Image[] colors;
    static public Color[] configurableColors = new Color[6] { new Color(1f, 0.46f, 0.52f, 1f), new Color(0.46f, 0.77f, 1f, 1f), new Color(0.49f, 1f, 0.46f, 1f), new Color(1f, 0.82f, 0.46f, 1f), new Color(1f, 0.46f, 0.93f, 1f), Color.red }; // Couleurs par défaut
    static public int[] playerColors = new int[4] { 0, 1, 2, 3 }; // À chaque joueur est attribuée la couleur de l'indice correspondant dans le tableau configurableColors

    [Header("Destroyed Ammo Cleaning")]
    public Text numberOfDestroyedAmmo;
    static public int maxNumberOfDestroyedAmmo = 1000;
    static bool maxNumberOfDestroyedAmmoHasBeenChanged = false;

    [Header("Game Mode")]
    static public int levelToLoad = 1;

    #endregion

    #region Initialisation

    void Start()
    {
        NumberOfPlayersButtonColor(numberOfPlayers);

        for (int i = 0; i < configurableColors.Length; i++)
        {
            configurableColors[i] = colors[i].color; // On récupère les couleurs configurées sur Unity (image des ColorButtons)
        }

        for (int i = 3; i >= 0; i--)
        {
            DisplayPlayerConfigurationButtonColor(i);
        }

        if (maxNumberOfDestroyedAmmoHasBeenChanged) // La fonction n'est plus disponible ingame (seulement paramètres Unity)
        {
            numberOfDestroyedAmmo.text = maxNumberOfDestroyedAmmo.ToString();
        }
    }

    #endregion

    #region Changement de couleur des boutons de l'interface

    void NumberOfPlayersButtonColor(int num) // Affiche en bleu le bouton du nombre de joueurs sélectionné, en blanc les autres boutons disponibles
    {
        int i = 0;

        foreach (Image image in numberOfPlayersButtons)
        {
            if (i == num - 2) // Le premier bouton est pour 2 joueurs
            {
                image.color = new Color(0.52f, 0.92f, 0.8f, 1f);
            }
            else
            {
                image.color = Color.white;
            }
            i++;
        }
    }

    void DisplayPlayerConfigurationButtonColor(int playerIdentifier) // Couleur des boutons pour les paramètres de chaque joueur
    {
        int i = 0;

        foreach (Image image in playerConfigurationButtons) // Sélection du joueur à configurer
        {
            image.color = configurableColors[playerColors[i]];
            i++;
        }

        foreach (Image image in playerSettingsColor) // Paramètres du joueur
        {
            image.color = configurableColors[playerColors[playerIdentifier]];
        }
    }

    #endregion

    // Méthodes appelées par les boutons de l'interface

    #region Paramètres généraux

    public void SetNumberOfPlayers(int num)
    {
        numberOfPlayers = num;
        NumberOfPlayersButtonColor(num);
    }

    public void GameMode(int gameMode)
    {
        levelToLoad = gameMode;
    }

    public void MaxAmmoDestroyed(string maxAmmo) // Fonction plus disponible dans l'interface
    {
        maxNumberOfDestroyedAmmoHasBeenChanged = true;
        if (int.TryParse(maxAmmo, out int number))
        {
            maxNumberOfDestroyedAmmo = number;
        }
    }

    #endregion

    #region Paramètres du joueur

    public void PlayerConfiguration(int playerIdentifier)
    {
        playerToConfigure = playerIdentifier;
        DisplayPlayerConfigurationButtonColor(playerIdentifier);
        DisplayPlayerName(playerIdentifier);
        EventSystem.current.SetSelectedGameObject(playerSettingsButtonToSelect); // Pour navigation plus aisée à la manette
    }

    public void PlayerColor(int colorIdentifier)
    {
        playerColors[playerToConfigure] = colorIdentifier;
        DisplayPlayerConfigurationButtonColor(playerToConfigure);
    }

    public void PlayerName(string playerName)
    {
        GameManager.playerNames[playerToConfigure] = playerName;
    }

    void DisplayPlayerName(int playerIdentifier)
    {
        playerName.text = GameManager.playerNames[playerIdentifier];
    }

    #endregion
}
