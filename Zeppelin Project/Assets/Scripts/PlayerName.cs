using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerName : MonoBehaviour
{
    #region Déclaration des variables

    public Settings settings;

    #endregion

    #region Initialisation

    void Start() // Applique le changement de nom dans le script Settings.
    {
        InputField input = gameObject.GetComponent<InputField>();
        input.onEndEdit.AddListener(settings.PlayerName);
    }

    #endregion
}
