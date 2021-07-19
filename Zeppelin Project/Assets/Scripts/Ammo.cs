using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ammo : MonoBehaviour
{

    #region Déclaration des variables

    public Rigidbody rb;
    public GameObject destroyedAmmoPrefab;
    public GameObject explosion;
    public AmmoColor ammoColor;
    public float rotationSpeed;

    #endregion

    #region Mouvement et explosion du projectile

    private void FixedUpdate()
    {
        transform.Rotate(new Vector3(rotationSpeed * Time.deltaTime, 0f, 0f), Space.Self);
    }

    private void OnCollisionEnter(Collision collision)
    {

        // Création de l'effet d'explosion

        GameObject thisExplosion = Instantiate(explosion, transform);
        ParticleSystem.MainModule explosionMain = thisExplosion.GetComponentInChildren<ParticleSystem>().main;
        ParticleSystem.TrailModule explosionTrails = thisExplosion.GetComponentInChildren<ParticleSystem>().trails;

        explosionMain.startLifetime = 3f;
        explosionTrails.lifetime = 0.1f;

        // Instanciation des résidus de projectile

        GameObject destroyedAmmo = Instantiate(destroyedAmmoPrefab, transform.position, Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y + 33f, transform.rotation.eulerAngles.z));
        foreach (Transform child in destroyedAmmo.transform)
        {
            child.GetComponent<Rigidbody>().AddForce(rb.velocity, ForceMode.VelocityChange);
            child.GetComponent<AmmoColor>().SetAmmoColor(ammoColor.ammoBlock);
        }

        // Suppression du projectile détruit

        transform.DetachChildren();
        Destroy(gameObject);

        // Ajout à la liste des projectiles détruits / suppression des plus anciens pour libérer de la ressource

        FindObjectOfType<GameManager>().RemoveDestroyedAmmo(destroyedAmmo);
    }

    #endregion
}
