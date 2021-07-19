using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{

    #region Déclaration des variables

    public Rigidbody rb;
    public float climbSpeed;
    public GameObject explosion;
    public GameObject explosionEffect;

    #endregion

    #region Amorçage et montée

    private void OnCollisionEnter(Collision collisionInfo)
    {
        if (collisionInfo.collider.CompareTag("Ground"))
        {
            StartCoroutine(Climb());
        }
    }

    IEnumerator Climb()
    {
        float impactHeight = transform.position.y;

        while (transform.position.y <= impactHeight + Settings.playerHeight)
        {
            rb.AddForce(0f, climbSpeed * Time.deltaTime, 0f, ForceMode.VelocityChange);
            yield return null;
        }

        // Change les paramètres de la bombe pour que l'onde de choc soit statique

        rb.collisionDetectionMode = CollisionDetectionMode.Discrete;
        rb.isKinematic = true;

        Explode();
    }

    #endregion

    #region Explosion

    private void Explode()
    {
        // Désactivation de l'affichage de la bombe et on réinitialisation de sa rotation

        gameObject.GetComponent<Renderer>().enabled = false;
        gameObject.GetComponent<BoxCollider>().enabled = false;
        transform.rotation = Quaternion.Euler(0f, 0f, 0f);

        // Instanciation de l'effet d'explosion et de l'onde de choc

        GameObject explosionEff = Instantiate(explosionEffect, transform);
        explosionEff.transform.parent = null;
        explosionEff.transform.localScale = new Vector3(1f, 1f, 1f);
        explosion.SetActive(true);
    }

    #endregion

}
