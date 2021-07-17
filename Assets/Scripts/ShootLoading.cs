using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShootLoading : MonoBehaviour
{
    public Slider slider;
    public Image fill;

    public Color[] fillColors = new Color[2] { new Color(0.74f, 0.46f, 0.46f, 1f), new Color(1f, 1f, 1f, 1f)};

    public void DisplayShootLoading(float loading)
    {
        slider.value = loading;
    }

    public void fillColor(bool isReadyToShoot)
    {
        if (!isReadyToShoot)
        {
            fill.color = fillColors[0];
        } else
        {
            fill.color = fillColors[1];
        }
    }
}
