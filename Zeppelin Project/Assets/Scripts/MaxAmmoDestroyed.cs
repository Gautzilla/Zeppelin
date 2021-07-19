using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MaxAmmoDestroyed : MonoBehaviour
{
    public Settings settings;

    void Start()
    {
        InputField input = gameObject.GetComponent<InputField>();
        input.onEndEdit.AddListener(settings.MaxAmmoDestroyed);
    }
}
