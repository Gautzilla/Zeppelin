using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyWhenTooLow : MonoBehaviour
{
    // Détruit le GameObject si il sort du champ de vision de la caméra 

    void Update()
    {
        if (transform.position.y < -100f)
        {
            Destroy(gameObject);
        }
    }
}
