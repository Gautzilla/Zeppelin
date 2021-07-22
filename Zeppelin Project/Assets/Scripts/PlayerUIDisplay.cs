using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUIDisplay : MonoBehaviour
{
    #region Déclaration des variables

    [Header("Player Identifier")]
    public int playerIdentifier;

    [Header("Player Name")]
    public Text playerNameDisplay;

    [Header("UI Borders")]
    public Image playerNameBorder;

    [Header("Weapon Icon")]
    public Sprite[] weaponIcon = new Sprite[2];
    public Image weaponToDisplay;

    [Header("Ammo Icons")]
    public Image[] ammos = new Image[5];
    public Sprite[] ammoState = new Sprite[2];
    public GameObject shootLoadingBar;
    public Image loadingBarBorder;

    #endregion

    #region Initialisation

    void Start()
    {
        playerNameDisplay.text = GameManager.playerNames[playerIdentifier];

        foreach (Image ammo in ammos) // Au départ : projectiles de base : munitions infinies
        {
            ammo.enabled = false;
        }

        Color playerColor = Settings.configurableColors[Settings.playerColors[playerIdentifier]];
        playerNameBorder.color = playerColor;
        loadingBarBorder.color = playerColor;
    }

    #endregion

    #region Affichage des nouvelles armes et munitions

    public void DisplayWeaponIcon(int weapon)
    {
        weaponToDisplay.sprite = weaponIcon[weapon];

        if (weapon == 0)
        {
            shootLoadingBar.SetActive(true); // Barre de puissance pour les projectiles de base
            foreach (Image ammo in ammos)
            {
                ammo.enabled = false;
            }
        }
        else
        {
            shootLoadingBar.SetActive(false);
            foreach (Image ammo in ammos) // Affiche les munitions pour les torpilles et bombes
            {
                ammo.enabled = true;
            }
        }
    }

    public void DisplayAmmo(int ammoStock, int ammoShot) 
    {
        int i = 0;
        foreach (Image ammo in ammos)
        {
            if (i < ammoStock - ammoShot) // Munition disponible
            {
                ammo.sprite = ammoState[1];
            }
            else if (i >= ammoStock) // Pour les armes qui ont moins de 5 munitions
            {
                ammo.enabled = false;
            }
            else // Munition déjà tirée
            {
                ammo.sprite = ammoState[0];
            }
            i++;
        }
    }

    #endregion
}
