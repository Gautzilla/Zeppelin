using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MaxAmmoDestroyed : MonoBehaviour
{
    // Attribue le nombre maximum de résidus de projectile au script de paramètres

    public Settings settings;

    void Start()
    {
        InputField input = gameObject.GetComponent<InputField>();
        input.onEndEdit.AddListener(settings.MaxAmmoDestroyed);
    }
}
