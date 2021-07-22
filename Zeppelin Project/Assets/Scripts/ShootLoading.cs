using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShootLoading : MonoBehaviour
{
    #region Déclaration des variables

    public Slider slider;
    public Image fill;

    public Color[] fillColors = new Color[2] { new Color(0.74f, 0.46f, 0.46f, 1f), new Color(1f, 1f, 1f, 1f) }; // Couleurs pendant et après le chargement

    #endregion

    #region Affichage de la barre de chargement

    public void DisplayShootLoading(float loading)
    {
        slider.value = loading;
    }

    public void fillColor(bool isReadyToShoot)
    {
        fill.color = isReadyToShoot ? fillColors[1] : fillColors[0];
    }

    #endregion
}
