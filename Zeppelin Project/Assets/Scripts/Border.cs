using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Border : MonoBehaviour
{

    #region Déclaration des variables

    private bool isInside = true; // Joueur à l'intérieur de l'aire de jeu
    private Rigidbody rb;

    public float forceModifier = 0f;
    public PlayerMovement movement;

    #endregion

    #region Initialisation

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    #endregion

    #region Champ de force

    // Si le joueur sort de l'aire de jeu : il est poussé vers l'intérieur
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Border") && !movement.isFalling) // Sauf si le joueur est détruit
        {
            isInside = false;
            StartCoroutine("PushPlayerIn");
        }
    }

    IEnumerator PushPlayerIn()
    {
        while (!isInside)
        {
            Vector3 rayDirection = new Vector3(-transform.position.x, 0f, -transform.position.z);
            rb.AddForce(rayDirection * forceModifier * Time.deltaTime, ForceMode.VelocityChange);
            yield return null;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Border"))
        {
            isInside = true;
        }
    }

    #endregion

}
