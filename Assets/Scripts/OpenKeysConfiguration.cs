using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenKeysConfiguration : MonoBehaviour
{
    public GameObject menu;
    public GameObject keysConfiguration;

    public void KeysConfiguration()
    {
        keysConfiguration.SetActive(true);
        gameObject.SetActive(false);
        menu.SetActive(false);
    }
}
