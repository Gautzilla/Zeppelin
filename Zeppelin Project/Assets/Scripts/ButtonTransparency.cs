using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonTransparency : MonoBehaviour
{

    #region Initialisation

    void Start()
    {
        GetComponent<Image>().alphaHitTestMinimumThreshold = 1f; // Boutons triangulaires pour la sélection de couleur du joueur
    }

    #endregion

}

