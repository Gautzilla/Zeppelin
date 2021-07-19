using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUIDisplay : MonoBehaviour
{
    [Header ("Player Identifier")]
    public int playerIdentifier;

    [Header ("Player Name")]
    public Text playerNameDisplay;

    [Header("UI Borders")]
    public Image playerNameBorder;

    [Header ("Weapon Icon")]
    public Sprite[] weaponIcon = new Sprite[2];
    public Image weaponToDisplay;

    [Header ("Ammo Icons")]
    public Image[] ammos = new Image[5];
    public Sprite[] ammoState = new Sprite[2];
    public GameObject shootLoadingBar;
    public Image loadingBarBorder;

    void Start()
    {
        playerNameDisplay.text = GameManager.playerNames[playerIdentifier];
        foreach (Image ammo in ammos)
        {
            ammo.enabled = false;
        }

        Color playerColor = Settings.configurableColors[Settings.playerColors[playerIdentifier]];
        playerNameBorder.color = playerColor;
        loadingBarBorder.color = playerColor;
    }

    public void DisplayWeaponIcon (int weapon)
    {
        weaponToDisplay.sprite = weaponIcon[weapon];

        if (weapon == 0)
        {
            shootLoadingBar.SetActive(true);
            foreach (Image ammo in ammos)
            {
                ammo.enabled = false;
            }
        }
        else
        {
            shootLoadingBar.SetActive(false);
            foreach (Image ammo in ammos)
            {
                ammo.enabled = true;
            }
        }
    }

    public void DisplayAmmo (int ammoStock, int ammoShot)
    {
        int i = 0;
        foreach (Image ammo in ammos)
        {
            if(i <= ammoStock - ammoShot)
            {
                ammo.sprite = ammoState[1];
            } else if (i > ammoStock)
            {
                ammo.enabled = false;
            } else
            {
                ammo.sprite = ammoState[0];
            }
            i++;
        }
    }

}
