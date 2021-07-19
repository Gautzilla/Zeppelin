using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class Settings : MonoBehaviour
{

    [Header("Players")]
    public int playerToConfigure = 0;
    public static int numberOfPlayers = 2;
    public Image[] numberOfPlayersButtons;
    public Image[] playerConfigurationButtons;
    public Image[] playerSettingsColor;
    public InputField playerName;
    public GameObject playerSettingsButtonToSelect;

    static public float playerHeight = 3f;

    [Header("Player Colors")]
    public Image[] colors;
    static public Color[] configurableColors = new Color[6] { new Color(1f, 0.46f, 0.52f, 1f), new Color(0.46f, 0.77f, 1f, 1f), new Color(0.49f, 1f, 0.46f, 1f), new Color(1f, 0.82f, 0.46f, 1f), new Color(1f, 0.46f, 0.93f, 1f), Color.red};
    static public int[] playerColors = new int[4] { 0, 1, 2, 3 };

    [Header("Destroyed Ammo Cleaning")]
    public Text numberOfDestroyedAmmo;
    static public int maxNumberOfDestroyedAmmo = 1000;
    static bool maxNumberOfDestroyedAmmoHasBeenChanged = false;

    [Header("Game Mode")]
    static public int levelToLoad = 1;


    void Start()
    {
        NumberOfPlayersButtonColor(numberOfPlayers);

        for (int i = 0; i < configurableColors.Length; i++)
        {
            configurableColors[i] = colors[i].color;
        }

        for (int i = 3; i >= 0; i--)
        {
            DisplayPlayerConfigurationButtonColor(i);
        }
        
        if (maxNumberOfDestroyedAmmoHasBeenChanged)
        {
            numberOfDestroyedAmmo.text = maxNumberOfDestroyedAmmo.ToString();
        }
    }

    public void SetNumberOfPlayers(int num)
    {
        numberOfPlayers = num;
        NumberOfPlayersButtonColor(num);
    }

    void NumberOfPlayersButtonColor(int num)
    {
        int i = 0;
        foreach (Image image in numberOfPlayersButtons)
        {
            if (i == num - 2)
            {
                image.color = new Color (0.534907f, 0.893f, 0.893f, 1f);
            } else
            {
                image.color = Color.white;
            }
            i++;
        }
    }

    public void PlayerConfiguration(int playerIdentifier)
    {
        playerToConfigure = playerIdentifier;
        DisplayPlayerConfigurationButtonColor(playerIdentifier);
        DisplayPlayerName(playerIdentifier);
        EventSystem.current.SetSelectedGameObject(playerSettingsButtonToSelect);
    }

    void DisplayPlayerConfigurationButtonColor(int playerIdentifier)
    {
        int i = 0;
        foreach (Image image in playerConfigurationButtons)
        {
                image.color = configurableColors[playerColors[i]];
                i++;
        }
        
        foreach (Image image in playerSettingsColor)
        {
            image.color = configurableColors[playerColors[playerIdentifier]];
        }
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

    public void GameMode (int gameMode)
    {
        levelToLoad = gameMode;
    }

    public void MaxAmmoDestroyed(string maxAmmo)
    {
        maxNumberOfDestroyedAmmoHasBeenChanged = true;
        if (int.TryParse(maxAmmo, out int number))
        { 
            maxNumberOfDestroyedAmmo = number;
        }
    }
}
