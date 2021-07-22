using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenKeysConfiguration : MonoBehaviour
{

    #region Déclaration des variables

    public GameObject menu;
    public GameObject keysConfiguration;

    #endregion

    public void KeysConfiguration() // Ferme les paramètres du joueur et affiche les boutons de configuration de touches
    {
        keysConfiguration.SetActive(true);
        gameObject.SetActive(false);
        menu.SetActive(false);
    }
}
